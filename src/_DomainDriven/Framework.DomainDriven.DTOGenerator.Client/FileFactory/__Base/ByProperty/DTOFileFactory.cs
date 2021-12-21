using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public abstract class DTOFileFactory<TConfiguration, TFileType> : FileFactory<TConfiguration, TFileType>, IDTOSource<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
        where TFileType : DTOFileType
    {
        protected DTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public virtual CodeTypeReference CurrentInterfaceReference { get; }

        protected virtual bool? InternalBaseTypeContainsPropertyChange { get; }

        private bool? BaseTypeContainsPropertyChange => this.Configuration.ContainsPropertyChange ? this.InternalBaseTypeContainsPropertyChange : null;

        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            return base.GetBaseTypes().Concat(this.CurrentInterfaceReference.MaybeYield());
        }

        public IEnumerable<PropertyInfo> GetProperties(bool isWritable)
        {
            return this.Configuration.GetDomainTypeProperties(this.DomainType, this.FileType, isWritable);
        }

        private IEnumerable<CodeTypeMember> CreatePropertyMembers(CodeMethodReferenceExpression raisePropertyChangingMethodReference = null, CodeMethodReferenceExpression raisePropertyChangedMethodReference = null)
        {
            return from sourceProperty in this.GetProperties(false)

                   from member in this.CreatePropertyMember(sourceProperty, raisePropertyChangingMethodReference, raisePropertyChangedMethodReference)

                   select member;
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            foreach (var notifyMember in this.GetNotifyMembers())
            {
                yield return notifyMember;
            }

            foreach (var member in this.Configuration.GetFileFactoryExtendedMembers(this))
            {
                yield return member;
            }
        }

        private IEnumerable<CodeTypeMember> GetNotifyMembers()
        {
            switch (this.BaseTypeContainsPropertyChange)
            {
                case true:
                    return this.CreatePropertyMembers(CodeDomHelper.CreateBaseRaisePropertyChangingMethodReference(),
                                                      CodeDomHelper.CreateBaseRaisePropertyChangedMethodReference());

                case false:
                    {
                        var changingEventMember = CodeDomHelper.GenerateChangingEventMember();
                        var raisePropertyChangingMethod = CodeDomHelper.GenerateRaisePropertyChangingMethod(changingEventMember, !this.DomainType.IsValueType);

                        var changedEventMember = CodeDomHelper.GenerateChangedEventMember();
                        var raisePropertyChangedMethod = CodeDomHelper.GenerateRaisePropertyChangedMethod(changedEventMember, !this.DomainType.IsValueType);

                        var eventMembers = new CodeTypeMember[] { changingEventMember, raisePropertyChangingMethod, changedEventMember, raisePropertyChangedMethod };

                        var propertyMembers = this.CreatePropertyMembers(new CodeThisReferenceExpression().ToMethodReferenceExpression(raisePropertyChangingMethod.Name),
                                                                         new CodeThisReferenceExpression().ToMethodReferenceExpression(raisePropertyChangedMethod.Name));

                        return eventMembers.Concat(propertyMembers);
                    }

                default:
                    return this.CreatePropertyMembers();
            }
        }

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            foreach (var customAttribute in base.GetCustomAttributes())
            {
                yield return customAttribute;
            }

            yield return this.GetDataContractCodeAttributeDeclaration();
        }

        protected virtual CodeMemberField CreateFieldMember(PropertyInfo property, string fieldName)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));

            return new CodeMemberField
            {
                Name = fieldName,
                Type = this.CodeTypeReferenceService.GetCodeTypeReference(property, true),
                InitExpression = this.GetFieldInitExpression(property)
            };
        }

        protected virtual CodeExpression GetFieldInitExpression(PropertyInfo property)
        {
            return this.CodeTypeReferenceService.IsOptional(property)
                       ? this.CodeTypeReferenceService.GetCodeTypeReference(property).ToNothingValueExpression()
                       : property.GetCustomAttribute<DefaultValueAttribute>().Maybe(attr => attr.Value.ToDynamicPrimitiveExpression());
        }

        protected virtual IEnumerable<CodeAttributeDeclaration> GetPropertyCustomAttributes(PropertyInfo sourceProperty)
        {
            if (sourceProperty == null) throw new ArgumentNullException(nameof(sourceProperty));

            yield return typeof(DataMemberAttribute).ToTypeReference().ToAttributeDeclaration();
        }

        protected virtual CodeMemberProperty CreatePropertyMember(PropertyInfo sourceProperty, CodeMemberField fieldMember, Func<CodeExpression, CodeExpression> overrideMethodInfo, CodeMethodReferenceExpression raisePropertyChangingMethodReference, CodeMethodReferenceExpression raisePropertyChangedMethodReference)
        {
            var preSetVariableName = "newValue";

            var preSetVarRef = new CodeVariableReferenceExpression(preSetVariableName);

            var fieldRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldMember.Name);

            var baseFieldTypeRef = this.CodeTypeReferenceService.GetCodeTypeReference(sourceProperty);

            var justTypeRef = baseFieldTypeRef.ToJustReference();

            var justVarName = "just" + sourceProperty.Name;

            var getJustVarDecl = new CodeVariableDeclarationStatement(justTypeRef, justVarName, fieldRef.ToAsCastExpression(justTypeRef));
            var setJustVarDecl = new CodeVariableDeclarationStatement(justTypeRef, justVarName, new CodePropertySetValueReferenceExpression().ToAsCastExpression(justTypeRef));

            var justVarDeclRef = new CodeVariableReferenceExpression(justVarName);


            var getStatement =

                this.CodeTypeReferenceService.IsOptional(sourceProperty)

                    ? new CodeStatement[]
                      {
                          getJustVarDecl,
                          new CodeNotNullConditionStatement (justVarDeclRef)
                              {
                                  TrueStatements =
                                  {
                                      justTypeRef.ToObjectCreateExpression(justVarDeclRef.ToValueFieldReference().Pipe(overrideMethodInfo))
                                                 .ToMethodReturnStatement()
                                  },
                                  FalseStatements =
                                  {
                                      baseFieldTypeRef.ToNothingValueExpression().ToMethodReturnStatement()
                                  }
                              }
                      }.Composite()

                    : overrideMethodInfo(fieldRef).ToMethodReturnStatement();


            var setNewValueDecl = new CodeVariableDeclarationStatement(fieldMember.Type, preSetVariableName);
            var setNewValueDeclRef = new CodeTypeReferenceExpression(setNewValueDecl.Name);

            var fieldMemberRef = new CodeThisReferenceExpression().ToFieldReference(fieldMember);

            var postSetStatementCondition = typeof(object).ToTypeReferenceExpression()
                                           .ToMethodInvokeExpression("Equals", fieldMemberRef, preSetVarRef)
                                           .ToNegateExpression();

            var postSetStatement =

                this.BaseTypeContainsPropertyChange == null

              ? new CodeConditionStatement
                {
                    Condition = postSetStatementCondition,

                    TrueStatements = { preSetVarRef.ToAssignStatement(fieldMemberRef) }
                }
              : new CodeConditionStatement
                {
                    Condition = postSetStatementCondition,

                    TrueStatements =
                    {
                        raisePropertyChangingMethodReference.ToMethodInvokeExpression(new CodePrimitiveExpression(sourceProperty.Name)),

                        preSetVarRef.ToAssignStatement(fieldMemberRef),

                        raisePropertyChangedMethodReference.ToMethodInvokeExpression(new CodePrimitiveExpression(sourceProperty.Name)),
                    }
                };

            var setStatement = this.CodeTypeReferenceService.IsOptional(sourceProperty)

                             ? new CodeStatement[]
                               {
                                   setNewValueDecl,
                                   setJustVarDecl,

                                   new CodeNotNullConditionStatement (justVarDeclRef)
                                   {
                                       TrueStatements =
                                           {
                                               justTypeRef.ToObjectCreateExpression(justVarDeclRef.ToValueFieldReference().Pipe(overrideMethodInfo))
                                              .ToAssignStatement(setNewValueDeclRef)
                                           },
                                       FalseStatements =
                                           {
                                               baseFieldTypeRef.ToNothingValueExpression().ToAssignStatement(setNewValueDeclRef)
                                           }
                                   },

                                   postSetStatement
                               }.Composite()

                            : new CodeStatement[]
                              {
                                  setNewValueDecl,
                                  new CodePropertySetValueReferenceExpression().Pipe(overrideMethodInfo).ToAssignStatement(preSetVarRef),
                                  postSetStatement
                              }.Composite();

            return new CodeMemberProperty
            {
                Type = fieldMember.Type,
                Name = sourceProperty.Name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                GetStatements = { getStatement },
                SetStatements = { setStatement }
            }.Self(p => p.CustomAttributes.AddRange(this.GetPropertyCustomAttributes(sourceProperty).ToArray()));
        }

        private IEnumerable<CodeTypeMember> CreatePropertyMember(PropertyInfo sourceProperty, CodeMethodReferenceExpression raisePropertyChangingMethodReference, CodeMethodReferenceExpression raisePropertyChangedMethodReference)
        {
            if (sourceProperty == null) throw new ArgumentNullException(nameof(sourceProperty));


            if (!this.Configuration.ForceGenerateProperties(this.DomainType, this.FileType) && this.BaseTypeContainsPropertyChange == null)
            {
                yield return this.CreateFieldMember(sourceProperty, sourceProperty.Name)
                                 .Self(field => field.CustomAttributes.Add(typeof(DataMemberAttribute).ToTypeReference().ToAttributeDeclaration()))
                                 .Self(field => field.Attributes = MemberAttributes.Public);
            }
            else
            {
                var overrideMethodInfo = sourceProperty.GetOverrideValueExpression();

                var fieldMember = this.CreateFieldMember(sourceProperty, "_" + sourceProperty.Name.ToStartLowerCase());


                yield return fieldMember;
                yield return this.CreatePropertyMember(sourceProperty, fieldMember, overrideMethodInfo, raisePropertyChangingMethodReference, raisePropertyChangedMethodReference);
            }
        }

        DTOFileType IDTOSource.FileType => this.FileType;
    }
}

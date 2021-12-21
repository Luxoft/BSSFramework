using System;
using System.CodeDom;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Reactive;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public static class CodeDomHelper
    {
        private const string SourceParameterName = "source";
        private const string CopyIdParameterName = "copyId";

        public static CodeMemberEvent GenerateChangingEventMember()
        {
            return new CodeMemberEvent
                       {
                           Type = new CodeTypeReference(typeof(PropertyChangingEventHandler)),
                           Attributes = MemberAttributes.Public,
                           Name = "PropertyChanging"
                       };
        }

        public static CodeMemberMethod GenerateRaisePropertyChangingMethod(CodeMemberEvent changingEventMember, bool isProtected = true)
        {
            var propertyNameParameter = new CodeParameterDeclarationExpression(typeof(string), "propertyName");

            var defineChangingEventVariable = new CodeVariableDeclarationStatement(
                changingEventMember.Type, changingEventMember.Name.ToStartLowerCase(),
                new CodeEventReferenceExpression(
                    new CodeThisReferenceExpression(), changingEventMember.Name));

            return new CodeMemberMethod
            {
                Name = "RaisePropertyChanging",
                Attributes = isProtected ? MemberAttributes.Family | MemberAttributes.Final : MemberAttributes.Private | MemberAttributes.Final,
                ReturnType = new CodeTypeReference(typeof(void)),
                Parameters = { propertyNameParameter },
                Statements =
                    {
                        defineChangingEventVariable,

                        new CodeNotNullConditionStatement(new CodeVariableReferenceExpression (defineChangingEventVariable.Name))
                        {
                            TrueStatements =
                            {
                                new CodeDelegateInvokeExpression(
                                    new CodeVariableReferenceExpression(defineChangingEventVariable.Name),
                                    new CodeThisReferenceExpression (),
                                    new CodeObjectCreateExpression (typeof(PropertyChangingEventArgs),
                                                                    new CodeArgumentReferenceExpression(propertyNameParameter.Name)))
                            }
                        },
                    }
            };
        }



        public static CodeMemberEvent GenerateChangedEventMember()
        {
            return new CodeMemberEvent
            {
                Type = new CodeTypeReference(typeof(PropertyChangedEventHandler)),
                Attributes = MemberAttributes.Public,
                Name = "PropertyChanged"
            };
        }

        public static CodeMemberMethod GenerateRaisePropertyChangedMethod(CodeMemberEvent changingEventMember, bool isProtected = true)
        {
            var propertyNameParameter = new CodeParameterDeclarationExpression(typeof(string), "propertyName");

            var defineChangedEventVariable = new CodeVariableDeclarationStatement(
                changingEventMember.Type, changingEventMember.Name.ToStartLowerCase(),
                new CodeEventReferenceExpression(
                    new CodeThisReferenceExpression(), changingEventMember.Name));

            return new CodeMemberMethod
            {
                Name = "RaisePropertyChanged",
                Attributes = isProtected ? MemberAttributes.Family | MemberAttributes.Final : MemberAttributes.Private | MemberAttributes.Final,
                ReturnType = new CodeTypeReference(typeof(void)),
                Parameters = { propertyNameParameter },
                Statements =
                {
                    defineChangedEventVariable,

                    new CodeNotNullConditionStatement(new CodeVariableReferenceExpression (defineChangedEventVariable.Name))
                    {
                        TrueStatements =
                        {
                            new CodeDelegateInvokeExpression(
                                new CodeVariableReferenceExpression(defineChangedEventVariable.Name),
                                new CodeThisReferenceExpression (),
                                new CodeObjectCreateExpression (typeof(PropertyChangedEventArgs),
                                                                new CodeArgumentReferenceExpression(
                                                                    propertyNameParameter.Name)))
                        }
                    },
                }
            };
        }


        public static CodeMethodReferenceExpression CreateBaseRaisePropertyChangingMethodReference()
        {
            return new CodeMethodReferenceExpression(new CodeBaseReferenceExpression(), "RaisePropertyChanging");
        }

        public static CodeMethodReferenceExpression CreateBaseRaisePropertyChangedMethodReference()
        {
            return new CodeMethodReferenceExpression(new CodeBaseReferenceExpression(), "RaisePropertyChanged");
        }


        public static CodeMemberMethod GenerateToStringMethod(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new CodeMemberMethod
            {
                Name = "ToString",
                ReturnType = new CodeTypeReference(property.PropertyType),

                Attributes = MemberAttributes.Public | MemberAttributes.Override,

                Statements =
                {
                    new CodeMethodReturnStatement(
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(), property.Name))
                }
            };
        }


        public static CodeMemberMethod GenerateToCompareMethod(CodeTypeReference currentRef, PropertyInfo property)
        {
            if (currentRef == null) throw new ArgumentNullException(nameof(currentRef));
            if (property == null) throw new ArgumentNullException(nameof(property));

            var otherParameter = new CodeParameterDeclarationExpression(currentRef, "other");

            return new CodeMemberMethod
            {
                Name = "CompareTo",
                ReturnType = new CodeTypeReference(typeof(int)),
                Parameters = { otherParameter },

                Attributes = MemberAttributes.Public | MemberAttributes.Final,

                Statements =
                {
                    new CodeMethodReturnStatement(
                        new CodeMethodInvokeExpression(
                            new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), property.Name),
                            "CompareTo",
                            new CodePropertyReferenceExpression(new CodeArgumentReferenceExpression(otherParameter.Name), property.Name)))
                }
            };
        }







        public static CodeConstructor GeneratePersistentCloneConstructor<TConfiguration, TFileType>(this DTOFileFactory<TConfiguration, TFileType> fileFactory)
            where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
            where TFileType : DTOFileType
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var sourceParameter = new CodeParameterDeclarationExpression(fileFactory.CurrentInterfaceReference ?? fileFactory.CurrentReference, SourceParameterName);
            var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

            return new CodeConstructor
            {
                Parameters = { sourceParameter },
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                ChainedConstructorArgs = { sourceParameterRef, new CodePrimitiveExpression(true) }
            };
        }





        public static CodeTypeMember GenerateExplicitImplementationBaseRaise()
        {
            return new CodeMemberProperty
            {
                PrivateImplementationType = new CodeTypeReference(typeof(IBaseRaiseObject)),
                Name = "PropertyChanged",
                Type = new CodeTypeReference(typeof(PropertyChangedEventHandler)),

                GetStatements =
                {
                    new CodeMethodReturnStatement (
                        new CodeEventReferenceExpression(
                            new CodeThisReferenceExpression(),
                            "PropertyChanged"))
                }
            };
        }

        public static CodeTypeMember GenerateExplicitImplementationClone()
        {
            return new CodeMemberMethod
            {
                PrivateImplementationType = new CodeTypeReference(typeof(ICloneable)),
                Name = "Clone",
                ReturnType = new CodeTypeReference(typeof(object)),
                Statements =
                {
                    new CodeMethodReturnStatement (
                        new CodeMethodInvokeExpression(
                            new CodeThisReferenceExpression(),
                            "Clone"))
                }
            };
        }

        public static CodeMemberMethod GenerateCloneMethod(this CodeTypeReference currentCodeTypeReference, bool withNew)
        {
            if (currentCodeTypeReference == null) throw new ArgumentNullException(nameof(currentCodeTypeReference));

            return new CodeMemberMethod
            {
                Attributes = withNew ? MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.New
                                     : MemberAttributes.Public | MemberAttributes.Final,
                Name = "Clone",
                ReturnType = currentCodeTypeReference,
                Statements =
                {
                    new CodeMethodReturnStatement (
                        new CodeObjectCreateExpression(
                            currentCodeTypeReference, new CodeThisReferenceExpression()))
                }
            };
        }

        public static CodeConstructor GenerateUnpersistentCloneConstructor<TConfiguration, TFileType>(this DTOFileFactory<TConfiguration, TFileType> fileFactory)
            where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
            where TFileType : DTOFileType
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var sourceParameter = new CodeParameterDeclarationExpression(fileFactory.CurrentInterfaceReference ?? fileFactory.CurrentReference, SourceParameterName);
            var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();


            var constructor = new CodeConstructor
            {
                Parameters = { sourceParameter },
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
            };



            if ((fileFactory.FileType == ClientFileType.Class && fileFactory.DomainType.BaseType == typeof(object)) || fileFactory.FileType == FileType.BaseAbstractDTO)
            {
                constructor.Statements.Add(new CodeThrowArgumentNullExceptionConditionStatement(sourceParameter));
            }
            else
            {
                constructor.BaseConstructorArgs.AddRange(new[] { sourceParameterRef });
            }

            var cloneWithSecAssigner = new ClonePropertyAssigner<TConfiguration>(fileFactory, null)
                                      .WithSecurityToSecurity(fileFactory.CodeTypeReferenceService);


            var assignStatements = fileFactory.GetProperties(false).Select(property =>

               cloneWithSecAssigner.GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

            constructor.Statements.AddRange(assignStatements.ToArray());

            return constructor;
        }

        public static CodeConstructor GeneratePersistentCloneConstructorWithCopyIdParameter<TConfiguration, TFileType>(this DTOFileFactory<TConfiguration, TFileType> fileFactory)
            where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
            where TFileType : DTOFileType
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            var sourceParameter = new CodeParameterDeclarationExpression(fileFactory.CurrentInterfaceReference ?? fileFactory.CurrentReference, SourceParameterName);
            var sourceParameterRef = sourceParameter.ToVariableReferenceExpression();

            var copyIdParameter = new CodeParameterDeclarationExpression(typeof(bool), CopyIdParameterName);
            var copyIdParameterRef = new CodeVariableReferenceExpression(copyIdParameter.Name);

            var constructor = new CodeConstructor
            {
                Parameters = { sourceParameter, copyIdParameter },
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                BaseConstructorArgs = { sourceParameterRef }
            };

            if (fileFactory.FileType != FileType.BasePersistentDTO)
            {
                constructor.BaseConstructorArgs.Add(copyIdParameterRef);
            }

            var cloneWithSecAssigner = new ClonePropertyAssigner<TConfiguration>(fileFactory, copyIdParameter)
                                      .WithSecurityToSecurity(fileFactory.CodeTypeReferenceService);


            var assignStatements = fileFactory.GetProperties(false).Select(property =>

                cloneWithSecAssigner.GetAssignStatementBySource(property, sourceParameterRef, new CodeThisReferenceExpression()));

            constructor.Statements.AddRange(assignStatements.ToArray());

            return constructor;
        }

        public static CodeMemberMethod GeneratePersistentCloneMethodWithCopyIdParameter(this CodeTypeReference currentCodeTypeReference)
        {
            if (currentCodeTypeReference == null) throw new ArgumentNullException(nameof(currentCodeTypeReference));

            return new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = "Clone",
                ReturnType = currentCodeTypeReference,
                Parameters = { new CodeParameterDeclarationExpression(typeof(bool), CopyIdParameterName) },
                Statements =
                {
                    new CodeMethodReturnStatement (
                        new CodeObjectCreateExpression(
                            currentCodeTypeReference, new CodeThisReferenceExpression(), new CodeVariableReferenceExpression(CopyIdParameterName)))
                }
            };
        }
    }
}

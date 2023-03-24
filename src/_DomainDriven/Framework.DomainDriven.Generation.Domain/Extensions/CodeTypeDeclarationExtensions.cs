using System;
using System.CodeDom;

namespace Framework.DomainDriven.Generation.Domain;

public static class CodeTypeDeclarationExtensions
{
    public static CodeTypeReference WithGenerateInfo<TFileType>(this CodeTypeReference typeDecl, Type domainType, TFileType fileType)
    {
        if (typeDecl == null) throw new ArgumentNullException(nameof(typeDecl));

        typeDecl.UserData["DomainType"] = domainType;
        typeDecl.UserData["FileType"] = fileType;

        return typeDecl;
    }

    public static string GetServiceFacadeFileNameFunc(this CodeTypeDeclaration typeDecl, string systemName)
    {
        var suffix = ".Facade";
        if (typeDecl.IsInterface)
        {
            suffix = ".Facade.Interface";
        }
        else if (typeDecl.Name.EndsWith("Facade"))
        {
            suffix = ".Facade.Impl";
        }

        return systemName + suffix + ".Generated";
    }

    public static string GetIntegrationServiceFacadeFileNameFunc(this CodeTypeDeclaration typeDecl, string systemName)
    {
        var suffix = ".IntegrationFacade";
        if (typeDecl.IsInterface)
        {
            suffix = ".IntegrationFacade.Interface";
        }
        else if (typeDecl.Name.EndsWith("IntegrationFacade"))
        {
            suffix = ".IntegrationFacade.Impl";
        }

        return systemName + suffix + ".Generated";
    }


    public static string GetBLLFileNameFunc(this  CodeTypeDeclaration typeDecl, string systemName)
    {
        var suffix = ".BLL";
        if (typeDecl.Name.EndsWith("Factory"))
        {
            suffix = ".BLL.Factory";
        }
        else if (typeDecl.Name.EndsWith("FactoryContainer"))
        {
            suffix = ".BLL.FactoryContainer";
        }
        return systemName + suffix + ".Generated";

    }

    public static string GetBLLCoreFileNameFunc(this CodeTypeDeclaration typeDecl, string systemName)
    {
        var suffix = ".BLL.Core";
        if (typeDecl.Name.EndsWith("SecurityService"))
        {
            suffix = ".BLL.Core.SecurityService";
        }
        else if (typeDecl.IsInterface)
        {
            suffix = ".BLL.Core.Interface";
        }
        return systemName + suffix + ".Generated";

    }
    public static string GetDefaultGroupName(this CodeTypeDeclaration typeDecl, string systemName)
    {
        var suffix = "";
        if (typeDecl.Name.Contains("StrictDTO"))
        {
            suffix = ".StrictDTO";
        }
        if (typeDecl.Name.EndsWith("RichDTO"))
        {
            suffix = ".RichDTO";
        }
        if (typeDecl.Name.EndsWith("EventRichDTO"))
        {
            suffix = ".EventRichDTO";
        }
        if (typeDecl.Name.EndsWith("EventDTO"))
        {
            suffix = ".EventDTO";
        }
        if (typeDecl.Name.EndsWith("UpdateDTO"))
        {
            suffix = ".UpdateDTO";
        }
        if (typeDecl.Name.EndsWith("VisualDTO"))
        {
            suffix = ".VisualDTO";
        }
        if (typeDecl.Name.EndsWith("LambdaHelper"))
        {
            suffix = ".LambdaHelper";
        }
        if (typeDecl.Name.EndsWith("IntegrationRichDTO"))
        {
            suffix = ".IntegrationRichDTO";
        }
        if (typeDecl.Name.Contains("FullDTO"))
        {
            suffix = ".FullDTO";
        }
        if (typeDecl.Name.Contains("SimpleDTO"))
        {
            suffix = ".SimpleDTO";
        }
        if (typeDecl.Name.Contains("IdentityDTO"))
        {
            suffix = ".IdentityDTO";
        }
        if (typeDecl.Name.Contains("DTOMappingService"))
        {
            suffix = ".DTOMappingService" + (typeDecl.IsInterface ? ".Contract" : "");
        }

        return systemName + suffix + ".Generated";
    }
}

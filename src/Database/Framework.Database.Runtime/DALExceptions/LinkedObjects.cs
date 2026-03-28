namespace Framework.Database.DALExceptions;

public record LinkedObjects(Type Source, Type Target, string PropertyName);

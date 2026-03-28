namespace Framework.Database;

public record DBSessionSettings(DBSessionMode DefaultSessionMode = DBSessionMode.Write);

namespace Framework.Database.DALExceptions;

public class ArithmeticOverflowDALException(string args, string message) : DALException<string>(args, message);

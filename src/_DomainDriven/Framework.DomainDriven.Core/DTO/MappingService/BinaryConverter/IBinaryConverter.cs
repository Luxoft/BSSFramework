namespace Framework.DomainDriven
{
    public interface IBinaryConverter
    {
        byte[] GetBytes(string value);


        byte[] GetBytes(short value);

        byte[] GetBytes(int value);

        byte[] GetBytes(long value);


        byte[] GetBytes(float value);

        byte[] GetBytes(double value);

        byte[] GetBytes(decimal value);

        byte[] GetBytes(bool value);



        string GetString(byte[] data);


        short GetInt16(byte[] data);

        int GetInt32(byte[] data);

        long GetInt64(byte[] data);


        float GetSingle(byte[] data);

        double GetDouble(byte[] data);

        decimal GetDecimal(byte[] data);

        bool GetBoolean(byte[] data);
    }
}

using System.Text;

namespace Framework.DomainDriven;

public class BinaryConverter : IBinaryConverter
{
    public BinaryConverter()
    {
    }


    protected virtual Encoding Encoding
    {
        get { return Encoding.Unicode; }
    }

    private void CheckData(byte[] data, int expectedSize)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        if (data.Length != expectedSize)
        {
            throw new ArgumentException($"invalid data size: {data.Length}. Expected: {expectedSize}");
        }
    }


    public virtual byte[] GetBytes(string value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        return this.Encoding.GetBytes(value);
    }

    public virtual byte[] GetBytes(short value)
    {
        return BitConverter.GetBytes(value);
    }

    public virtual byte[] GetBytes(int value)
    {
        return BitConverter.GetBytes(value);
    }

    public virtual byte[] GetBytes(long value)
    {
        return BitConverter.GetBytes(value);
    }

    public byte[] GetBytes(float value)
    {
        return BitConverter.GetBytes(value);
    }

    public byte[] GetBytes(double value)
    {
        return BitConverter.GetBytes(value);
    }

    public byte[] GetBytes(decimal value)
    {
        throw new NotSupportedException();
    }

    public byte[] GetBytes(bool value)
    {
        return BitConverter.GetBytes(value);
    }


    public string GetString(byte[] data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        return this.Encoding.GetString(data);
    }

    public short GetInt16(byte[] data)
    {
        this.CheckData(data, sizeof (short));

        return BitConverter.ToInt16(data, 0);
    }

    public int GetInt32(byte[] data)
    {
        this.CheckData(data, sizeof(int));

        return BitConverter.ToInt32(data, 0);
    }

    public long GetInt64(byte[] data)
    {
        this.CheckData(data, sizeof(long));

        //return this.ReadOperation(r => r.ReadInt64(), data);
        return BitConverter.ToInt64(data, 0);
    }

    public float GetSingle(byte[] data)
    {
        this.CheckData(data, sizeof(float));

        //return this.ReadOperation(r => r.ReadSingle(), data);
        return BitConverter.ToSingle(data, 0);
    }

    public double GetDouble(byte[] data)
    {
        this.CheckData(data, sizeof(double));

        //return this.ReadOperation(r => r.ReadDouble(), data);
        return BitConverter.ToDouble(data, 0);
    }

    public decimal GetDecimal(byte[] data)
    {
        throw new NotSupportedException();
    }

    public bool GetBoolean(byte[] data)
    {
        this.CheckData(data, sizeof(bool));

        //return this.ReadOperation(r => r.ReadBoolean(), data);
        return BitConverter.ToBoolean(data, 0);
    }
}

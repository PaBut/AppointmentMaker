namespace AppointmentMaker.Domain.Extentions;

public static class ByteExtentions
{
    private const int bitsInByte = 8;

    public static bool[] ToBoolArray(this byte arg)
    {
        bool[] result = new bool[bitsInByte];

        for (int i = 0; i < bitsInByte; i++)
        {
            result[i] = Convert.ToBoolean(arg >> i & 1);
        }

        return result;
    }
}

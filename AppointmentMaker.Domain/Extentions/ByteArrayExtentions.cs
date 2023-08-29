namespace AppointmentMaker.Domain.Extentions;

public static class ByteArrayExtentions
{
    private const int bitsInByte = 8;

    public static bool[] ToBoolArray(this byte[] array)
    {
        bool[] result = new bool[array.Length * bitsInByte];

        for (int i = 0; i < array.Length; i++)
        {
            bool[] temp = array[i].ToBoolArray();

            int boolArrayIndex = i * bitsInByte;

            temp.CopyTo(result, boolArrayIndex);
        }

        return result;
    }
}

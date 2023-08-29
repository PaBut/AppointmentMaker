namespace AppointmentMaker.Domain.Extentions;

public static class BoolArrayExtentions
{
    private const int bitsInByte = 8;

    public static byte[] ToByteArray(this bool[] array)
    {
        int length = Convert.ToInt32(Math.Ceiling(array.Length / (double)bitsInByte));
        byte[] result = new byte[length];

        for (int i = 0; i < array.Length; i++)
        {
            int index = i / bitsInByte;
            int offset = i % bitsInByte;

            result[index] |= (byte)(Convert.ToByte(array[i]) << offset);
        }

        return result;
    }
}

public class Base16Encoder
{
    // Converting a byte array into a hexadecimal string
    public string ByteArrayToHexStrings(byte[] byteArray)
    {
        string[] hexStrings = new string[byteArray.Length];

        for (int i = 0; i < byteArray.Length; i++)
        {
            // Converting each byte to its hexadecimal representation
            hexStrings[i] = byteArray[i].ToString("X2"); 
        }
        // Joining all hex strings into one single string
        return string.Join("", hexStrings);
    }

    // Converting a hexadecimal string into a byte array
    public byte[] HexStringsToByteArray(string base16)
    {
        byte[] byteArray = new byte[base16.Length / 2];

        for (int i = 0; i < base16.Length; i += 2)
        {
            // Converting every two characters in the string into a byte and store it in the array
            byteArray[i / 2] = Convert.ToByte(base16.Substring(i, 2), 16);
        }
        return byteArray;
    }
}

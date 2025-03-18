using System;

namespace MorseCodeTranslator
{
    public class Base16Encoder
    {
        public static string ByteArrayToHexStrings(byte[] byteArray)
        {
            string[] hexStrings = new string[byteArray.Length];

            for (int i = 0; i < byteArray.Length; i++)
            {
                hexStrings[i] = byteArray[i].ToString("X2"); // Convert each byte to its hexadecimal representation
            }

            return string.Join("", hexStrings);
        }

        public static byte[] HexStringsToByteArray(string base16)
        {
            Console.WriteLine(base16);
            byte[] byteArray = new byte[base16.Length / 2];

            for (int i = 0; i < base16.Length; i += 2)
            {
                byteArray[i / 2] = Convert.ToByte(base16.Substring(i, 2), 16); // Convert each pair of characters to byte
            }
            Console.WriteLine(byteArray);
            return byteArray;
        }
    }
}
using System;
using System.Numerics;
using System.Text;

namespace MorseCodeTranslator
{
    public static class Base36Encoder
    {
        private const string Base36Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string Encode(byte[] data)
        {
            // Prefix data with a non-zero value to preserve leading zeros in BigInteger
            var nonZeroPrefixedData = new byte[] { 1 }.Concat(data).ToArray();
            BigInteger number = new BigInteger(nonZeroPrefixedData);

            if (number == 0) return "0";

            StringBuilder result = new StringBuilder();
            while (number > 0)
            {
                number = BigInteger.DivRem(number, 36, out BigInteger remainder);
                result.Insert(0, Base36Chars[(int)remainder]);
            }
            return result.ToString();
        }


        public static byte[] Decode(string base36)
        {
            if (string.IsNullOrEmpty(base36))
                throw new ArgumentException("Base36 string is null or empty.");

            BigInteger number = 0;
            foreach (char c in base36)
            {
                number = number * 36 + Base36Chars.IndexOf(c);
            }

            byte[] bytes = number.ToByteArray();
            if (bytes.Length > 1 || bytes[0] != 0)
            {
                // Remove the prefixed byte (1) added during encoding
                if (bytes[bytes.Length - 1] == 1)
                {
                    Array.Resize(ref bytes, bytes.Length - 1);
                }
                Array.Reverse(bytes);  // Convert to little-endian if necessary
                return bytes;
            }
            return new byte[0];  // In case of "0", return an empty array
        }

    }
}
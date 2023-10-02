using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Adefagia.Helper
{
    public static class HashCodeCalculator
    {
        // https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/compute-hash-values
        public static string CalculateHashCode(int[][] grid)
        {
            byte[] tmpSource = (byte[])grid.SelectMany(x => GetByteArrayFromIntArray(x)).ToArray();

            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return ByteArrayToString(tmpHash);
        }

        public static byte[] GetByteArrayFromIntArray(int[] intArray)
        {
            byte[] data = new byte[intArray.Length * 4];

            for (int i = 0; i < intArray.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(intArray[i]), 0, data, i * 4, 4);
            }

            return data;
        }

        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}

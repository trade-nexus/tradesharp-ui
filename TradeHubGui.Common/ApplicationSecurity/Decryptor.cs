using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TradeHubGui.Common.ApplicationSecurity
{
    internal class Decryptor
    {
        private ICryptoTransform DecryptorTransform;
        private UTF8Encoding UTFEncoder;

        private byte[] Key = {};

        private byte[] Vector = {};

        public Decryptor()
        {
            //This is our encryption method
            RijndaelManaged rijndaelManaged = new RijndaelManaged();

            DecryptorTransform = rijndaelManaged.CreateDecryptor(this.Key, this.Vector);

            //Used to translate bytes to text and vice versa
            UTFEncoder = new UTF8Encoding();
        }

        internal Tuple<string, string, string> DecryptLicense(byte[] byteArray)
        {

            var information = DecryptString(Encoding.ASCII.GetString(byteArray));

            var item1 = information.Substring(0, 10);
            var item2 = information.Substring(10, 20);
            var item3 = information.Substring(30, 10);

            return new Tuple<string, string, string>(item3, item2, item1);
        }

        /// The other side: Decryption methods
        internal string DecryptString(string encryptedString)
        {
            return Decrypt(StrToByteArray(encryptedString));
        }

        /// Decryption when working with byte arrays.    
        private string Decrypt(byte[] encryptedValue)
        {
            #region Write the encrypted value to the decryption stream

            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(encryptedValue, 0, encryptedValue.Length);
            decryptStream.FlushFinalBlock();

            #endregion

            #region Read the decrypted value from the stream.

            encryptedStream.Position = 0;
            Byte[] decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();

            #endregion

            return UTFEncoder.GetString(decryptedBytes);
        }

        /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
        //      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        //      return encoding.GetBytes(str);
        // However, this results in character values that cannot be passed in a URL.  So, instead, I just
        // lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
        private byte[] StrToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StrToByteArray");

            byte val;
            byte[] byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            } while (i < str.Length);
            return byteArr;
        }
    }
}

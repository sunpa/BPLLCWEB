using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace BPLLCWEB.Helpers
{
    public static class ArithmeticCaptcha
    {
        private static Random random = new Random();

        private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
        private static string sSharedSecret = "-=bpllc=-";

        public static MvcHtmlString CaptchaCalculationAnswer(this HtmlHelper htmlHelper, int iMaxNumber,
            string textBoxUserInput)
        {
            string[] sPlusSign = new string[3];
            sPlusSign[0] = "+";
            sPlusSign[1] = "add";
            sPlusSign[2] = "plus";

            int iNumberOne = getRandomNumber(iMaxNumber);
            int iNumberTwo = getRandomNumber(iMaxNumber);
            string sCalculation = iNumberOne + " " + sPlusSign[getRandomNumber(sPlusSign.Length)]
                + " " + iNumberTwo + " = ";
            int iTotal = iNumberOne + iNumberTwo;

            return MvcHtmlString.Create(String.Format("<label for=\"{0}\">{1}</label>", textBoxUserInput, sCalculation)
                +
                htmlHelper.Hidden("ArithmeticValueEnc", encryptString(iTotal.ToString()))
                );
        }


        private static int getRandomNumber(int iMax)
        {
            lock (random)
            {
                return random.Next(0, iMax);
            }
        }


        public static bool isValid(string sKey, string sEncryptedKey)
        {
            try
            {
                if (sKey == decryptString(sEncryptedKey))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static string encryptString(String dataString)
        {
            AesManaged aes = new AesManaged();

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sSharedSecret, _salt);

            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            ICryptoTransform encryptor = aes.CreateEncryptor(
            );

            UTF8Encoding utf8Converter = new UTF8Encoding();

            byte[] encodedData = utf8Converter.GetBytes(dataString);

            return Convert.ToBase64String(performCryptoAndEncoding(encodedData, encryptor));
        }


        public static string decryptString(String dataString)
        {
            AesManaged aes = new AesManaged();

            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sSharedSecret, _salt);

            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);

            ICryptoTransform decryptor = aes.CreateDecryptor(
                );

            byte[] encodedData = Convert.FromBase64String(dataString);

            UTF8Encoding utf8Converter = new UTF8Encoding();

            string decryptedString = utf8Converter.GetString(performCryptoAndEncoding(encodedData, decryptor));

            return decryptedString;
        }


        private static byte[] performCryptoAndEncoding(byte[] encodedData, ICryptoTransform transform)
        {
            MemoryStream dataStream = new MemoryStream();
            CryptoStream encryptionStream = new CryptoStream(dataStream, transform, CryptoStreamMode.Write);

            encryptionStream.Write(encodedData, 0, encodedData.Length);
            encryptionStream.FlushFinalBlock();
            dataStream.Position = 0;

            byte[] transformedBytes = new byte[dataStream.Length];
            dataStream.Read(transformedBytes, 0, transformedBytes.Length);
            encryptionStream.Close();
            dataStream.Close();

            return transformedBytes;
        } 
    }
}
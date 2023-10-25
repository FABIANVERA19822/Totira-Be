using System.Net;
using System.Security.Cryptography;
using System.Text;
using Totira.Support.CommonLibrary.Interfaces;

namespace Totira.Support.CommonLibrary.CommonlHandlers;

public class EncryptionHandler : IEncryptionHandler
{
    private string secretKey = "vdtvvYS91HMSugU0TPxF6PeNMKQqOh8eXQXPrUrJUuTaRVQa43IgNzv5axfmVO0/n0Cnlc3zmqxsyv7cKE/DaLdaOOqi7WN+5tmOCYUXYPr+1pdKzwb+6c5k3DwuJE8/C4hiLs+ypBgr68Y0KlTXJyRcY77eRMgwM099lEyeGt6On6TqbA2xitE92PwhxA0SrR4EyOWF5206O6JLDbiBCBZVeywPCa3DBfALxoKhkgYZK1fdhC4Rv9poTvwULlwMo88MOlL1L2WQAuzXrBS+hiFww/ALvsUgUCrsx9ML2io4o0TvYG2q4sKd2DRoC9KFxaqRgTNM2cd3gv7t9XExAQ==";

    public string EncryptString(string text)
    {
        return WebUtility.UrlEncode(Encrypt(text));
    }
    public string DecryptString(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        var data = WebUtility.UrlDecode(text);
        return Decrypt(data);
    }


    #region Helper Method


    string Encrypt(string text)
    {
        try
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            byte[] plainText = System.Text.Encoding.Unicode.GetBytes(text);

            byte[] salt = Encoding.ASCII.GetBytes(secretKey.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(secretKey, salt);

            ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainText, 0, plainText.Length);
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string encryptedData = Convert.ToBase64String(CipherBytes);
            return ToBase64(encryptedData);
        }
        catch
        {
            throw;
        }
    }
    string Decrypt(string text)
    {
        try
        {
            text = FromBase64(text);
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] encryptedData = Convert.FromBase64String(text);
            byte[] salt = Encoding.ASCII.GetBytes(secretKey.Length.ToString());
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(secretKey, salt);
            ICryptoTransform Decryptor = rijndaelCipher.CreateDecryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(encryptedData);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
            byte[] plainText = new byte[encryptedData.Length];
            int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
            return decryptedData;
        }
        catch
        {
            throw;
        }
    }

    string ToBase64(string input)
    {
        StringBuilder result = new StringBuilder(input.TrimEnd('='));
        result.Replace('+', '-');
        result.Replace('/', '_');
        return result.ToString();
    }
    string FromBase64(string input)
    {
        int padChars = (input.Length % 4) == 0 ? 0 : (4 - (input.Length % 4));
        StringBuilder result = new StringBuilder(input, input.Length + padChars);
        result.Append(String.Empty.PadRight(padChars, '='));
        result.Replace('-', '+');
        result.Replace('_', '/');
        return result.ToString();
    }

    #endregion
}


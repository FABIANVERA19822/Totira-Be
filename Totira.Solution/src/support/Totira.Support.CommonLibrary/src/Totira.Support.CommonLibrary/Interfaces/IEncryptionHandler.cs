using System;
namespace Totira.Support.CommonLibrary.Interfaces
{
    public interface IEncryptionHandler
	{
        string EncryptString(string plaintext);
        string DecryptString(string encrypted);
    }
}


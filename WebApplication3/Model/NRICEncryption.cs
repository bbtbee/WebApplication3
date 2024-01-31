using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication3.Model
{
	public class NRICEncryption
	{
		private static readonly string EncryptionKey = "YourEncryptionKey"; // Replace with a secure key
		private static readonly string IV = "YourInitializationVector"; // Replace with a secure IV

		public static string EncryptNRIC(string plainNRIC)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
				aesAlg.IV = Encoding.UTF8.GetBytes(IV);

				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(plainNRIC);
						}
					}

					return Convert.ToBase64String(msEncrypt.ToArray());
				}
			}
		}

		public static string DecryptNRIC(string encryptedNRIC)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
				aesAlg.IV = Encoding.UTF8.GetBytes(IV);

				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

				using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedNRIC)))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
						{
							return srDecrypt.ReadToEnd();
						}
					}
				}
			}
		}
	}
}

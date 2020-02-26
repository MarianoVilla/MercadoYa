using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MercadoYa.Lib.Util
{
    public class CryptoUtil
    {
		public static List<string> EncryptStrings(string Key, params string[] Strings)
		{
			var Output = new List<string>();
			foreach(var s in Strings)
			{
				Output.Add(EncryptString(Key, s));
			}
			return Output;
		}
		public static List<string> TryEncryptStrings(string Key, string DefaultTo = null, params string[] Strings)
		{
			var Output = new List<string>();
			foreach(var s in Strings)
			{
				Output.Add(TryEncryptString(Key, s, DefaultTo));
			}
			return Output;
		}
		public static string TryEncryptString(string Key, string PlainInput, string DefaultTo = null)
		{
			return string.IsNullOrWhiteSpace(PlainInput) ? DefaultTo : EncryptString(Key, PlainInput);
		}
		public static string EncryptString(string Key, string PlainInput)
		{
			if (string.IsNullOrWhiteSpace(PlainInput))
				throw new ArgumentNullException("Cannot encrypt an empty string");
			byte[] Iv = new byte[16];
			byte[] Array;
			using (var aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(Key);
				aes.IV = Iv;
				ICryptoTransform Encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
				using (var MemoryStream = new MemoryStream())
				{
					using (var CryptoStream = new CryptoStream(MemoryStream, Encryptor, CryptoStreamMode.Write))
					{
						using (var Writer = new StreamWriter(CryptoStream))
						{
							Writer.Write(PlainInput);
						}

						Array = MemoryStream.ToArray();
					}
				}
			}
			return Convert.ToBase64String(Array);
		}

		public static List<string> DecryptStrings(string Key, params string[] Strings)
		{
			var Output = new List<string>();
			foreach(var s in Strings)
			{
				Output.Add(DecryptString(Key, s));
			}
			return Output;
		}
		public static string TryDecryptString(string Key, string CipherText, string DefaultTo = null)
		{
			return string.IsNullOrWhiteSpace(CipherText) ? DefaultTo : DecryptString(Key, CipherText);
		}
		public static List<string> TryDecryptStrings(string Key, string DefaultTo = null, params string[] Strings)
		{
			var Output = new List<string>();
			foreach(var s in Strings)
			{
				Output.Add(TryDecryptString(Key, s, DefaultTo));
			}
			return Output;
		}
		public static string DecryptString(string Key, string CipherText)
		{
			if (string.IsNullOrWhiteSpace(CipherText))
				throw new ArgumentNullException("Cannot Decrypt an empty string");
			byte[] Iv = new byte[16];
			byte[] Buffer = Convert.FromBase64String(CipherText);
			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(Key);
				aes.IV = Iv;
				ICryptoTransform Decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
				using (var MemStream = new MemoryStream(Buffer))
				{
					using (var CrypStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
					{
						using (var StrReader = new StreamReader(CrypStream))
						{
							return StrReader.ReadToEnd();
						}
					}
				}
			}
		}



		public static string RandomString(int Length)
		{
			const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			var Res = new StringBuilder(Length);
			using (var rng = new RNGCryptoServiceProvider())
			{
				int Ount = (int)Math.Ceiling(Math.Log(alphabet.Length, 2) / 8.0);
				Debug.Assert(Ount <= sizeof(uint));
				int Offset = BitConverter.IsLittleEndian ? 0 : sizeof(uint) - Ount;
				int Max = (int)(Math.Pow(2, Ount * 8) / alphabet.Length) * alphabet.Length;
				byte[] uintBuffer = new byte[sizeof(uint)];

				while (Res.Length < Length)
				{
					rng.GetBytes(uintBuffer, Offset, Ount);
					uint Num = BitConverter.ToUInt32(uintBuffer, 0);
					if (Num < Max)
					{
						Res.Append(alphabet[(int)(Num % alphabet.Length)]);
					}
				}
			}

			return Res.ToString();
		}
	}
}

using MercadoYa.Lib.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MercadoYa.Test
{
    public class CryptoUtilTest
    {
        string Key = "b14ca5898a4e4133bbce2ea2315a1916";

        [Test]
        public void TestEncryptDecryptString()
        {
            string EncriptedString = CryptoUtil.EncryptString(Key, "Test");
            string DecryptedString = CryptoUtil.DecryptString(Key, EncriptedString);

            Assert.AreEqual("Test", DecryptedString);
        }
        [Test]
        public void TestEncryptDecryptStringNullOrEmpty_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => CryptoUtil.EncryptString(Key, null));
            Assert.Throws<ArgumentNullException>(() => CryptoUtil.DecryptString(Key, null));
            Assert.Throws<ArgumentNullException>(() => CryptoUtil.EncryptString(Key, ""));
            Assert.Throws<ArgumentNullException>(() => CryptoUtil.DecryptString(Key, ""));
            Assert.Throws<ArgumentNullException>(() => CryptoUtil.EncryptString(Key, "\n\r \r  "));
            Assert.Throws<ArgumentNullException>(() => CryptoUtil.DecryptString(Key, "\n\r \r  "));
        }
        [Test]
        public void TestEncryptDecryptMultipleStrings()
        {
            List<string> EncriptedString = CryptoUtil.EncryptStrings(Key: Key, "Test", "Test2");
            List<string> DecryptedString = CryptoUtil.DecryptStrings(Key: Key, EncriptedString.ToArray());

            Assert.AreEqual("Test", DecryptedString[0]);
            Assert.AreEqual("Test2", DecryptedString[1]);
        }
        [Test]
        public void TestTryEncryptDecryptString()
        {
            string RandomKey = CryptoUtil.RandomString(32);

            string EncriptedString = CryptoUtil.TryEncryptString(RandomKey, "Test");
            string DecryptedString = CryptoUtil.TryDecryptString(RandomKey, EncriptedString);

            Assert.AreEqual("Test", DecryptedString);
        }
        [Test]
        public void TestTryEncryptDecryptStringNullOrEmpty_ShouldWork()
        {

            string EncriptedEmptyString = CryptoUtil.TryEncryptString(Key, "");
            string EncriptedNullString = CryptoUtil.TryEncryptString(Key, null);

            Assert.IsNull(EncriptedEmptyString);
            Assert.IsNull(EncriptedNullString);

        }
        [Test]
        public void TestEncryptDecriptStringWithRandomKey()
        {
            string RandomKey = CryptoUtil.RandomString(32);

            string EncriptedString = CryptoUtil.EncryptString(RandomKey, "Test");
            string DecryptedString = CryptoUtil.DecryptString(RandomKey, EncriptedString);

            Assert.AreEqual("Test", DecryptedString);
        }
        [Test]
        public void TestRandomString()
        {
            string GeneratedString = CryptoUtil.RandomString(32);

            Assert.AreEqual(32, GeneratedString.Length);
        }

    }
}
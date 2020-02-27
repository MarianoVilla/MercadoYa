using MercadoYa.Lib.Util;
using MercadoYa.Rest.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Test
{
    [TestFixture]
    public class HashUtilTest
    {
        [Test]
        public void TestHashAndCheck_ShouldAuth()
        {
            var Hasher = new HashUtil();
            string TestPassword = "Abc123456";
            string Hashed = Hasher.HashPassword(TestPassword);

            Assert.True(Hasher.CheckPassword(Hashed, TestPassword));
        }
        [Test]
        public void TestHashAndCheck_ShouldReject()
        {
            var Hasher = new HashUtil();
            string TestPassword = "Abc123456";
            string Hashed = Hasher.HashPassword(TestPassword);

            Assert.False(Hasher.CheckPassword(Hashed, "SomeInvalidPass"));
        }
    }
}

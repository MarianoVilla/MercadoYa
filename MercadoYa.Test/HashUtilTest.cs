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
        public void TestHashAndCheck()
        {
            var Hasher = new HashUtil();
            string TestPassword = "Abc123456";
            string SaltedPassword = TestPassword + Hasher.GenerateSalt();
            string Hashed = Hasher.HashPassword(SaltedPassword);

            Assert.True(Hasher.CheckPassword(TestPassword, Hashed));

        }
    }
}

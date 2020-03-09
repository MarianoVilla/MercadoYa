using MercadoYa.Lib.Comparers;
using MercadoYa.Model.Concrete;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Test
{
    [TestFixture]
    public class AppUserTest
    {
        [Test]
        public void TestUserEqualityComparer()
        {
            var FirstUser = new AppUser() { Uid = "SomeUid" };
            var SecondUser = new AppUser() { Uid = "AnotherUid" };
            var Comparer = new UserEqualityComparer();

            bool TheyAreEqual = Comparer.Equals(FirstUser, SecondUser);

            Assert.IsFalse(TheyAreEqual);
        }
    }
}

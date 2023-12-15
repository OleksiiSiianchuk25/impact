using EfCore.service.impl;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.service.impl
{
    [TestFixture]
    public class VerificationCodeManagerTest
    {
        [Test]
        public void TestGenerateVerificationCode_ReturnsValidCode()
        {
            // Arrange
            var verificationCodeManager = new VerificationCodeManager(Mock.Of<IConfiguration>());

            // Act
            var code = VerificationCodeManager.GenerateVerificationCode();

            // Assert
            Assert.IsNotNull(code);
            Assert.IsTrue(int.TryParse(code, out _));
        }

        [Test]
        public void TestStoreVerificationCodeAndVerifyCode_CodeIsValid_ReturnsTrue()
        {
            // Arrange
            var verificationCodeManager = new VerificationCodeManager(Mock.Of<IConfiguration>());
            var email = "test@example.com";
            var code = "12345";

            // Act
            VerificationCodeManager.StoreVerificationCode(email, code);
            var result = VerificationCodeManager.VerifyCode(email, code);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TestVerifyCode_CodeIsInvalid_ReturnsFalse()
        {
            // Arrange
            var verificationCodeManager = new VerificationCodeManager(Mock.Of<IConfiguration>());
            var email = "test@example.com";
            var storedCode = "12345";
            var enteredCode = "54321";

            // Act
            VerificationCodeManager.StoreVerificationCode(email, storedCode);
            var result = VerificationCodeManager.VerifyCode(email, enteredCode);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

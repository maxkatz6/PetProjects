namespace BlockchainNet.Test.Serfices
{
    using System;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Services;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SignatureServiceTests
    {
        [TestMethod]
        public void Should_Return_Not_Empty_Keys()
        {
            var signatureService = new SignatureService();
            var (publicKey, privateKey) = signatureService.GetKeysFromPassword("password");

            Assert.AreNotEqual(0, publicKey.Length, "Public key is empty");
            Assert.AreNotEqual(0, privateKey.Length, "Public key is empty");
        }

        [DataRow(
            "password",
            "BMJ0gBKO6Y2sx2TB3+OM1APvKmY4LIIRN0lHz3ycYccPm1l9g2UCnojmU5f1h1c+xyULosxlgwUYgrZznuFPtrk=",
            "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=")]
        [DataRow(
            "hello world",
            "BALb/cRAiDvAw8uK0Ojm+9xUF0o9xU4rigTZR6L/SMN0V8jf7H/BcNwMpx8EYrhhYDg5ueSK1lZKxNDbV/phEB8=",
            "uU0nuZNNPgilLlLX2n2r+sSE7+N6U4DukIj3rOLvzek=")]
        [DataTestMethod]
        public void Should_Create_Correct_Keys(string password, string publicKeyStr, string privateKeyStr)
        {
            var signatureService = new SignatureService();
            var (publicKey, privateKey) = signatureService.GetKeysFromPassword(password);
            var publicKeyStrActual = Convert.ToBase64String(publicKey);
            var privateKeyStrActual = Convert.ToBase64String(privateKey);

            Assert.AreEqual(publicKeyStr, publicKeyStrActual, "Public key doesn't match");
            Assert.AreEqual(privateKeyStr, privateKeyStrActual, "Public key doesn't match");
        }
        
        [DataRow(
            "hello world",
            "BMJ0gBKO6Y2sx2TB3+OM1APvKmY4LIIRN0lHz3ycYccPm1l9g2UCnojmU5f1h1c+xyULosxlgwUYgrZznuFPtrk=",
            "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=")]
        [DataTestMethod]
        public void Should_Create_Correct_Signature(string content, string publicKeyStr, string privateKeyStr)
        {
            var publicKey = Convert.FromBase64String(publicKeyStr);
            var privateKey = Convert.FromBase64String(privateKeyStr);
            var transaction = new Transaction<string>("sender", "recipient", publicKey, content, DateTime.Now);
            

            var signatureService = new SignatureService();
            signatureService.SignTransaction(transaction, privateKey);

            var isTrusted = signatureService.VerifyTransaction(transaction);
            
            Assert.IsTrue(isTrusted, "Signature is not trusted");
        }
    }
}
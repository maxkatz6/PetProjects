namespace BlockchainNet.Test.Protobuf
{
    using System;
    using System.IO;

    using BlockchainNet.Core.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void Transaction_SerializeDeserializeTest()
        {
            var transaction = new Transaction<decimal>("UserA", "UserB", new byte[] { 42 }, 999, DateTime.Now);

            using var stream = new MemoryStream();

            using var writer = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(writer);

            var serializer = JsonSerializer.CreateDefault();
            serializer.Serialize(jsonWriter, transaction);
            jsonWriter.Flush();
            _ = stream.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            var newTransaction = serializer.Deserialize<Transaction<decimal>>(jsonReader);

            Assert.IsTrue(transaction.Sender == newTransaction.Sender, "Transactions sender is not equal");
            Assert.IsTrue(transaction.Recipient == newTransaction.Recipient, "Transactions recipient is not equal");
            CollectionAssert.AreEquivalent(transaction.PublicKey, newTransaction.PublicKey, "Transactions public keys is not equal");
            Assert.IsTrue(transaction.Content == newTransaction.Content, "Transactions amount is not equal");
            Assert.IsTrue(transaction.Date == newTransaction.Date, "Transactions date is not equal");
        }
    }
}

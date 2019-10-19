namespace BlockchainNet.Test.Json
{
    using System;
    using System.IO;

    using BlockchainNet.Core.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    [TestClass]
    public class BlockTest
    {
        [TestMethod]
        public void Block_SerializeDeserializeTest()
        {
            var transactions = new[] { new Transaction<decimal>("UserA", "UserB", new byte[] { 42 }, 999, DateTime.Now) };
            var block = new Block<decimal>(transactions, "h34j34h634g5i4h536oi==", 0);
            block.ConfirmBlock("hash", 123);

            using var stream = new MemoryStream();

            using var writer = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(writer);

            var serializer = JsonSerializer.CreateDefault();
            serializer.Serialize(jsonWriter, block);
            jsonWriter.Flush();
            _ = stream.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            var newBlock = serializer.Deserialize<Block<decimal>>(jsonReader);

            Assert.IsTrue(newBlock.Id == block.Id, "Blocks indexes is not equal");
            Assert.IsTrue(newBlock.Date == block.Date, "Blocks date is not equal");
            Assert.IsTrue(newBlock.Proof == block.Proof, "Blocks proof is not equal");
            Assert.IsTrue(newBlock.PreviousBlockId == block.PreviousBlockId, "Blocks previous hash is not equal");

            for (var index = 0; index < transactions.Length; index++)
            {
                var transaction = block.Transactions[index];
                var newTransaction = newBlock.Transactions[index];

                Assert.IsTrue(transaction.Sender == newTransaction.Sender, $"#{index + 1} transactions sender is not equal");
                Assert.IsTrue(transaction.Recipient == newTransaction.Recipient, $"#{index + 1} transactions recipient is not equal");
                Assert.IsTrue(transaction.Content == newTransaction.Content, $"#{index + 1} transactions amount is not equal");
            }
        }
    }
}

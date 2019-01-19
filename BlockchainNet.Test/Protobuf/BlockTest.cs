namespace BlockchainNet.Test.Protobuf
{
    using System;
    using System.IO;

    using ProtoBuf;

    using BlockchainNet.Core.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BlockTest
    {
        [TestMethod]
        public void Block_SerializeDeserializeTest()
        {
            var transactions = new[] { new Transaction<decimal>("UserA", "UserB", 999, DateTime.Now) };
            var block = new Block<decimal>(0, DateTime.Now, transactions, 123, "h34j34h634g5i4h536oi==");

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, block);

                stream.Flush();
                stream.Position = 0;

                var newBlock = Serializer.Deserialize<Block<decimal>>(stream);

                Assert.IsTrue(newBlock.Index == block.Index, "Blocks indexes is not equal");
                Assert.IsTrue(newBlock.Date == block.Date, "Blocks date is not equal");
                Assert.IsTrue(newBlock.Proof == block.Proof, "Blocks proof is not equal");
                Assert.IsTrue(newBlock.PreviousHash == block.PreviousHash, "Blocks previous hash is not equal");

                for (int index = 0; index < transactions.Length; index++)
                {
                    var transaction = block.Content[index];
                    var newTransaction = newBlock.Content[index];

                    Assert.IsTrue(transaction.Sender == newTransaction.Sender, $"#{index + 1} transactions sender is not equal");
                    Assert.IsTrue(transaction.Recipient == newTransaction.Recipient, $"#{index + 1} transactions recipient is not equal");
                    Assert.IsTrue(transaction.Content == newTransaction.Content, $"#{index + 1} transactions amount is not equal");
                }
            }
        }
    }
}

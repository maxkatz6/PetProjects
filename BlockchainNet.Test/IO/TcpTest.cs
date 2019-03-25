namespace BlockchainNet.Test.IO
{
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.IO.TCP;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TcpTest
    {
        [TestMethod, Timeout(5000)]
        public async Task Tcp_Instantiate_TcpServer_IdTest()
        {
            var server = new TcpServer<string>();
            await server.StartAsync();

            Assert.IsFalse(string.IsNullOrWhiteSpace(server.ServerId), "Server id is null or empty");
        }

        [TestMethod, Timeout(5000)]
        public async Task Tcp_Client_ConnectTest()
        {
            var tcs = new TaskCompletionSource<bool>();

            var isConnected = false;

            var server = new TcpServer<string>();
            await server.StartAsync();

            var client = new TcpClient<string>(server.ServerId);
            server.ClientConnectedEvent += (sender, args) =>
            {
                isConnected = true;
                tcs.SetResult(true);
            };
            await client.StartAsync();

            await tcs.Task;

            Assert.IsTrue(isConnected, "Client is not connected");
        }

        [TestMethod, Timeout(5000)]
        public async Task Tcp_Client_Connect_ResponceServerIdTest()
        {
            const string ResponceServerId = "responceServerId";

            var tcs = new TaskCompletionSource<bool>();
            var id = string.Empty;

            var server = new TcpServer<string>();
            await server.StartAsync();

            var client = new TcpClient<string>(server.ServerId)
            {
                ResponseServerId = ResponceServerId
            };
            server.ClientConnectedEvent += (sender, args) =>
            {
                id = args.ClientId;
                tcs.SetResult(true);
            };
            await client.StartAsync();

            await tcs.Task;

            Assert.AreEqual(ResponceServerId, id, "Responce server id does not match the correct");
        }

        [TestMethod, Timeout(5000)]
        public async Task Tcp_Client_Connect_ResponceServerId_EmptyTest()
        {
            const string? ResponceServerId = null;

            var tcs = new TaskCompletionSource<bool>();
            var id = string.Empty;

            var server = new TcpServer<string>();
            await server.StartAsync();

            var client = new TcpClient<string>(server.ServerId);
            server.ClientConnectedEvent += (sender, args) =>
            {
                id = args.ClientId;
                tcs.SetResult(true);
            };
            await client.StartAsync();

            await tcs.Task;

            Assert.AreEqual(ResponceServerId, id, "Responce server id does not match the correct");
        }

        [TestMethod, Timeout(5000)]
        public async Task Tcp_Client_DisconnectTest()
        {
            var tcs = new TaskCompletionSource<bool>();

            var isDisconnected = false;

            var server = new TcpServer<string>();
            await server.StartAsync();

            var client = new TcpClient<string>(server.ServerId);
            server.ClientDisconnectedEvent += (sender, args) =>
            {
                isDisconnected = true;
                tcs.SetResult(true);
            };
            await client.StartAsync();

            Assert.IsFalse(isDisconnected, "Client is disconected");

            await client.StopAsync();

            await tcs.Task;

            Assert.IsTrue(isDisconnected, "Client is still connected");
        }

        [TestMethod, Timeout(5000)]
        public async Task Tcp_Client_SendMessageTest()
        {
            const string Message = "Client's message";

            var tcs = new TaskCompletionSource<string>();

            var server = new TcpServer<string>();
            await server.StartAsync();

            var client = new TcpClient<string>(server.ServerId);
            server.MessageReceivedEvent += (sender, args) =>
            {
                tcs.SetResult(args.Message);
            };
            await client.StartAsync();

            var success = await client.SendMessageAsync(Message);
            Assert.IsTrue(success, "Send message failed");

            var message = await tcs.Task;

            Assert.AreEqual(Message, message, "Message are not equal");
        }

        [TestMethod, Timeout(5000)]
        public async Task Tcp_Client_SendMessage_LongMessageTest()
        {
            const int BufferSize = 2048;

            var tcs = new TaskCompletionSource<string>();

            var server = new TcpServer<string>();
            await server.StartAsync();

            var client = new TcpClient<string>(server.ServerId);
            server.MessageReceivedEvent += (sender, args) => tcs.SetResult(args.Message);
            await client.StartAsync();

            var longString = $"[{new string('*', 10 * 2048)}]";

            Assert.IsTrue(Encoding.UTF8.GetByteCount(longString) > BufferSize, "Test string is smaller than needed buffer length");

            var success = await client.SendMessageAsync(longString);
            Assert.IsTrue(success, "Send message failed");

            var message = await tcs.Task;

            Assert.AreEqual(longString, message, "Messages are not equal");
        }

        [TestMethod, Timeout(5000)]
        public async Task Tcp_Client_SendMessage_MultiMessageTest()
        {
            const string FirstTestMessage = "Hi from first";
            const string SecondTestMessage = "Hi from second";

            var autoEvent = new AutoResetEvent(false);
            var message = string.Empty;

            var server = new TcpServer<string>();
            await server.StartAsync();

            var client = new TcpClient<string>(server.ServerId);
            server.MessageReceivedEvent += (sender, args) =>
            {
                message = args.Message;
                autoEvent.Set();
            };
            await client.StartAsync();

            var success = await client.SendMessageAsync(FirstTestMessage);
            Assert.IsTrue(success, "Send #1 message failed");

            autoEvent.WaitOne();
            Assert.AreEqual(FirstTestMessage, message, "#1 messages are not equal");

            success = await client.SendMessageAsync(SecondTestMessage);
            Assert.IsTrue(success, "Send #2 message failed");

            autoEvent.WaitOne();
            Assert.AreEqual(SecondTestMessage, message, "#2 messages are not equal");
        }

        [TestMethod, Timeout(1000000)]
        public async Task Tcp_Client_SendMessage_RestartedTest()
        {
            const string FirstMessage = "First";
            const string SecondMessage = "Second";

            var message = string.Empty;
            var autoEvent = new AutoResetEvent(false);

            var server = new TcpServer<string>();
            server.MessageReceivedEvent += (sender, args) =>
            {
                message = args.Message;

                autoEvent.Set();
            };
            await server.StartAsync();

            var client = new TcpClient<string>(server.ServerId);
            await client.StartAsync();

            await client.SendMessageAsync(FirstMessage);
            autoEvent.WaitOne();
            Assert.AreEqual(FirstMessage, message, "Message are not equal");

            await client.StopAsync();
            await client.StartAsync();

            await client.SendMessageAsync(SecondMessage);
            autoEvent.WaitOne();
            Assert.AreEqual(SecondMessage, message, "Message are not equal");
        }
    }
}

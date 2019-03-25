namespace BlockchainNet.Core
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.IO;
    using BlockchainNet.Core.Models;

    public class Communicator<TBlockchain, TContent>
        where TBlockchain : Blockchain<TContent>
    {
        private readonly ICommunicationServer<List<Block<TContent>>> server;
        private readonly ICommunicationClientFactory<List<Block<TContent>>> clientFactory;

        private readonly List<ICommunicationClient<List<Block<TContent>>>> nodes;

        /// <summary>
        /// Конструктор коммуникатора
        /// </summary>
        /// <param name="server">Сервер</param>
        /// <param name="clientFactory">Фаблика клиентов</param>
        public Communicator(
            ICommunicationServer<List<Block<TContent>>> server,
            ICommunicationClientFactory<List<Block<TContent>>> clientFactory)
        {
            this.server = server;
            this.clientFactory = clientFactory;

            nodes = new List<ICommunicationClient<List<Block<TContent>>>>();

            server.MessageReceivedEvent += Server_MessageReceivedEvent;
            server.ClientConnectedEvent += Server_ClientConnectedEvent;
            server.ClientDisconnectedEvent += Server_ClientDisconnectedEvent;
        }

        public string ServerId => server.ServerId;

        public TBlockchain Blockchain { get; set; }

        public Task StartAsync()
        {
            return server.StartAsync();
        }

        public Task SyncAsync(bool onlyGet = false)
        {
            if (Blockchain == null)
            {
                throw new InvalidOperationException("Blockchain must be setted");
            }

            // Пустой массив не будет опознан как какое-либо сообщение, 
            // которое можно прочитать. Но безопасно отправить общий начальный блок,
            // который опознается как неполный блокчейн, и в ответ получим остальные блоки
            var sendedList = onlyGet
                ? new List<Block<TContent>> { Blockchain.Chain[0] }
                : Blockchain.Chain.ToList();
            return Task
                .WhenAll(nodes
                .Select(node => node.SendMessageAsync(sendedList)));
        }

        public void ConnectTo(IEnumerable<string> serversId)
        {
            foreach (var serverId in serversId)
            {
                var nodeClient = clientFactory.CreateNew(serverId);
                nodeClient.ResponseServerId = server.ServerId;
                nodeClient.StartAsync();
                nodes.Add(nodeClient);
            }
        }

        public void Close()
        {
            server.StopAsync();

            foreach (var nodeClient in nodes)
            {
                nodeClient.StopAsync();
            }
        }

        private void Server_ClientConnectedEvent(object sender, ClientConnectedEventArgs e)
        {
            if (nodes.All(c => c.ServerId != e.ClientId))
            {
                var nodeClient = clientFactory.CreateNew(e.ClientId);
                nodeClient.ResponseServerId = server.ServerId;
                nodeClient.StartAsync();
                nodes.Add(nodeClient);
            }
        }

        private void Server_ClientDisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            var node = nodes.Find(n => n.ServerId == e.ClientId);
            if (node != null)
            {
                node.StopAsync();
                nodes.Remove(node);
            }
        }

        private async void Server_MessageReceivedEvent(object sender, MessageReceivedEventArgs<List<Block<TContent>>> e)
        {
            if (Blockchain == null)
            {
                throw new InvalidOperationException("Blockchain must be setted");
            }

            var replaced = Blockchain.TrySetChainIfValid(e.Message);
            // Если не заменено, то входящяя цепочка или невалидна, или меньше существующей
            // Есть смысл отправить отправителю свою цепочку для замены
            if (!replaced)
            {
                var nodeClient = nodes.Find(c => c.ServerId == e.ClientId);
                await nodeClient.SendMessageAsync(Blockchain.Chain.ToList());
            }
        }
    }
}

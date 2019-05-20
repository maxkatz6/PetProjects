namespace BlockchainNet.Console
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BlockchainNet.IO.TCP;
    using BlockchainNet.Core;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Messenger;
    using BlockchainNet.Core.Consensus;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Services;
    using BlockchainNet.Core.EventArgs;

    internal static class Program
    {
        private static Communicator<string> communicator;
        private static ISignatureService signatureService;
        private static IBlockRepository<string> blockRepository;
        private static MessengerBlockchain blockchain;

        private static string account;
        private static (byte[] publicKey, byte[] privateKey) keys;

        private static async Task Main()
        {
            var currentPort = TcpHelper.GetAvailablePort();
            System.IO.File.Delete($"database_{currentPort}.litedb");
            blockRepository = new DefaultBlockRepository<string>($"database_{currentPort}.litedb");

            signatureService = new SignatureService();
            communicator = new Communicator<string>(
                blockRepository,
                new TcpServer<BlockchainPayload<string>>(currentPort),
                new TcpClientFactory<BlockchainPayload<string>>());

            blockchain = new MessengerBlockchain(
                communicator,
                blockRepository,
                new ProofOfWorkConsensus<string>(),
                signatureService);
            blockchain.BlockAdded += Blockchain_BlockAdded;

            await communicator.StartAsync().ConfigureAwait(false);

            Console.Title = communicator.ServerId;

            AskAndSetAccount();

            var active = true;
            while (active)
            {
                var parts = Console.ReadLine().Split(' ');
                if (parts.Length == 0)
                {
                    continue;
                }

                switch (parts[0])
                {
                    case "connect":
                    case "c":
                        await communicator.ConnectToAsync(parts.Skip(1)).ConfigureAwait(false);
                        break;
                    case "exit":
                    case "e":
                        active = false;
                        break;
                    case "blocks":
                    case "b":
                        await PrintBlockchainAsync().ConfigureAwait(false);
                        break;
                    case "transactions":
                    case "t":
                        PrintCurrentTransactions();
                        break;
                    case "mine":
                    case "m":
                        _ = await blockchain.MineAsync(account, default).ConfigureAwait(false);
                        break;
                    case "add":
                    case "a":
                        await SendMessageAsync(parts.Skip(1).FirstOrDefault(), string.Join(" ", parts.Skip(2).ToArray()));
                        break;
                    case "help":
                    case "h":
                        PrintHelp();
                        break;
                    case "switch":
                    case "sw":
                        AskAndSetAccount(parts.Skip(1).FirstOrDefault(), parts.Skip(2).FirstOrDefault());
                        break;
                }
            }

            await communicator.CloseAsync().ConfigureAwait(false);
        }

        private static void Blockchain_BlockchainReplaced(object sender, EventArgs e)
        {
            Console.WriteLine("Blockchain replaced");
        }

        private static void Blockchain_BlockAdded(object sender, BlockAddedEventArgs<string> e)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var transaction in e.AddedBlocks.SelectMany(b => b.Transactions))
            {
                if (transaction.Recipient == account)
                {
                    Console.WriteLine($"({transaction.Date}) {transaction.Sender}: {transaction.Content}");
                    Console.Beep();
                }
                else if (transaction.Sender == account)
                {
                    Console.WriteLine($"({transaction.Date}) You: {transaction.Content}");
                }
            }
            Console.ResetColor();
        }

        private static void AskAndSetAccount(string? username = null, string? password = null)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.Write("Input account: ");
                username = Console.ReadLine();
            }
            account = username;

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.Write("Input password: ");
                password = Console.ReadLine();
            }
            keys = signatureService.GetKeysFromPassword(password);
            Console.Clear();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("e, exit - close the node");
            Console.WriteLine("a, add [recipient] [amount] - add new transaction");
            Console.WriteLine("t, transactions - print current transactions");
            Console.WriteLine("b, blocks - print blockschain");
            Console.WriteLine("m, mine - mine new block");
            Console.WriteLine("s, sync - sync blockchain with neighbor");
            Console.WriteLine("sv, save [filename] - save blockchain");
            Console.WriteLine("ld, load [filename] - load blockchain");
            Console.WriteLine("sw, switch [account] - change account");
        }

        private static async Task SendMessageAsync(string? recipient, string? message)
        {
            while (string.IsNullOrEmpty(recipient))
            {
                Console.Write("Recipient: ");
                recipient = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(message))
            {
                Console.Write("Message: ");
                message = Console.ReadLine();
            }

            blockchain.NewTransaction(account, recipient, message, keys);

            _ = await blockchain.MineAsync(account, default).ConfigureAwait(false);
        }

        private static async Task PrintBlockchainAsync()
        {
            var blocks = await blockRepository.GetFork(MessengerBlockchain.RootId).ToListAsync();
            Console.WriteLine($"Blocks count = {blocks.Count}");
            foreach (var block in blocks)
            {
                Console.WriteLine(block.ToString());
                if (block.Transactions.Count > 0)
                {
                    Console.WriteLine(string.Join("\r\n", block.Transactions));
                }
            }
            Console.WriteLine();
        }

        private static void PrintCurrentTransactions()
        {
            Console.WriteLine($"Current transactions {blockchain.CurrentTransactions.Count}:");
            Console.WriteLine(string.Join("\r\n", blockchain.CurrentTransactions));
        }
    }
}

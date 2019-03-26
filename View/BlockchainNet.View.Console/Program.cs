namespace BlockchainNet.Console
{
    using BlockchainNet.IO.TCP;
    using BlockchainNet.Core;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Messenger;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    internal static class Program
    {
        private static Communicator<MessengerBlockchain, string> communicator;
        private static MessengerBlockchain blockchain => communicator.Blockchain!;

        private static string account;

        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        private static async Task Main(string[] args)
        {
            SetConsoleCtrlHandler(arg =>
                {
                    if (arg == 2)
                    {
                        communicator.Close();
                    }

                    return false;
                }, true);

            var currentPort = TcpHelper.GetAvailablePort();

            var blockchain = LoadBlockChain();
            blockchain.BlockAdded += Blockchain_BlockAdded;
            blockchain.BlockchainReplaced += Blockchain_BlockchainReplaced;

            communicator = new Communicator<MessengerBlockchain, string>(
                blockchain,
                new TcpServer<List<Block<string>>>(currentPort),
                new TcpClientFactory<List<Block<string>>>());

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
                        communicator.ConnectTo(parts.Skip(1));
                        break;
                    case "exit":
                    case "e":
                        active = false;
                        break;
                    case "blocks":
                    case "b":
                        PrintBlockchain();
                        break;
                    case "transactions":
                    case "t":
                        PrintCurrentTransactions();
                        break;
                    case "mine":
                    case "m":
                        _ = MineAndPrintNewBlock();
                        break;
                    case "add":
                    case "a":
                        ReadAndAddNewTransaction(parts.Skip(1).ToArray());
                        break;
                    case "sync":
                    case "s":
                        SyncBlockchain();
                        break;
                    case "help":
                    case "h":
                        PrintHelp();
                        break;
                    case "save":
                    case "sv":
                        SaveBlockChain(parts.Skip(1).FirstOrDefault());
                        break;
                    case "switch":
                    case "sw":
                        AskAndSetAccount(parts.Skip(1).FirstOrDefault());
                        break;
                }
            }

            communicator.Close();
        }

        private static void Blockchain_BlockchainReplaced(object sender, EventArgs e)
        {
            Console.WriteLine("Blockchain replaced");
        }

        private static void Blockchain_BlockAdded(object sender, BlockAddedEventArgs<string> e)
        {
            Console.WriteLine("Message: " + e.AddedBlock);
        }

        private static void AskAndSetAccount(string? newAccount = null)
        {
            if (string.IsNullOrWhiteSpace(newAccount))
            {
                Console.Write("Input account: ");
                newAccount = Console.ReadLine();
            }
            account = newAccount;
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

        private static void ReadAndAddNewTransaction(string[] parts)
        {
            var recipient = parts.FirstOrDefault();
            while (string.IsNullOrEmpty(recipient))
            {
                Console.Write("Recipient: ");
                recipient = Console.ReadLine();
            }
            var message = parts.Skip(1).FirstOrDefault();

            var index = blockchain.NewTransaction(account, recipient, message);
            Console.WriteLine($"Transaction added to block #{index}");
        }

        private static Task MineAndPrintNewBlock()
        {
            return Task.Run(() =>
            {
                var block = blockchain.Mine(account);
                Console.WriteLine(block.ToString());
            });
        }

        private static void PrintBlockchain()
        {
            Console.WriteLine($"Blocks count = {blockchain.Chain.Count}");
            foreach (var block in blockchain.Chain)
            {
                Console.WriteLine(block.ToString());
                if (block.Content.Count > 0)
                {
                    Console.WriteLine(string.Join("\r\n", block.Content));
                }
            }
            Console.WriteLine();
        }

        private static void PrintCurrentTransactions()
        {
            Console.WriteLine($"Current transactions {blockchain.CurrentTransactions.Count}:");
            Console.WriteLine(string.Join("\r\n", blockchain.CurrentTransactions));
        }

        private static async void SyncBlockchain()
        {
            await communicator.SyncAsync();
            Console.WriteLine("Sync messages sended");
        }

        private static void SaveBlockChain(string? fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "chain.protobuf";
            }

            try
            {
                blockchain.SaveFile(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Blockchain saved to " + fileName);
        }

        private static MessengerBlockchain LoadBlockChain(string? fileName = "chain.protobuf")
        {
            try
            {
                return MessengerBlockchain.FromFile(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return MessengerBlockchain.CreateNew();
            }
        }
    }
}

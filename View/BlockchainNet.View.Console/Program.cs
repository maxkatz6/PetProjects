namespace BlockchainNet.Console
{
    using System;
    using System.Linq;
    using System.Drawing;
    using System.Threading.Tasks;

    using BlockchainNet.Messenger;
    using BlockchainNet.Core.EventArgs;
    using BlockchainNet.Messenger.Models;

    internal static class Program
    {
        private static MessengerServiceLocator messengerServiceLocator;

        private static string account;
        private static (byte[] publicKey, byte[] privateKey) keys;

        private static async Task Main()
        {
            messengerServiceLocator = new MessengerServiceLocator();
            
            await messengerServiceLocator.Communicator.StartAsync().ConfigureAwait(false);

            Console.Title = messengerServiceLocator.Communicator.ServerId;

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
                        await messengerServiceLocator.Communicator.ConnectToAsync(parts.Skip(1)).ConfigureAwait(false);
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
                        _ = await messengerServiceLocator.Blockchain.MineAsync(account, default).ConfigureAwait(false);
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

            await messengerServiceLocator.Communicator.CloseAsync().ConfigureAwait(false);
        }

        private static void Blockchain_BlockchainReplaced(object sender, EventArgs e)
        {
            Console.WriteLine("Blockchain replaced");
        }

        private static void Blockchain_BlockAdded(object sender, BlockAddedEventArgs<MessageInstruction> e)
        {
            var savedColor = Console.ForegroundColor;
            foreach (var transaction in e.AddedBlocks.SelectMany(b => b.Transactions))
            {
                if (transaction.Content.HexColor is string color)
                {
                    _ = SetScreenColorsApp.SetScreenColors(ColorTranslator.FromHtml(color), Color.Black);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                if (transaction.Recipient == account)
                {
                    Console.WriteLine($"({transaction.Date}) {transaction.Sender}: {transaction.Content.Message}");
                    Console.Beep();
                }
                else if (transaction.Sender == account)
                {
                    Console.WriteLine($"({transaction.Date}) You: {transaction.Content.Message}");
                }
            }
            Console.ForegroundColor = savedColor;
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
            keys = messengerServiceLocator.SignatureService.GetKeysFromPassword(password);
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
            var instruction = new MessageInstruction(message)
            {
                HexColor = ColorTranslator.ToHtml(Color.FromArgb(
                    new Random().Next(0, 255),
                    new Random().Next(0, 255),
                    new Random().Next(0, 255)
                ))
            };

            messengerServiceLocator.Blockchain.NewTransaction(account, recipient, instruction, keys);

            _ = await messengerServiceLocator.Blockchain.MineAsync(account, default).ConfigureAwait(false);
        }

        private static async Task PrintBlockchainAsync()
        {
            var blocks = await messengerServiceLocator.BlockRepository.GetFork(MessengerBlockchain.RootId).ToListAsync();
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
            Console.WriteLine($"Current transactions {messengerServiceLocator.Blockchain.CurrentTransactions.Count}:");
            Console.WriteLine(string.Join("\r\n", messengerServiceLocator.Blockchain.CurrentTransactions));
        }
    }
}

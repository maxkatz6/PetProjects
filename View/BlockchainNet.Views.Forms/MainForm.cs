﻿namespace BlockchainNet.View.Forms
{
    using BlockchainNet.Core.Models;
    using BlockchainNet.Wallet;
    using BlockchainNet.Core;
    using BlockchainNet.IO.Pipe;
    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        private static Communicator<WalletBlockchain, decimal> communicator;
        private static WalletBlockchain blockchain;
        private string userAccount;

        public MainForm()
        {
            InitializeComponent();

            var pipeId = ProcessPipeHelper.GetCurrentPipeId();

            blockchain = WalletBlockchain.CreateNew();

            communicator = new Communicator<WalletBlockchain, decimal>(
                new PipeServer<List<Block<decimal>>>(pipeId),
                new PipeClientFactory<List<Block<decimal>>>())
            {
                Blockchain = blockchain
            };

            communicator
                .ConnectTo(ProcessPipeHelper
                .GetNeighborPipesIds());

            senderTextBox.Text = userAccount = AskForAccount(true)!;

            UpdateBlocksList();
            UpdateTransactionsList();
            UpdateUserAmount();

            blockchain.BlockchainReplaced += (s, a) =>
            {
                UpdateBlocksList();
                UpdateTransactionsList();
                UpdateUserAmount();
            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            communicator.Close();
            base.OnClosing(e);
        }

        private void AccountAmount_Click(object sender, EventArgs e)
        {
            var account = AskForAccount(false, userAccount);
            if (account == null)
            {
                return;
            }

            var amount = blockchain.GetAccountAmount(account);
            MessageBox.Show(amount + " монет на счету аккаунта \"" + account + "\"", "Вывод счета");
        }

        private string? AskForAccount(bool requied, string? def = null)
        {
            InputBox.Show("Введите аккаунт", "Ввод аккаунта", def, out var account);
            if (string.IsNullOrWhiteSpace(account))
            {
                MessageBox.Show("Аккаунт не может быть пустым");
                if (requied)
                {
                    return AskForAccount(requied, def);
                }

                return null;
            }
            return account;
        }

        private void SaveBlockchainClick(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    blockchain.SaveFile(saveFileDialog.FileName);
                    MessageBox.Show("Успешно сохранено");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void LoadBlockchainClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    communicator.Blockchain = blockchain
                        = WalletBlockchain.FromFile(openFileDialog.FileName);

                    UpdateBlocksList();
                    UpdateTransactionsList();
                    UpdateUserAmount();
                    blockchain.BlockchainReplaced += (s, a) =>
                    {
                        UpdateBlocksList();
                        UpdateTransactionsList();
                        UpdateUserAmount();
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SwithAccountClick(object sender, EventArgs e)
        {
            var newAccount = AskForAccount(false, userAccount);
            if (!string.IsNullOrEmpty(newAccount))
            {
                senderTextBox.Text = userAccount = newAccount;
            }
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private async void SyncGetBlocksClick(object sender, EventArgs e)
        {
            await communicator.SyncAsync(true);
        }

        private async void SyncBlocksClick(object sender, EventArgs e)
        {
            await communicator.SyncAsync();
        }

        private void SendTransactionClick(object sender, EventArgs e)
        {
            var recipient = recipTextBox.Text;
            if (string.IsNullOrEmpty(recipient))
            {
                MessageBox.Show("Аккаунт получателя не может быть пустым");
                return;
            }
            var amount = amountDropDown.Value;

            if (blockchain.GetAccountAmount(userAccount) < amount)
            {
                MessageBox.Show("Не хватает монет на счету");
                return;
            }

            if (MessageBox.Show("Отправить " + amount + " монет на аккаунт \"" + recipient + "\"", "Подтверждение", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            blockchain.NewTransaction(userAccount, recipient, amount);

            UpdateTransactionsList();
            UpdateUserAmount();
        }

        private void UpdateBlocksList()
        {
            blocksDataGridView.Rows.Clear();
            foreach (var block in blockchain.Chain)
            {
                blocksDataGridView.Rows.Add(new object?[]
                    {
                        block.Index,
                        block.Date,
                        block.Proof,
                        block.PreviousHash
                    });
            }
        }

        private void UpdateTransactionsList(int blockIndex = -1)
        {
            transactionsDataGridView.Rows.Clear();
            var transactions = blockIndex >= 0
                ? blockchain.Chain
                    .FirstOrDefault(b => b.Index == blockIndex)?
                    .Content ?? new List<Transaction<decimal>>()
                : blockchain.CurrentTransactions;

            foreach (var transaction in transactions)
            {
                transactionsDataGridView.Rows.Add(new object?[]
                    {
                        transaction.Date,
                        transaction.Content,
                        transaction.Sender,
                        transaction.Recipient
                    });
            }
        }

        private void UpdateUserAmount()
        {
            amountDropDown.Maximum = blockchain.GetAccountAmount(userAccount);
        }

        private void BlocksDataGridViewSelectionChanged(object sender, EventArgs e)
        {
            if (blocksDataGridView.SelectedCells.Count == 0)
            {
                return;
            }

            var index = blocksDataGridView.Rows[blocksDataGridView.SelectedCells[0].RowIndex].Cells["Index"].Value as int?;

            UpdateTransactionsList(index ?? -1);
        }

        private async void MineClick(object sender, EventArgs e)
        {
            await Task.Run(() => blockchain.Mine(userAccount));
            MessageBox.Show("Создание блока завершено");
            UpdateBlocksList();
            UpdateTransactionsList();
        }
    }
}

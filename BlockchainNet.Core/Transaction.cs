﻿namespace BlockchainNet.Core
{
    using ProtoBuf;

    [ProtoContract]
    public class Transaction
    {
        [ProtoMember(1)]
        public string Sender { get; }

        [ProtoMember(2)]
        public string Recipient { get; }

        [ProtoMember(3)]
        public double Amount { get; }

        private Transaction()
        {

        }

        public Transaction(string sender, string recipient, double amout)
        {
            Sender = sender;
            Recipient = recipient;
            Amount = amout;
        }
    }
}
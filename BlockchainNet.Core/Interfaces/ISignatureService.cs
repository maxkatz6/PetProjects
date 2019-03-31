namespace BlockchainNet.Core.Interfaces
{
    using BlockchainNet.Core.Models;

    public interface ISignatureService
    {
        (byte[] publicKey, byte[] privateKey) GetKeysFromPassword(string phrase);
        void SignTransaction<TContent>(Transaction<TContent> instruction, byte[] privateKey);
        bool VerifyTransaction<TContent>(Transaction<TContent> instruction);
    }
}

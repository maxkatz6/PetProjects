namespace BlockchainNet.Core.Interfaces
{
    using BlockchainNet.Core.Models;

    public interface ISignatureService
    {
        (byte[] publicKey, byte[] privateKey) GetKeysFromPassword(string phrase);
        void SignTransaction<TInstruction>(Transaction<TInstruction> instruction, byte[] privateKey);
        bool VerifyTransaction<TInstruction>(Transaction<TInstruction> instruction);
    }
}

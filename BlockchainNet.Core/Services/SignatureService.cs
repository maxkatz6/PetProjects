namespace BlockchainNet.Core.Services
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Security.Cryptography;

    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;

    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.Nist;
    using Org.BouncyCastle.Asn1.Sec;
    using Org.BouncyCastle.Crypto.Parameters;

    public class SignatureService : ISignatureService
    {
        private readonly DerObjectIdentifier _curveId = SecObjectIdentifiers.SecP256r1;

        public (byte[] publicKey, byte[] privateKey) GetKeysFromPassword(string password)
        {
            using (var hasher = SHA256.Create())
            {
                var privateKey = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
                var privKeyInt = new BigInteger(privateKey);

                var parameters = NistNamedCurves.GetByOid(_curveId);
                var qa = parameters.G.Multiply(privKeyInt);

                var publicKey = qa.GetEncoded();

                return (publicKey, privateKey);
            }
        }

        public void SignTransaction<TContent>(Transaction<TContent> transaction, byte[] privateKey)
        {
            var bytesId = ResolveTransactionId(transaction);
            transaction.Id = Convert.ToBase64String(bytesId);

            var parameters = NistNamedCurves.GetByOid(_curveId);
            var privKeyInt = new BigInteger(privateKey);
            var dp = new ECDomainParameters(parameters.Curve, parameters.G, parameters.N);
            var cp = new ECPrivateKeyParameters("ECDSA", privKeyInt, dp);
            var signer = SignerUtilities.GetSigner("ECDSA");
            signer.Init(true, cp);
            signer.BlockUpdate(bytesId, 0, bytesId.Length);
            transaction.Signature = signer.GenerateSignature();
        }

        public bool VerifyTransaction<TContent>(Transaction<TContent> transaction)
        {
            if (transaction.Signature == null)
            {
                return false;
            }

            var bytesId = Convert.FromBase64String(transaction.Id);
            if (!bytesId.SequenceEqual(ResolveTransactionId(transaction)))
            {
                return false;
            }

            var parameters = NistNamedCurves.GetByOid(_curveId);
            var dp = new ECDomainParameters(parameters.Curve, parameters.G, parameters.N);
            var q = parameters.Curve.DecodePoint(transaction.PublicKey);
            var cp = new ECPublicKeyParameters("ECDSA", q, dp);
            var signer = SignerUtilities.GetSigner("ECDSA");
            signer.Init(false, cp);
            signer.BlockUpdate(bytesId, 0, bytesId.Length);
            return signer.VerifySignature(transaction.Signature);
        }

        private static byte[] ResolveTransactionId<TContent>(Transaction<TContent> transaction)
        {
            using (var h = SHA256.Create())
            {
                var data = transaction.GetHash().Concat(transaction.PublicKey).ToArray();
                return h.ComputeHash(data);
            }
        }
    }
}

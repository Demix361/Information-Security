using System.IO;
using System.Security.Cryptography;

namespace DigitalSignature
{
    class Signer
    {
        HashAlgorithm hashAlgorithm;
        RSACryptoServiceProvider cryptoProvider;
        RSAPKCS1SignatureDeformatter deformatter;
        RSAPKCS1SignatureFormatter formatter;

        public Signer(string hashAlgorithmName)
        {
            hashAlgorithm = HashAlgorithm.Create(hashAlgorithmName);

            cryptoProvider = new RSACryptoServiceProvider();
            formatter = new RSAPKCS1SignatureFormatter(cryptoProvider);
            deformatter = new RSAPKCS1SignatureDeformatter(cryptoProvider);

            formatter.SetHashAlgorithm(hashAlgorithmName);
            deformatter.SetHashAlgorithm(hashAlgorithmName);
        }

        public void Sign(string dataFilename, string signatureFilename)
        {
            FileStream dataFS = new FileStream(dataFilename, FileMode.Open);
            FileStream signatureFS = new FileStream(signatureFilename, FileMode.Create);
            BinaryWriter signatureWriter = new BinaryWriter(signatureFS);

            byte[] key = cryptoProvider.ExportCspBlob(false);
            signatureWriter.Write(key.Length);
            signatureWriter.Write(key);

            var hash = hashAlgorithm.ComputeHash(dataFS);
            var signed_hash = formatter.CreateSignature(hash);
            int hash_len = signed_hash.Length;

            signatureWriter.Write(hash_len);
            signatureWriter.Write(signed_hash);

            dataFS.Close();
            signatureWriter.Close();
            signatureFS.Close();
        }

        public bool CheckSignature(string dataFilename, string signatureFilename)
        {
            FileStream dataFS = new FileStream(dataFilename, FileMode.Open);
            FileStream signatureFS = new FileStream(signatureFilename, FileMode.Open);
            BinaryReader signatureReader = new BinaryReader(signatureFS);
            
            var key = signatureReader.ReadBytes(signatureReader.ReadInt32());
            cryptoProvider.ImportCspBlob(key);

            var tail = signatureReader.ReadInt32();

            var hash = hashAlgorithm.ComputeHash(dataFS);
            var signed_hash = signatureReader.ReadBytes(tail);

            dataFS.Close();
            signatureReader.Close();
            signatureFS.Close();

            return deformatter.VerifySignature(hash, signed_hash);
        }
    }
}

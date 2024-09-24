using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace FS.Common.Encryption
{ 
    public class EncryptionServices
    {
        private const string PrivateSaltedKey = "458678943525326356769826";

        public EncryptionServices()
        {
        }

        public string DefaultDecryption(string cypherText)
        {
            if (cypherText.Trim().Length == 0)
            {
                throw new System.Exception("Invalid value for the cypher text. Value can not be empty for the encryption.");
            }

            return this.Decrypt(cypherText, FS.Common.Encryption.EncryptionServices.Settings.EncryptPassPhrase,
                FS.Common.Encryption.EncryptionServices.Settings.EncryptSaltValue,
                FS.Common.Encryption.EncryptionServices.Settings.EncryptHashAlgorithm,
                FS.Common.Encryption.EncryptionServices.Settings.EncryptPassIterations,
                FS.Common.Encryption.EncryptionServices.Settings.EncryptInitVector, 256);
        }
        public string DefaultEncryption(string plainText)
        {
            if (plainText.Trim().Length == 0)
            {
                throw new System.Exception("Invalid value for the plain text. Value can not be empty for the encryption.");
            }

            return this.Encrypt(plainText, FS.Common.Encryption.EncryptionServices.Settings.EncryptPassPhrase,
                FS.Common.Encryption.EncryptionServices.Settings.EncryptSaltValue,
                FS.Common.Encryption.EncryptionServices.Settings.EncryptHashAlgorithm,
                FS.Common.Encryption.EncryptionServices.Settings.EncryptPassIterations,
                FS.Common.Encryption.EncryptionServices.Settings.EncryptInitVector, 256);
        }

        public string Encrypt(string plainText)
        {
            TripleDESCryptoServiceProvider crp = new TripleDESCryptoServiceProvider();
            UnicodeEncoding uEncode = new UnicodeEncoding();
            ASCIIEncoding aEncode = new ASCIIEncoding();
            MemoryStream stmCipherText = new MemoryStream();

            if (plainText == string.Empty)
                return string.Empty;
            if (plainText == null)
                return string.Empty;
            Byte[] bytPlainText = uEncode.GetBytes(plainText);
            crp.Key = aEncode.GetBytes(PrivateSaltedKey);
            crp.IV = aEncode.GetBytes(PrivateSaltedKey.Substring(0, 8));
            CryptoStream csEncrypted = new CryptoStream(stmCipherText, crp.CreateEncryptor(), CryptoStreamMode.Write);
            csEncrypted.Write(bytPlainText, 0, bytPlainText.Length);
            csEncrypted.FlushFinalBlock();

            return Convert.ToBase64String(stmCipherText.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            TripleDESCryptoServiceProvider crp = new TripleDESCryptoServiceProvider();
            UnicodeEncoding uEncode = new UnicodeEncoding();
            ASCIIEncoding aEncode = new ASCIIEncoding();
            MemoryStream stmPlainText = new MemoryStream();

            try
            {
                if (cipherText == string.Empty)
                    return string.Empty;
                if (cipherText == null)
                    return string.Empty;
                Byte[] bytCipherText = Convert.FromBase64String(cipherText);
                MemoryStream stmCipherText = new MemoryStream(bytCipherText);
                crp.Key = aEncode.GetBytes(PrivateSaltedKey);
                crp.IV = aEncode.GetBytes(PrivateSaltedKey.Substring(0, 8));

                CryptoStream csDecrypted = new CryptoStream(stmCipherText, crp.CreateDecryptor(), CryptoStreamMode.Read);
                StreamWriter sw = new StreamWriter(stmPlainText);
                StreamReader sr = new StreamReader(csDecrypted);

                sw.Write(sr.ReadToEnd());
                sw.Flush();
                csDecrypted.Clear();
                crp.Clear();
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                return string.Empty;
                //throw ex;
            }

            return uEncode.GetString(stmPlainText.ToArray());

        }
        public string Encrypt(string plainText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            string cipherText;
            Byte[] initVectorBytes = null;
            Byte[] saltValueBytes = null;
            Byte[] plainTextBytes = null;
            PasswordDeriveBytes password = null;
            Byte[] keyBytes = null;
            MemoryStream memoryStream = null;
            ICryptoTransform encryptor = null;
            CryptoStream cryptoStream = null;

            try
            {
                //Convert strings into byte arrays.
                //Let us assume that strings only contain ASCII codes.
                //If strings include Unicode characters, use Unicode, UTF7, or UTF8 
                //encoding.
                initVectorBytes = Encoding.ASCII.GetBytes(initVector);

                saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                //Convert our plaintext into a byte array.
                //Let us assume that plaintext contains UTF8-encoded characters.
                plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                //First, we must create a password, from which the key will be derived.
                //This password will be generated from the specified passphrase and 
                //salt value. The password will be created using the specified hash 
                //algorithm. Password creation can be done in several iterations.
                password = new PasswordDeriveBytes(passPhrase,
                    saltValueBytes,
                    hashAlgorithm,
                    passwordIterations);

                //Use the password to generate pseudo-random bytes for the encryption
                //key. Specify the size of the key in bytes (instead of bits).
                keyBytes = password.GetBytes((int)(keySize / 8));

                //Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey;
                symmetricKey = new RijndaelManaged();

                //It is reasonable to set encryption mode to Cipher Block Chaining
                //(CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                //Generate encryptor from the existing key bytes and initialization 
                //vector. Key size will be defined based on the number of the key 
                //bytes.
                encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                //Define memory stream which will be used to hold encrypted data.
                memoryStream = new MemoryStream();

                //Define cryptographic stream (always use Write mode for encryption).
                cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                //Start encrypting.
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                //Finish encrypting.
                cryptoStream.FlushFinalBlock();

                //Convert our encrypted data from a memory stream into a byte array.
                Byte[] cipherTextBytes;
                cipherTextBytes = memoryStream.ToArray();

                //Convert encrypted data into a base64-encoded string.
                cipherText = Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!(memoryStream == null))
                {
                    memoryStream.Close();
                }
                if (!(cryptoStream == null))
                {
                    cryptoStream.Close();
                }
            }
            //Return encrypted string.
            return cipherText;
        }

        public string Decrypt(string cipherText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            Byte[] initVectorBytes = null;
            Byte[] saltValueBytes = null;
            Byte[] cipherTextBytes = null;
            PasswordDeriveBytes password = null;
            Byte[] keyBytes = null;
            ICryptoTransform decryptor = null;
            RijndaelManaged symmetricKey = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;
            Byte[] plainTextBytes = null;
            Byte[] plainTextBytes2 = null;
            int decryptedByteCount;
            string plainText;

            try
            {
                //' Convert strings defining encryption key characteristics into byte
                //' arrays. Let us assume that strings only contain ASCII codes.
                //' If strings include Unicode characters, use Unicode, UTF7, or UTF8
                //' encoding.
                initVectorBytes = Encoding.ASCII.GetBytes(initVector);

                saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                //' Convert our ciphertext into a byte array.
                cipherTextBytes = Convert.FromBase64String(cipherText);

                //' First, we must create a password, from which the key will be 
                //' derived. This password will be generated from the specified 
                //' passphrase and salt value. The password will be created using
                //' the specified hash algorithm. Password creation can be done in
                //' several iterations.
                password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                //' Use the password to generate pseudo-random bytes for the encryption
                //' key. Specify the size of the key in bytes (instead of bits).
                keyBytes = password.GetBytes((System.Int32)(keySize / 8));

                //' Create uninitialized Rijndael encryption object.
                symmetricKey = new RijndaelManaged();

                //' It is reasonable to set encryption mode to Cipher Block Chaining
                //' (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                //' Generate decryptor from the existing key bytes and initialization 
                //' vector. Key size will be defined based on the number of the key 
                //' bytes.
                decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                //' Define memory stream which will be used to hold encrypted data.
                memoryStream = new MemoryStream(cipherTextBytes);

                //' Define memory stream which will be used to hold encrypted data.
                cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                //' Since at this point we don't know what the size of decrypted data
                //' will be, allocate the buffer long enough to hold ciphertext;
                //' plaintext is never longer than ciphertext.
                //ReDim plainTextBytes(cipherTextBytes.Length);
                plainTextBytes2 = new Byte[cipherTextBytes.Length];
                for (int i = 0; i < plainTextBytes.GetUpperBound(0); i++)
                {
                    plainTextBytes2[i] = plainTextBytes[i];
                }
                //' Start decrypting.
                decryptedByteCount = cryptoStream.Read(plainTextBytes2, 0, plainTextBytes2.Length);

                //' Convert decrypted data into a string. 
                //' Let us assume that the original plaintext string was UTF8-encoded.
                plainText = Encoding.UTF8.GetString(plainTextBytes2, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!(memoryStream == null))
                {
                    memoryStream.Close();
                }
                if (!(cryptoStream == null))
                {
                    cryptoStream.Close();
                }

            }
            //' Return decrypted string.
            return plainText;
        }
        public class Settings
        {
            public const string EncryptPassPhrase = "&PUIH%$&*";
            public const string EncryptSaltValue = "(*(&HY%R%*";
            public const string EncryptHashAlgorithm = "SHA1";
            public const System.Int32 EncryptPassIterations = 2;
            public const string EncryptInitVector = "K)(&T^&%$*R%IGFT";

            public Settings()
            {
            }
        }
    }
}

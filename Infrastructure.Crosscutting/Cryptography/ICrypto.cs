
using System;
using System.Security.Cryptography;
using System.Text; 
using System.Configuration;

namespace Infrastructure.CrossCutting.Cryptography
{
    /// <summary>
    /// Cryptography interface to encrypt and decrypt strings.
    /// </summary>
    public interface ICrypto
    {
        /// <summary>
        /// Options for encryption.
        /// </summary>
        CryptoConfig Settings { get; }


        /// <summary>
        /// Encrypts a string.
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        string Encrypt(string plaintext);


        /// <summary>
        /// Decrypt the encrypted text.
        /// </summary>
        /// <param name="base64Text">The encrypted base64 text</param>
        /// <returns></returns>
        string Decrypt(string base64Text);


        /// <summary>
        /// Determine if encrypted text can be matched to unencrypted text.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="plainText"></param>
        /// <returns></returns>
        bool IsMatch(string encrypted, string plainText);
    }
}

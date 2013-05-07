using System;
using System.Security.Cryptography;
using System.Text; 
using System.Configuration;

namespace Infrastructure.CrossCutting.Cryptography
{

    /// <summary>
    /// Cryptography service to encrypt and decrypt strings.
    /// </summary>
    public class Crypto
	{
        private static ICrypto _provider;


        /// <summary>
        /// Create default instance of symmetric cryptographer.
        /// </summary>
        static Crypto()
        {
            _provider = new CryptoSym();
        }


        /// <summary>
        /// Initialize to new provider.
        /// </summary>
        /// <param name="service"></param>
        public static void Init(ICrypto service)
        {
            _provider = service;
        }


        /// <summary>
        /// Get reference to current encryption provider.
        /// </summary>
        public static ICrypto Provider
        {
            get { return _provider; }
        }        


		/// <summary>
		/// Encrypts the plaintext using an internal private key.
		/// </summary>
		/// <param name="plaintext">The text to encrypt.</param>
		/// <returns>An encrypted string in base64 format.</returns>
		public static string Encrypt( string plaintext )
		{
            return _provider.Encrypt(plaintext);
		}


		/// <summary>
		/// Decrypts the base64key using an internal private key.
		/// </summary>
		/// <param name="base64Text">The encrypted string in base64 format.</param>
		/// <returns>The plaintext string.</returns>
        public static string Decrypt( string base64Text )
		{
            return _provider.Decrypt(base64Text);
		}


        /// <summary>
        /// Determine if the plain text and encrypted are ultimately the same.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static bool IsMatch(string encrypted, string plainText)
        {
            return _provider.IsMatch(encrypted, plainText);
        }


        /// <summary>
        /// Calculate the md5 hash of the input text.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // Now convert to hex.
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
	}
}

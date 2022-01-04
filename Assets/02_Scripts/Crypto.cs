using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace NORK
{
    public class Crypto : MonoBehaviour
    {
        private const string str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public List<Manager_Common.Data_Dictionary> lists = new List<Manager_Common.Data_Dictionary>();
        [TextArea]
        public string _decrypto;
        
        [ContextMenu("2")]
        public void Test_Decrypto()
        {
            Debug.Log(Decompress_Text(_decrypto));
        }
        /// <summary>
        /// 문자열 압축 풀기
        /// </summary>
        public string Decompress_Text(string _str)
        {
            byte[] _byte;

            var compressedStream = new MemoryStream(Convert.FromBase64String(_str));

            using (var decompressorStream = new System.IO.Compression.DeflateStream(compressedStream, System.IO.Compression.CompressionMode.Decompress))
            {
                using (var decompressedStream = new MemoryStream())
                {
                    decompressorStream.CopyTo(decompressedStream);

                    _byte = decompressedStream.ToArray();
                }
            }

            return Encoding.UTF8.GetString(_byte);
        }
        public static string Encrypt_Web(Manager_Common.Data_Dictionary[] dictionaries)
        {
            string _encrypt = string.Empty;

            #region To Json
            _encrypt = "{";
            for (int i = 0; i < dictionaries.Length; i++)
            {
                _encrypt += "\"";
                _encrypt += dictionaries[i].key;
                _encrypt += "\":";
                _encrypt += "\"";
                _encrypt += dictionaries[i].value;
                _encrypt += "\"";

                if (i < dictionaries.Length - 1)
                    _encrypt += ",";
            }
            _encrypt += "}";
            #endregion

            #region To Base64
            byte[] _b = Encoding.UTF8.GetBytes(_encrypt);
            _encrypt = Convert.ToBase64String(_b);
            #endregion

            #region 중간에 값 넣기
            _encrypt = _encrypt.Insert(0, str[UnityEngine.Random.Range(0, str.Length)].ToString());
            #endregion

            #region 순서바꾸기
            int _l1 = (int)(_encrypt.Length * 0.3f);
            int _l2 = _l1 * 2;
            int _l3 = _l1 * 3;
            string _s1 = _encrypt.Substring(0, _l1);
            string _s2 = _encrypt.Substring(_l1, _l1);
            string _s3 = _encrypt.Substring(_l2, _l1);
            string _s4 = _encrypt.Substring(_l3, _encrypt.Length - _l3);
            _encrypt = _s4 + _s2 + _s3 + _s1;
            #endregion

            _encrypt = Encrypt(_encrypt);

            #region 값 변경
            _encrypt = _encrypt.Replace('1', '～');
            _encrypt = _encrypt.Replace('a', 'ㆎ');
            #endregion

            return _encrypt;
        }

        public static string Decrypt_Web(string cipherText)
        {
            string _decrypt = cipherText;

            #region 값 변경
            _decrypt = _decrypt.Replace('～', '1');
            _decrypt = _decrypt.Replace('ㆎ', 'a');
            #endregion

            _decrypt = Decrypt(_decrypt);

            #region 순서바꾸기
            int length = _decrypt.Length;
            int _l1 = (int)(length * 0.3f);
            int remainder = length - _l1 * 3;
            string _s4 = _decrypt.Substring(0, remainder);
            string tmp_ds = _decrypt.Substring(remainder);
            string _s3 = tmp_ds.Substring(0, _l1);
            string _s2 = tmp_ds.Substring(_l1, _l1);
            string _s1 = tmp_ds.Substring(_l1 * 2, _l1);
            _decrypt = _s1 + _s3 + _s2 + _s4;
            #endregion

            #region 중간에 값 넣기

            //StringBuilder sb = new StringBuilder();
            //for (int i = 1; i < length; i += 2)
            //{
            //    sb.Append(_decrypt[i]);
            //}
            //_decrypt = sb.ToString();
            //Debug.Log(_decrypt);
            _decrypt = _decrypt.Substring(1);
            #endregion

            #region To Base64
            byte[] _b = Convert.FromBase64String(_decrypt);
            _decrypt = Encoding.UTF8.GetString(_b);
            #endregion

            return _decrypt;
        }

        private const string initVector = "dj401;cn=g.mc8-_";
        private const string passPhrase = "sldkfghqpwo!)($%_+sr=g234";

        private const int keysize = 256;

        public static string Encrypt(string plainText)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}


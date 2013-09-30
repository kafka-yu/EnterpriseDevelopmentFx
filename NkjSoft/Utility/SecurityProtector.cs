//--------------------------版权信息----------------------------
//       
//                 文件名: SecurityProtector                 
//                 CLR Version: 2.0.50727.4927
//                 项目命名空间: NkjSoft.Utility
//
//                 作  者: 俞如凯 
//                 Q Q: 250820436　 yurukai@vip.qq.com
//                 E-Mail: yurukai@hotmail.com
//                 创建时间 : 2010/7/22 20:33:12
//                 Copyright (c) Yurukai , All rights reserved.
//----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;

namespace NkjSoft.Utility
{
    /// <summary>
    /// 提供对文本进行 MD5、DES 加密解密的功能。无法继承此类。
    /// </summary>
    public sealed class SecurityProtector
    {
        /// <summary>
        /// 无需参数的方法实例化一个新的 <see cref="NkjSoft.Utility.SecurityProtector"/> 对象。
        /// </summary>
        public SecurityProtector()
        {

        }

        #region 公共方法
        /// <summary>
        /// 对指定的 <see cref="System.String"/> 文本进行DES加密，返回加密之后的 <see cref="System.String"/>。。该方法还接收一个 <see cref="System.String"/> 作为加密密匙。
        /// </summary>
        /// <param name="stringToBeEncrypt">需要加密的 <see cref="System.String"/> 文本.</param>
        /// <param name="desKey">一个 <see cref="System.String"/> 作为加密密匙 </param>
        /// <exception cref="System.ArgumentNullException">加密密匙为空,或者加密文本为空</exception>
        /// <exception cref="System.Exception">无法进行加密</exception>
        /// <returns>被加密之后的 <see cref="System.String"/></returns>
        public string DesEncrypt(string stringToBeEncrypt, string desKey)
        {
            if (String.IsNullOrEmpty(stringToBeEncrypt))
            { throw new ArgumentNullException("stringToBeEncrypt", "加密文本不能为空!"); }
            if (string.IsNullOrEmpty(desKey))
            { throw new ArgumentNullException("desKey", "加密密匙不能为空!"); }
            try
            {
                byte[] MyStr_E = Encoding.UTF8.GetBytes(stringToBeEncrypt);
                byte[] MyKey_E = Encoding.UTF8.GetBytes(desKey);

                DESCryptoServiceProvider MyDes_E = new DESCryptoServiceProvider();
                MyDes_E.Key = MyKey_E;
                MyDes_E.IV = MyKey_E;

                MemoryStream MyMem_E = new MemoryStream();

                CryptoStream MyCry_E = new CryptoStream(MyMem_E, MyDes_E.CreateEncryptor(), CryptoStreamMode.Write);
                MyCry_E.Write(MyStr_E, 0, MyStr_E.Length);
                MyCry_E.FlushFinalBlock();
                MyCry_E.Close();

                string mydesStr = Convert.ToBase64String(MyMem_E.ToArray());
                return mydesStr;
            }
            catch (Exception Error)
            {
                throw Error;
            }
        }

        /// <summary>
        /// 对指定的 <see cref="System.String"/> 文本进行DES解密，返回解密之后的 <see cref="System.String"/>。该方法需要一个 <see cref="System.String"/> 作为解密密匙。
        /// </summary>
        /// <param name="ToBeDeEncrypt">To be de encrypt.</param>
        /// <param name="desKey">一个 <see cref="System.String"/> 作为解密密匙</param>
        /// <returns>解密之后的 <see cref="System.String"/></returns>
        /// <exception cref="System.ArgumentNullException"> 解密密匙为空</exception>
        /// <exception cref="System.Exception">无法进行解密</exception>
        public string DesDecrypt(string ToBeDeEncrypt, string desKey)
        {
            if (string.IsNullOrEmpty(desKey))
            { throw new ArgumentNullException("desKey", "解密密匙不能为空!"); }
            try
            {
                byte[] MyStr_D = Convert.FromBase64String(ToBeDeEncrypt);
                byte[] MyKey_D = Encoding.UTF8.GetBytes(desKey);

                DESCryptoServiceProvider MyDes_D = new DESCryptoServiceProvider();
                MyDes_D.Key = MyKey_D;
                MyDes_D.IV = MyKey_D;

                MemoryStream MyMem_D = new MemoryStream();

                CryptoStream MyCry_D = new CryptoStream(MyMem_D, MyDes_D.CreateDecryptor(), CryptoStreamMode.Write);
                MyCry_D.Write(MyStr_D, 0, MyStr_D.Length);
                MyCry_D.FlushFinalBlock();
                MyCry_D.Close();

                string result = Encoding.UTF8.GetString(MyMem_D.ToArray());
                return result;
            }
            catch (Exception Error)
            {
                throw Error;
            }
        }

        /// <summary>
        /// 使用指定的MD5加密密匙，对指定文本进行MD5加密。
        /// </summary>
        /// <param name="pToEncrypt">需要加密的文本</param>
        /// <param name="sKey">MD5加密密匙.</param>
        /// <returns>加密后的文本</returns> 
        public string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();


        }

        /// <summary>
        ///  使用指定的MD5解密密匙，对经过MD5加密的文本进行解密。
        /// </summary>
        /// <param name="pToDecrypt">经过MD5加密的文本</param>
        /// <param name="sKey">解密密匙.</param>
        /// <returns>解密后的文本</returns> 
        public string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        // 创建Key
        /// <summary>
        /// 生成一个 <see cref="System.String"/>，该字符串可用于DES、MD5算法的加密解密。
        /// </summary>
        /// <returns>一个 <see cref="System.String"/> 密匙</returns>
        public string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }
        /// <summary>
        /// 使用DES算法对指定的文本进行加密。
        /// </summary>
        /// <param name="sInputString"> 需要加密的文本.</param>
        /// <param name="sKey">密匙.</param>
        /// <returns>加密后的文本</returns>
        public string EncryptString(string sInputString, string sKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(sInputString);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return BitConverter.ToString(result);
        }

        /// <summary>
        /// 使用DES算法对经过DES加密的文本进行解密。
        /// </summary>
        /// <param name="sInputString">经过DES加密的文本</param>
        /// <param name="sKey">密匙.</param>
        /// <returns>解密后的文本</returns>
        public string DecryptString(string sInputString, string sKey)
        {
            string[] sInput = sInputString.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateDecryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }



    }
}

////*********************************************************
////������ó : �ƽ����(http://www.hoonsbara.com/hoonsboard.aspx?table_name=cshaptip&board_idx=445778&page=1&keyword=&search=&boardmode=2)
////*********************************************************

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace lib.Common.Crypto
{

    /// <summary>
    /// ��Ʈ��/���� ��/��ȣȭ Ŭ����(���Ī �˰��� ���)
    /// </summary>
    public class CryptoEx
    {
        #region enums, constants & fields

        /// <summary>
        /// ���Ī ��/��ȣȭ �˰��� ���
        /// </summary>
        public enum CryptoTypes
        {
            encTypeDES = 0,
            encTypeRC2,
            encTypeRijndael,
            encTypeTripleDES
        }
        

        const string CRYPT_DEFAULT_PASSWORD = "ChangDol";
	    const CryptoTypes CRYPT_DEFAULT_METHOD = CryptoTypes.encTypeDES;

        private byte[] mKey = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24};
        private byte[] mIV = {65, 110, 68, 26, 69, 178, 200, 219};
        private byte[] SaltByteArray  = {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76};
        private CryptoTypes mCryptoType = CRYPT_DEFAULT_METHOD;
        private string mPassword = CRYPT_DEFAULT_PASSWORD;

        private string sInputFile = string.Empty;
        private string sOutputFile = string.Empty;
                
        #endregion
 
        #region Constructors
        /// <summary>
        /// ����Ʈ ������
        /// </summary>
        public CryptoEx()
        {
            calculateNewKeyAndIV();
        }


        /// <summary>
        /// ������(��/��ȣȭ Ÿ���� ����)
        /// </summary>
        /// <param name="CryptoType"></param>
        public CryptoEx(CryptoTypes CryptoType)
        {
            this.mCryptoType = CryptoType;
        }
        #endregion

        #region Props
        /// <summary>
        ///     type of encryption / decryption used
        /// </summary>
        public CryptoTypes CryptoType
        {
            get
            {
                return mCryptoType;
            }

            set
            {
                mCryptoType = value;
                calculateNewKeyAndIV();
            }
        }


        /// <summary>
        ///     Passsword Key Property.
        ///     The password key used when encrypting / decrypting
        /// </summary>
        public string Password
        {
            get
            {
                return mPassword;
            }

            set
            {   
                if (mPassword != value)
                {
                    mPassword = value;
                    calculateNewKeyAndIV();
                }
            }
        }

        #endregion


        #region Encryption

        /// <summary>
        /// �ؽ�Ʈ�� ��ȣȭ�Ѵ�.
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        public string Encrypt(string inputText)
        {
            //declare a new encoder
            System.Text.UTF8Encoding UTF8Encoder = new System.Text.UTF8Encoding();

            //get byte representation of string
            byte[] inputBytes = UTF8Encoder.GetBytes(inputText);

            //convert back to a string
            return Convert.ToBase64String(EncryptDecrypt(ref inputBytes,true));
        }
 

        /// <summary>
        /// ����ڰ� ������ �н������ ��ȣȭ�Ѵ�.
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <param name="password">��ȣȭ�� ����� �н�����</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        public string Encrypt(string inputText, string password)
        {
            this.mPassword = password;
            return this.Encrypt(inputText);
        }

 

        /// <summary>
        /// ����ڰ� ������ cryptoType�� �н������ �ؽ�Ʈ�� ��ȣȭ�Ѵ�.
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <param name="password">��ȣȭ�� ����� �н�����</param>
        /// <param name="cryptoType">��ȣȭ Ÿ��</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        public string Encrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            this.mCryptoType = cryptoType;
            return this.Encrypt(inputText,password);
        }

        /// <summary>
        /// ����ڰ� ������ cryptoType���� �ؽ�Ʈ�� ��ȣȭ�Ѵ�.
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <param name="cryptoType">��ȣȭ Ÿ��</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        public string Encrypt(string inputText, CryptoTypes cryptoType)
        {
            this.mCryptoType = cryptoType;
            return this.Encrypt(inputText);
        }
 
        #endregion
 
        #region Decryption
        /// <summary>
        /// �ؽ�Ʈ�� ��ȣȭ�Ѵ�.
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        public string Decrypt(string inputText)
        {   
            //declare a new encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();

            //get byte representation of string
            byte[] inputBytes = Convert.FromBase64String(inputText);

            //convert back to a string
            return UTF8Encoder.GetString(EncryptDecrypt(ref inputBytes,false));
        }

 
        /// <summary>
        /// ����ڰ� ������ �н�����Ű�� ���� �ؽ�Ʈ�� ��ȣȭ�Ѵ�.
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <param name="password">��ȣȭ�� �� ����� �н�����</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        public string Decrypt(string inputText, string password)
        {
            this.mPassword = password;
            return Decrypt(inputText);
        }

 
        /// <summary>
        /// ����ڰ� ������ cryptoType�� �н������ �ؽ�Ʈ�� ��ȣȭ�Ѵ�.
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <param name="password">��ȣȭ�� ����� �н�����</param>
        /// <param name="cryptoType">��ȣȭ Ÿ��</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        public string Decrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            this.mCryptoType = cryptoType;
            return Decrypt(inputText,password);
        }

        /// <summary>
        /// ����ڰ� ������ cryptoType���� �ؽ�Ʈ�� ��ȣȭ�Ѵ�.
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <param name="cryptoType">��ȣȭ Ÿ��</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        public string Decrypt(string inputText, CryptoTypes cryptoType)
        {
            this.mCryptoType = cryptoType;
            return Decrypt(inputText);
        }
        #endregion
 
        #region Symmetric Engine
 
        /// <summary>
        ///     performs the actual enc/dec.
        /// </summary>
        /// <param name="inputBytes">input byte array</param>
        /// <param name="Encrpyt">wheather or not to perform enc/dec</param>
        /// <returns>byte array output</returns>
        private byte[] EncryptDecrypt(ref byte[] inputBytes, bool Encrpyt)
        {
            //get the correct transform
            ICryptoTransform transform = getCryptoTransform(Encrpyt);
 
            //memory stream for output
            MemoryStream memStream = new MemoryStream();    
 
            try
            {
                //setup the cryption - output written to memstream
                CryptoStream cryptStream = new CryptoStream(memStream,transform,CryptoStreamMode.Write);
 
                //write data to cryption engine
                cryptStream.Write(inputBytes,0,inputBytes.Length);
 
                //we are finished
                cryptStream.FlushFinalBlock();
                
                //get result
                byte[] output = memStream.ToArray();
 
                //finished with engine, so close the stream
                cryptStream.Close();
 
                return output;
            }
            catch (Exception e)
            {
                //throw an error
                throw new Exception("Error in symmetric engine. Error : " + e.Message,e);
            }
        }
 
        /// <summary>
        ///     returns the symmetric engine and creates the encyptor/decryptor
        /// </summary>
        /// <param name="encrypt">whether to return a encrpytor or decryptor</param>
        /// <returns>ICryptoTransform</returns>
        private ICryptoTransform getCryptoTransform(bool encrypt)
        {
            SymmetricAlgorithm SA = selectAlgorithm();
            SA.Key = mKey;
            SA.IV = mIV;
            if (encrypt)
            {
                return SA.CreateEncryptor();
            }
            else
            {
                return SA.CreateDecryptor();
            }
        }

        /// <summary>
        ///     returns the specific symmetric algorithm acc. to the cryptotype
        /// </summary>
        /// <returns>SymmetricAlgorithm</returns>
        private SymmetricAlgorithm selectAlgorithm()
        {
            SymmetricAlgorithm SA = null;
            switch (mCryptoType)
            {
                case CryptoTypes.encTypeDES:
                    SA = DES.Create();
                    break;
                case CryptoTypes.encTypeRC2:
                    SA = RC2.Create();
                    break;
                case CryptoTypes.encTypeRijndael:
                    SA = Rijndael.Create();
                    break;
                case CryptoTypes.encTypeTripleDES:
                    SA = TripleDES.Create();
                    break;
                default:
                    SA = TripleDES.Create();
                    break;
            }
            
            return SA;
            
        }
 
        /// <summary>
        ///     calculates the key and IV acc. to the symmetric method from the password
        ///     key and IV size dependant on symmetric method
        /// </summary>
        private void calculateNewKeyAndIV()
        {
    		//// ���ο� �޼���� ��ü�Ǿ���.
    		//// Dim pdb As PasswordDeriveBytes = New PasswordDeriveBytes(mPassword, SaltByteArray)
    		Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(mPassword, SaltByteArray);            
            
    		SymmetricAlgorithm algo = selectAlgorithm();
    		mKey = pdb.GetBytes(Convert.ToInt32(algo.KeySize / 8));
    		mIV = pdb.GetBytes(Convert.ToInt32(algo.BlockSize / 8));
        }
 
        #endregion
 
        #region File Encryption/Decryption
        /// <summary>
        /// ��ȣȭ ó�� �Լ�(DES �˰���)
        /// </summary>
        /// <param name="sInputFile">�Է� ����</param>
        /// <param name="sOutputFile">��� ����</param>
        public void EncryptFile(string sInputFile, string sOutputFile)
        {
            EncryptOrDecryptFile(sInputFile, sOutputFile, 1);
        }
 
        /// <summary>
        /// ��ȣȭ ó�� �Լ�(DES �˰���)
        /// </summary>
        /// <param name="sInputFile">�Է� ����</param>
        /// <param name="sOutputFile">��� ����</param>
        public void DecryptFile(string sInputFile, string sOutputFile)
        {
            EncryptOrDecryptFile(sInputFile, sOutputFile, 2);
        }
 
        /// <summary>
        /// ��ȣȭ/��ȣȭ ó�� �Լ�(DES �˰���)
        /// </summary>
        /// <param name="sInputFile">�Է� ����</param>
        /// <param name="sOutputFile">��� ����</param>
        /// <param name="Direction">��ȣȭ(1)/��ȣȭ(2) ����</param>
        private void EncryptOrDecryptFile(string sInputFile, string sOutputFile, int Direction)
        {
            //          byte[] byteDESKey = GetKeyByteArray(password);
            //          byte[] byteDESIV = GetKeyByteArray(ivpassword);
 
            // ���� ��Ʈ���� ����� �Է� �� ��� ������ ó���մϴ�.
            FileStream fsInput = new FileStream(sInputFile, FileMode.Open, FileAccess.Read);
            FileStream fsOutput = new FileStream(sOutputFile, FileMode.OpenOrCreate, FileAccess.Write);
            fsOutput.SetLength(0);
 
            // ��ȣȭ/��ȣ �ص� ���μ��� �� �ʿ��� �����Դϴ�.
            byte[] byteBuffer = new byte[4097]; // ó���� ���� ����Ʈ ����� �����մϴ�.
            long nBytesProcessed = 0; // ��ȣȭ�� ����Ʈ�� ���� ī��Ʈ
            long nFileLength = fsInput.Length;
            int iBytesInCurrentBlock = 0;
            CryptoStream csMyCryptoStream = null;
 
            switch (mCryptoType)
            {
                case CryptoTypes.encTypeDES:
                    csMyCryptoStream = DESCSP(fsOutput, Direction);
                    break;
                case CryptoTypes.encTypeRC2:
                    csMyCryptoStream = RC2CSP(fsOutput, Direction);
                    break;
                case CryptoTypes.encTypeRijndael:
                    csMyCryptoStream = RijndaelCSP(fsOutput, Direction);
                    break;
                case CryptoTypes.encTypeTripleDES:
                    csMyCryptoStream = TripleDESCSP(fsOutput, Direction);
                    break;
                default:
                    csMyCryptoStream = DESCSP(fsOutput, Direction);
                    break;
            }
 
            // �Է� ���Ͽ��� ���� ���� ��ȣȭ�ϰų� ��ȣ�� �ص��ϰ�
            // ��� ���Ͽ� ���ϴ�.
            while(nBytesProcessed < nFileLength)
            {
                iBytesInCurrentBlock = fsInput.Read(byteBuffer, 0, 4096);
                csMyCryptoStream.Write(byteBuffer, 0, iBytesInCurrentBlock);
    			nBytesProcessed = nBytesProcessed + Convert.ToInt64(iBytesInCurrentBlock);
            }
 
            csMyCryptoStream.Close();
            fsInput.Close();
            fsOutput.Close();
        }
 
        private CryptoStream DESCSP(FileStream fsOutput, int Direction)
        {
            DESCryptoServiceProvider Provider = new DESCryptoServiceProvider();
    		CryptoStream csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(), CryptoStreamMode.Read);
 
            // ��ȣȭ�� ��ȣ �ص��� ���� ����
            switch(Direction)
            {
                case 1 : // ��ȣȭ
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
                case 2 :// ��ȣȭ
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }
 
            return csMyCryptoStream;
        }
 
        private CryptoStream RC2CSP(FileStream fsOutput, int Direction)
        {
            RC2CryptoServiceProvider Provider = new RC2CryptoServiceProvider();
 		    CryptoStream csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(), CryptoStreamMode.Read);
 
            // ��ȣȭ�� ��ȣ �ص��� ���� ����
            switch(Direction)
            {
                case 1 : // ��ȣȭ
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
                case 2 :// ��ȣȭ
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }
 
            return csMyCryptoStream;
        }
 
        private CryptoStream RijndaelCSP(FileStream fsOutput, int Direction)
        {
            RijndaelManaged Provider = new RijndaelManaged();
 
		    CryptoStream csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(), CryptoStreamMode.Read);
 
            // ��ȣȭ�� ��ȣ �ص��� ���� ����
            switch(Direction)
            {
                case 1 : // ��ȣȭ
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
                case 2 :// ��ȣȭ
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }
 
            return csMyCryptoStream;
        }
 
        private CryptoStream TripleDESCSP(FileStream fsOutput, int Direction)
        {
            TripleDESCryptoServiceProvider Provider = new TripleDESCryptoServiceProvider();
            
		    CryptoStream csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(), CryptoStreamMode.Read);
 
            // ��ȣȭ�� ��ȣ �ص��� ���� ����
            switch(Direction)
            {
                case 1 : // ��ȣȭ
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
                case 2 :// ��ȣȭ
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }
 
            return csMyCryptoStream;
        }
 
        private byte[] GetKeyByteArray(string sPassword)
        {
            byte[] byteTemp = new byte[8];
            sPassword = sPassword.PadRight(8);
            byteTemp = System.Text.Encoding.ASCII.GetBytes(sPassword.ToCharArray());
            return byteTemp;
        } 
        #endregion

              
		#region DES�Ϻ�ȣȭ : DES����� Key�� 8Byte�����Դϴ�.
        
 		//-----------------------------------------------------------------------------
		// ��ȣȭ Ű�Դϴ�. �������� ������........DES�� 8Byte ����մϴ�.
		//-----------------------------------------------------------------------------
		private const string desKey = "PORTALCD";

		// Public Function
		public string DESEncrypt(string inStr)
		{
			return DESEncrypt(inStr, desKey);
		}

		//���ڿ� ��ȣȭ(BASE64�ƴ�)
		private string DESEncrypt(string str, string key)
		{
			//Ű ��ȿ�� �˻�
			byte[] btKey = ConvertStringToByteArrayA(key);

			//Ű�� 8Byte�� �ƴϸ� ���ܹ߻�
			if (btKey.Length != 8)
			{
				throw (new Exception("Invalid key. Key length must be 8 byte."));
			}

			//�ҽ� ���ڿ�
			byte[] btSrc = ConvertStringToByteArray(str);
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();

			des.Key = btKey;
			des.IV = btKey;

			ICryptoTransform desencrypt = des.CreateEncryptor();

			MemoryStream ms = new MemoryStream();

			CryptoStream cs = new CryptoStream(ms, desencrypt, CryptoStreamMode.Write);

			cs.Write(btSrc, 0, btSrc.Length);
			cs.FlushFinalBlock();


			byte[] btEncData = ms.ToArray();

			return (ConvertByteArrayToStringB(btEncData));
		}

		// Public Function
		public string DESDecrypt(string inStr) // ��ȣȭ
		{
			return DESDecrypt(inStr, desKey);
		}

		//���ڿ� ��ȣȭ
		private string DESDecrypt(string str, string key)
		{
			if (string.IsNullOrEmpty(str)) return string.Empty;

			//Ű ��ȿ�� �˻�
			byte[] btKey = ConvertStringToByteArrayA(key);

			//Ű�� 8Byte�� �ƴϸ� ���ܹ߻�
			if (btKey.Length != 8)
			{
				throw (new Exception("Invalid key. Key length must be 8 byte."));
			}

			///Ȥ�� ��ȣȭ �ʵ忡 " "�� ������� "+"�� �ٲپ��ش�. 
			str = str.Replace(" ", "+");

			byte[] btEncData = ConvertStringToByteArrayB(str);
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();

			des.Key = btKey;
			des.IV = btKey;

			ICryptoTransform desdecrypt = des.CreateDecryptor();

			MemoryStream ms = new MemoryStream();

			CryptoStream cs = new CryptoStream(ms, desdecrypt, CryptoStreamMode.Write);

			cs.Write(btEncData, 0, btEncData.Length);

			cs.FlushFinalBlock();

			byte[] btSrc = ms.ToArray();


			return (ConvertByteArrayToString(btSrc));
		}
		#endregion	//DES�Ϻ�ȣȭ
        
		#region string�迭 ��ȯ
		//���ڿ�->�����ڵ� ����Ʈ �迭
		public Byte[] ConvertStringToByteArray(String s)
		{
			return (new UnicodeEncoding()).GetBytes(s);
		}

		//�����ڵ� ����Ʈ �迭->���ڿ�
		public string ConvertByteArrayToString(byte[] b)
		{
			return (new UnicodeEncoding()).GetString(b, 0, b.Length);
		}

		//���ڿ�->�Ƚ� ����Ʈ �迭
		public Byte[] ConvertStringToByteArrayA(String s)
		{
			return (new ASCIIEncoding()).GetBytes(s);
		}

		//�Ƚ� ����Ʈ �迭->���ڿ�
		public string ConvertByteArrayToStringA(byte[] b)
		{
			return (new ASCIIEncoding()).GetString(b, 0, b.Length);
		}

		//���ڿ�->Base64 ����Ʈ �迭
		public Byte[] ConvertStringToByteArrayB(String s)
		{
			return Convert.FromBase64String(s);
		}

		//Base64 ����Ʈ �迭->���ڿ�
		public string ConvertByteArrayToStringB(byte[] b)
		{
			return Convert.ToBase64String(b);
		}
		#endregion
    }
 
    /// <summary>
    /// �ؽ� Ŭ������ ����ƽ ����� �ִ�.
    /// </summary>
    public class Hashing
    {
        #region enum, constants and fields
        /// <summary>
        /// ������ �ؽ� ���
        /// </summary>
        public enum HashingTypes
        {
            SHA, SHA256, SHA384, SHA512, MD5
        }
        #endregion
 
        #region static members
        /// <summary>
        /// �Է� �ؽ�Ʈ�� �ؽ� �˰������� ��ȣȭ�Ѵ�(�ؽ� �˰��� MD5)
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        private string Hash(string inputText)
        {
            return ComputeHash(inputText,HashingTypes.MD5);
        }
        
        /// <summary>
        /// �Է� �ؽ�Ʈ�� ����ڰ� ������ �ؽ� �˰������� ��ȣȭ�Ѵ�.
        /// </summary>
        /// <param name="inputText">��ȣȭ�� �ؽ�Ʈ</param>
        /// <param name="hashingType">�ؽ� �˰���</param>
        /// <returns>��ȣȭ�� �ؽ�Ʈ</returns>
        private string Hash(string inputText, HashingTypes hashingType)
        {
            return ComputeHash(inputText,hashingType);
        }
 
        /// <summary>
        /// �Է� �ؽ�Ʈ�� �ؽ��� �ؽ�Ʈ�� ������ ���θ� ���Ѵ�.
        /// </summary>
        /// <param name="inputText">�ؽ����� ���� �Է� �ؽ�Ʈ</param>
        /// <param name="hashText">�ؽ��� �ؽ�Ʈ</param>
        /// <returns>�� ���</returns>
        private bool isHashEqual(string inputText, string hashText)
        {
            return (Hash(inputText) == hashText);
        }
 
        /// <summary>
        /// ����ڰ� ������ �ؽ� �˰������� �Է� �ؽ�Ʈ�� �ؽ��� �ؽ�Ʈ�� ������ ���θ� ���Ѵ�.
        /// </summary>
        /// <param name="inputText">�ؽ����� ���� �Է� �ؽ�Ʈ</param>
        /// <param name="hashText">�ؽ��� �ؽ�Ʈ</param>
        /// <param name="hashingType">����ڰ� ������ �ؽ� Ÿ��</param>
        /// <returns>�� ���</returns>
        private bool isHashEqual(string inputText, string hashText, HashingTypes hashingType)
        {
            return (Hash(inputText,hashingType) == hashText);
        }
        #endregion
 
        #region Hashing Engine
 
        /// <summary>
        /// �ؽ� �ڵ带 ����ؼ� ��Ʈ������ ��ȯ��
        /// </summary>
        /// <param name="inputText">�ؽ��ڵ�� ��ȯ�� ��Ʈ��</param>
        /// <param name="hashingType">����� �ؽ� Ÿ��</param>
        /// <returns>hashed string</returns>
        private string ComputeHash(string inputText, HashingTypes hashingType)
        {
            HashAlgorithm HA = getHashAlgorithm(hashingType);
 
            //declare a new encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //get byte representation of input text
            byte[] inputBytes = UTF8Encoder.GetBytes(inputText);
            
            
            //hash the input byte array
            byte[] output = HA.ComputeHash(inputBytes);
 
            //convert output byte array to a string
            return Convert.ToBase64String(output);
        }
 
        /// <summary>
        /// Ư���� �ؽ� �˰����� ������
        /// </summary>
        /// <param name="hashingType">����� �ؽ� �˰���</param>
        /// <returns>HashAlgorithm</returns>
        private HashAlgorithm getHashAlgorithm(HashingTypes hashingType)
        {
            switch (hashingType)
            {
                case HashingTypes.MD5 :
                    return new MD5CryptoServiceProvider();
                case HashingTypes.SHA :
                    return new SHA1CryptoServiceProvider();
                case HashingTypes.SHA256 :
                    return new SHA256Managed();
                case HashingTypes.SHA384 :
                    return new SHA384Managed();
                case HashingTypes.SHA512 :
                    return new SHA512Managed();
                default :
                    return new MD5CryptoServiceProvider();
            }
        }
        #endregion


        private static readonly string CryptoKey = ConfigurationManager.AppSettings["Enkey"];

        /// <summary>
        /// ��ȣȭ �޼���.
        /// </summary>
        /// <param name="InputText"></param>
        /// <returns>��ȣȭ�� ���ڿ� ��ȯ</returns>
        static public string EncryptoString(string InputText)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);

            byte[] Salt = Encoding.ASCII.GetBytes(CryptoKey.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(CryptoKey, Salt);

            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(PlainText, 0, PlainText.Length);

            cryptoStream.FlushFinalBlock();

            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Clear();

            string EcnryptedDate = Convert.ToBase64String(CipherBytes);

            return EcnryptedDate;
        }

        /// <summary>
        /// ��ȣȭ �޼���
        /// </summary>
        /// <param name="InputText">��ȣȭ�� ���ڿ�</param>
        /// <returns>��ȣȭ�� ���ڿ� ��ȯ</returns>
        static public string DecryptString(string InputText)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] EncryptedDate = Convert.FromBase64String(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(CryptoKey.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(CryptoKey, Salt);

            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream(EncryptedDate);

            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

            byte[] PlainText = new byte[EncryptedDate.Length];

            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

            return DecryptedData;
        }
    }
}

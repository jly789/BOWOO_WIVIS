////*********************************************************
////원본출처 : 훈스닷넷(http://www.hoonsbara.com/hoonsboard.aspx?table_name=cshaptip&board_idx=445778&page=1&keyword=&search=&boardmode=2)
////*********************************************************

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace lib.Common.Crypto
{

    /// <summary>
    /// 스트링/파일 암/복호화 클래스(비대칭 알고리즘 사용)
    /// </summary>
    public class CryptoEx
    {
        #region enums, constants & fields

        /// <summary>
        /// 비대칭 암/복호화 알고리즘 상수
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
        /// 디폴트 생성자
        /// </summary>
        public CryptoEx()
        {
            calculateNewKeyAndIV();
        }


        /// <summary>
        /// 생성자(암/복호화 타입을 지정)
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
        /// 텍스트를 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <returns>암호화된 텍스트</returns>
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
        /// 사용자가 지정한 패스워드로 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <param name="password">암호화에 사용할 패스워드</param>
        /// <returns>암호화된 텍스트</returns>
        public string Encrypt(string inputText, string password)
        {
            this.mPassword = password;
            return this.Encrypt(inputText);
        }

 

        /// <summary>
        /// 사용자가 지정한 cryptoType과 패스워드로 텍스트를 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <param name="password">암호화에 사용할 패스워드</param>
        /// <param name="cryptoType">암호화 타입</param>
        /// <returns>암호화된 텍스트</returns>
        public string Encrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            this.mCryptoType = cryptoType;
            return this.Encrypt(inputText,password);
        }

        /// <summary>
        /// 사용자가 지정한 cryptoType으로 텍스트를 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <param name="cryptoType">암호화 타입</param>
        /// <returns>암호화된 텍스트</returns>
        public string Encrypt(string inputText, CryptoTypes cryptoType)
        {
            this.mCryptoType = cryptoType;
            return this.Encrypt(inputText);
        }
 
        #endregion
 
        #region Decryption
        /// <summary>
        /// 텍스트를 복호화한다.
        /// </summary>
        /// <param name="inputText">복호화할 텍스트</param>
        /// <returns>복호화된 텍스트</returns>
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
        /// 사용자가 지정한 패스워드키에 의해 텍스트를 복호화한다.
        /// </summary>
        /// <param name="inputText">복호화할 텍스트</param>
        /// <param name="password">복호화할 때 사용할 패스워드</param>
        /// <returns>복호화된 텍스트</returns>
        public string Decrypt(string inputText, string password)
        {
            this.mPassword = password;
            return Decrypt(inputText);
        }

 
        /// <summary>
        /// 사용자가 지정한 cryptoType과 패스워드로 텍스트를 복호화한다.
        /// </summary>
        /// <param name="inputText">복호화할 텍스트</param>
        /// <param name="password">복호화에 사용할 패스워드</param>
        /// <param name="cryptoType">복호화 타입</param>
        /// <returns>복호화된 텍스트</returns>
        public string Decrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            this.mCryptoType = cryptoType;
            return Decrypt(inputText,password);
        }

        /// <summary>
        /// 사용자가 지정한 cryptoType으로 텍스트를 복호화한다.
        /// </summary>
        /// <param name="inputText">복호화할 텍스트</param>
        /// <param name="cryptoType">복호화 타입</param>
        /// <returns>복호화된 텍스트</returns>
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
    		//// 새로운 메서드로 대체되었다.
    		//// Dim pdb As PasswordDeriveBytes = New PasswordDeriveBytes(mPassword, SaltByteArray)
    		Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(mPassword, SaltByteArray);            
            
    		SymmetricAlgorithm algo = selectAlgorithm();
    		mKey = pdb.GetBytes(Convert.ToInt32(algo.KeySize / 8));
    		mIV = pdb.GetBytes(Convert.ToInt32(algo.BlockSize / 8));
        }
 
        #endregion
 
        #region File Encryption/Decryption
        /// <summary>
        /// 암호화 처리 함수(DES 알고리즘)
        /// </summary>
        /// <param name="sInputFile">입력 파일</param>
        /// <param name="sOutputFile">출력 파일</param>
        public void EncryptFile(string sInputFile, string sOutputFile)
        {
            EncryptOrDecryptFile(sInputFile, sOutputFile, 1);
        }
 
        /// <summary>
        /// 복호화 처리 함수(DES 알고리즘)
        /// </summary>
        /// <param name="sInputFile">입력 파일</param>
        /// <param name="sOutputFile">출력 파일</param>
        public void DecryptFile(string sInputFile, string sOutputFile)
        {
            EncryptOrDecryptFile(sInputFile, sOutputFile, 2);
        }
 
        /// <summary>
        /// 암호화/복호화 처리 함수(DES 알고리즘)
        /// </summary>
        /// <param name="sInputFile">입력 파일</param>
        /// <param name="sOutputFile">출력 파일</param>
        /// <param name="Direction">암호화(1)/복호화(2) 여부</param>
        private void EncryptOrDecryptFile(string sInputFile, string sOutputFile, int Direction)
        {
            //          byte[] byteDESKey = GetKeyByteArray(password);
            //          byte[] byteDESIV = GetKeyByteArray(ivpassword);
 
            // 파일 스트림을 만들어 입력 및 출력 파일을 처리합니다.
            FileStream fsInput = new FileStream(sInputFile, FileMode.Open, FileAccess.Read);
            FileStream fsOutput = new FileStream(sOutputFile, FileMode.OpenOrCreate, FileAccess.Write);
            fsOutput.SetLength(0);
 
            // 암호화/암호 해독 프로세스 중 필요한 변수입니다.
            byte[] byteBuffer = new byte[4097]; // 처리를 위해 바이트 블록을 보유합니다.
            long nBytesProcessed = 0; // 암호화된 바이트의 실행 카운트
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
 
            // 입력 파일에서 읽은 다음 암호화하거나 암호를 해독하고
            // 출력 파일에 씁니다.
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
 
            // 암호화나 암호 해독을 위한 설정
            switch(Direction)
            {
                case 1 : // 암호화
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
                case 2 :// 복호화
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }
 
            return csMyCryptoStream;
        }
 
        private CryptoStream RC2CSP(FileStream fsOutput, int Direction)
        {
            RC2CryptoServiceProvider Provider = new RC2CryptoServiceProvider();
 		    CryptoStream csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(), CryptoStreamMode.Read);
 
            // 암호화나 암호 해독을 위한 설정
            switch(Direction)
            {
                case 1 : // 암호화
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
                case 2 :// 복호화
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }
 
            return csMyCryptoStream;
        }
 
        private CryptoStream RijndaelCSP(FileStream fsOutput, int Direction)
        {
            RijndaelManaged Provider = new RijndaelManaged();
 
		    CryptoStream csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(), CryptoStreamMode.Read);
 
            // 암호화나 암호 해독을 위한 설정
            switch(Direction)
            {
                case 1 : // 암호화
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
                case 2 :// 복호화
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }
 
            return csMyCryptoStream;
        }
 
        private CryptoStream TripleDESCSP(FileStream fsOutput, int Direction)
        {
            TripleDESCryptoServiceProvider Provider = new TripleDESCryptoServiceProvider();
            
		    CryptoStream csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(), CryptoStreamMode.Read);
 
            // 암호화나 암호 해독을 위한 설정
            switch(Direction)
            {
                case 1 : // 암호화
                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
                case 2 :// 복호화
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

              
		#region DES암복호화 : DES방식의 Key는 8Byte고정입니다.
        
 		//-----------------------------------------------------------------------------
		// 암호화 키입니다. 변경하지 마세요........DES는 8Byte 사용합니다.
		//-----------------------------------------------------------------------------
		private const string desKey = "PORTALCD";

		// Public Function
		public string DESEncrypt(string inStr)
		{
			return DESEncrypt(inStr, desKey);
		}

		//문자열 암호화(BASE64아님)
		private string DESEncrypt(string str, string key)
		{
			//키 유효성 검사
			byte[] btKey = ConvertStringToByteArrayA(key);

			//키가 8Byte가 아니면 예외발생
			if (btKey.Length != 8)
			{
				throw (new Exception("Invalid key. Key length must be 8 byte."));
			}

			//소스 문자열
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
		public string DESDecrypt(string inStr) // 복호화
		{
			return DESDecrypt(inStr, desKey);
		}

		//문자열 복호화
		private string DESDecrypt(string str, string key)
		{
			if (string.IsNullOrEmpty(str)) return string.Empty;

			//키 유효성 검사
			byte[] btKey = ConvertStringToByteArrayA(key);

			//키가 8Byte가 아니면 예외발생
			if (btKey.Length != 8)
			{
				throw (new Exception("Invalid key. Key length must be 8 byte."));
			}

			///혹시 암호화 필드에 " "이 있을경우 "+"로 바꾸어준다. 
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
		#endregion	//DES암복호화
        
		#region string배열 변환
		//문자열->유니코드 바이트 배열
		public Byte[] ConvertStringToByteArray(String s)
		{
			return (new UnicodeEncoding()).GetBytes(s);
		}

		//유니코드 바이트 배열->문자열
		public string ConvertByteArrayToString(byte[] b)
		{
			return (new UnicodeEncoding()).GetString(b, 0, b.Length);
		}

		//문자열->안시 바이트 배열
		public Byte[] ConvertStringToByteArrayA(String s)
		{
			return (new ASCIIEncoding()).GetBytes(s);
		}

		//안시 바이트 배열->문자열
		public string ConvertByteArrayToStringA(byte[] b)
		{
			return (new ASCIIEncoding()).GetString(b, 0, b.Length);
		}

		//문자열->Base64 바이트 배열
		public Byte[] ConvertStringToByteArrayB(String s)
		{
			return Convert.FromBase64String(s);
		}

		//Base64 바이트 배열->문자열
		public string ConvertByteArrayToStringB(byte[] b)
		{
			return Convert.ToBase64String(b);
		}
		#endregion
    }
 
    /// <summary>
    /// 해슁 클래스로 스태틱 멤버만 있다.
    /// </summary>
    public class Hashing
    {
        #region enum, constants and fields
        /// <summary>
        /// 가능한 해슁 멤버
        /// </summary>
        public enum HashingTypes
        {
            SHA, SHA256, SHA384, SHA512, MD5
        }
        #endregion
 
        #region static members
        /// <summary>
        /// 입력 텍스트를 해슁 알고리즘으로 암호화한다(해슁 알고리즘 MD5)
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <returns>암호화된 텍스트</returns>
        private string Hash(string inputText)
        {
            return ComputeHash(inputText,HashingTypes.MD5);
        }
        
        /// <summary>
        /// 입력 텍스트를 사용자가 지정한 해슁 알고리즘으로 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <param name="hashingType">해슁 알고리즘</param>
        /// <returns>암호화된 텍스트</returns>
        private string Hash(string inputText, HashingTypes hashingType)
        {
            return ComputeHash(inputText,hashingType);
        }
 
        /// <summary>
        /// 입력 텍스트와 해쉬된 텍스트가 같은지 여부를 비교한다.
        /// </summary>
        /// <param name="inputText">해쉬되지 않은 입력 텍스트</param>
        /// <param name="hashText">해쉬된 텍스트</param>
        /// <returns>비교 결과</returns>
        private bool isHashEqual(string inputText, string hashText)
        {
            return (Hash(inputText) == hashText);
        }
 
        /// <summary>
        /// 사용자가 지정한 해쉬 알고리즘으로 입력 텍스트와 해쉬된 텍스트가 같은지 여부를 비교한다.
        /// </summary>
        /// <param name="inputText">해쉬되지 않은 입력 텍스트</param>
        /// <param name="hashText">해쉬된 텍스트</param>
        /// <param name="hashingType">사용자가 지정한 해슁 타입</param>
        /// <returns>비교 결과</returns>
        private bool isHashEqual(string inputText, string hashText, HashingTypes hashingType)
        {
            return (Hash(inputText,hashingType) == hashText);
        }
        #endregion
 
        #region Hashing Engine
 
        /// <summary>
        /// 해쉬 코드를 계산해서 스트링으로 변환함
        /// </summary>
        /// <param name="inputText">해쉬코드로 변환할 스트링</param>
        /// <param name="hashingType">사용할 해슁 타입</param>
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
        /// 특정한 해슁 알고리즘을 리턴함
        /// </summary>
        /// <param name="hashingType">사용할 해슁 알고리즘</param>
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
        /// 암호화 메서드.
        /// </summary>
        /// <param name="InputText"></param>
        /// <returns>암호화된 문자열 반환</returns>
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
        /// 복호화 메서드
        /// </summary>
        /// <param name="InputText">암호화된 문자열</param>
        /// <returns>복호화된 문자열 반환</returns>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace lib.Common
{
    public class FtpClient
    {
        #region :Field/Const

        private string _server;
        private string _userid;
        private string _password;

        //private static AppLog _log;

        #endregion

        #region :Property

        /// <summary>
        /// FTP Host 정보입니다. 
        /// 프로토콜 형식을 제외한 IP나 호스트 명으로 설정해야 합니다.
        /// </summary>
        public string Server
        {
            get { return _server; }
            set
            {
                _server = value;
            }
        }

        public string UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string ConnectInfo
        {
            get { return string.Format(@"ftp://{1}:{2}@{0}", Server, UserID, Password); }
        }

        #endregion

        #region :Constructor

        static FtpClient()
        {
            //_log = new AppLog();
        }

        public FtpClient()
        {
            //_log.Write("FtpClient가 생성되었습니다.");
        }

        public FtpClient(string server)
        {
            _server = server;

            //_log.Write(string.Format("FtpClient가 {0} 서버에 생성되었습니다.", server));
        }

        public FtpClient(string server, string userid, string password)
        {
            _server = server;
            _userid = userid;
            _password = password;

            //_log.Write(string.Format("FtpClient가 {0} 서버에 {1}/{2} 계정으로 연결되었습니다.", server, userid, password));
        }

        #endregion

        #region :Public Method

        /// <summary>
        /// 로컬 경로의 파일을 원격 경로로 업로드 합니다.
        /// </summary>
        /// <param name="LocalPath"></param>
        /// <param name="RemotePath"></param>
        public bool Upload(string LocalPath, string RemotePath)
        {
            bool rtnBool = false;
            Stream requestStream = null;
            FileStream fileStream = null;
            FtpWebResponse uploadResponse = null;
            try
            {
                String uploadDir = RemotePath.Substring(0, RemotePath.LastIndexOf("/") + 1);
                if (IsExistDirOnServer(uploadDir) == false) MkDirOnServer(uploadDir);
                FtpWebRequest uploadRequest =
                    (FtpWebRequest)WebRequest.Create(ConnectInfo + RemotePath);

                uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;

                // UploadFile is not supported through an Http proxy
                // so we disable the proxy for this request.
                uploadRequest.Proxy = null;

                requestStream = uploadRequest.GetRequestStream();
                fileStream = File.Open(LocalPath, FileMode.Open);

                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    requestStream.Write(buffer, 0, bytesRead);
                }

                // The request stream must be closed before getting 
                // the response.
                requestStream.Close();

                uploadResponse =
                    (FtpWebResponse)uploadRequest.GetResponse();

                //_log.Write(string.Format("{0} 경로로 업로드 되었습니다.", RemotePath));
                rtnBool = true;
            }
            catch (UriFormatException)
            {
                //_log.Write(ex);
                rtnBool = false;
            }
            catch (WebException)
            {
                //_log.Write(ex);
                rtnBool = false;
            }
            catch (IOException)
            {
                //_log.Write(ex);
                rtnBool = false;
            }
            finally
            {
                if (uploadResponse != null)
                    uploadResponse.Close();
                if (fileStream != null)
                    fileStream.Close();
                if (requestStream != null)
                    requestStream.Close();
            }
            return rtnBool;

        }

        /// <summary>
        /// 원격 경로의 파일을 로컬 경로로 다운로드 합니다.
        /// </summary>
        /// <param name="RemotePath"></param>
        /// <param name="LocalPath"></param>
        public bool Download(string RemotePath, string LocalPath, bool useBinary)
        {
            bool rtnBool = false;
            Stream responseStream = null;
            FileStream fileStream = null;
            StreamReader reader = null;

            try
            {
                FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(ConnectInfo + RemotePath);
                downloadRequest.UseBinary = useBinary;

                FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                responseStream = downloadResponse.GetResponseStream();

                string fileName = Path.GetFileName(downloadRequest.RequestUri.AbsolutePath);

                if (fileName.Length == 0)
                {
                    reader = new StreamReader(responseStream);
                }
                else
                {
                    fileStream = File.Create(LocalPath);
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while (true)
                    {
                        bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                            break;
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                }

                rtnBool = true;
            }
            catch (UriFormatException)
            {
                //_log.Write(ex);
                rtnBool = false;
            }
            catch (WebException)
            {
                //_log.Write(ex);
                rtnBool = false;
            }
            catch (IOException)
            {
                //_log.Write(ex);
                rtnBool = false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                else if (responseStream != null)
                    responseStream.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
            return rtnBool;
        }

        /// <summary>
        /// ftp 서버에 있는 파일을 삭제한다.
        /// </summary>
        /// <param name="RemotePath"></param>
        /// <returns></returns>
        public bool DeleteFileOnServer(String RemotePath)
        {
            bool rtnBool = false;
            try
            {
                RemotePath = ConnectInfo + RemotePath;
                Uri remoteUri = new Uri(RemotePath);

                if (remoteUri.Scheme != Uri.UriSchemeFtp)
                {
                    rtnBool = false;
                }
                else
                {
                    // Get the object used to communicate with the server.
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteUri);
                    request.Method = WebRequestMethods.Ftp.DeleteFile;

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    System.Diagnostics.Trace.WriteLine("Delete status: {0}", response.StatusDescription);
                    response.Close();
                    rtnBool = true;
                }
            }
            catch (Exception)
            {
                rtnBool = false;
                //_log.Write(ex);
            }
            return rtnBool;
        }

        /// <summary>
        /// FTP 서버에 있는 디렉토리를 삭제한다.RemotePath는 '/aaa/bbb' 형태이다.
        /// </summary>
        /// <param name="RemotePath"></param>
        /// <returns></returns>
        public bool RemoveDirOnServer(String RemotePath)
        {
            bool rtnBool = false;
            try
            {
                Uri remoteUri = new Uri(ConnectInfo + RemotePath);

                if (remoteUri.Scheme != Uri.UriSchemeFtp)
                {
                    rtnBool = false;
                }
                else
                {
                    List<string> files = new List<string>();
                    files = ListDirectory(RemotePath);
                    foreach (string file in files)
                    {
                        DeleteFileOnServer(RemotePath + "/" + file);
                    }
                    // Get the object used to communicate with the server.
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteUri);
                    request.Method = WebRequestMethods.Ftp.RemoveDirectory;

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    System.Diagnostics.Trace.WriteLine("RemoveDirectory status: {0}", response.StatusDescription);
                    response.Close();
                    rtnBool = true;
                }
            }
            catch (Exception)
            {
                rtnBool = false;
                //_log.Write(ex);
            }
            return rtnBool;
        }

        /// <summary>
        /// FTP 서버에 디렉토리를 생성한다. RemotePath는 '/aaa/bbb' 형태이다.
        /// </summary>
        /// <param name="RemotePath"></param>
        /// <returns></returns>
        public bool MkDirOnServer(String RemotePath)
        {
            bool rtnBool = false;
            try
            {
                string[] dirs = RemotePath.Split("/".ToCharArray());
                string strDir = "";
                foreach (string dir in dirs)
                {
                    if (dir != null && dir != "")
                    {
                        strDir = strDir + "/" + dir;
                        Uri remoteUri = new Uri(ConnectInfo + strDir);
                        if (IsExistDirOnServer(strDir) == false)
                        {
                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteUri);
                            request.Method = WebRequestMethods.Ftp.MakeDirectory;

                            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                            System.Diagnostics.Trace.WriteLine("MakeDirectory status: {0}", response.StatusDescription);
                            response.Close();
                            rtnBool = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                rtnBool = false;
                //_log.Write(ex);
            }
            return rtnBool;
        }

        /// <summary>
        /// FTP 서버에 디렉토리가 있는지 없는지 판단한다.remotePath는 '/aaa/bbb' 형태이다.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <returns></returns>
        public bool IsExistDirOnServer(string remotePath)
        {
            bool returnValue = false;
            try
            {
                remotePath = ConnectInfo + remotePath;
                Uri remoteUri = new Uri(remotePath);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remoteUri);
                //request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
                returnValue = true;
            }
            catch (WebException)
            {
                returnValue = false;
            }
            catch (Exception)
            {
                returnValue = false;
                //_log.Write(ex);
            }
            return returnValue;
        }

        public List<string> ListDirectory(string remotePath)
        {
            return ListDirectory(remotePath, "*.*");
        }


        /// <summary>
        /// FTP서버에 있는 파일목록을 가져온다.remotePath는 '/aaa/bbb' 형태이고 filter는 '*.txt'형태이다.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<string> ListDirectory(string remotePath, string filter)
        {

            List<string> files = new List<string>();
            string line = "";
            remotePath = ConnectInfo + remotePath + "/" + filter;
            StreamReader reader = null; ;

            try
            {
                //Create a request for directory listing
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(remotePath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                //Get a reference to the response stream
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());

                while ((line = reader.ReadLine()) != null)
                {
                    files.Add(line);
                }
            }
            catch (WebException)
            {
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            //Return the list of files
            return files;
        }
        #endregion
    }
}

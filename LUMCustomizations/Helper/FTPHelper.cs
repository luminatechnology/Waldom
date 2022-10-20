using LUMCustomizations.Helper.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LUMCustomizations.Helper
{
    public class FTPHelper
    {
        protected IFTPConfig config;
        public FTPHelper(IFTPConfig _config)
            => this.config = _config;

        /// <summary> Upload File by FTP </summary>
        public bool UploadFileToFTP(byte[] data, string uploadPath, string fileName)
        {
            string url = $"ftp://{config.FtpHost}:{config.FtpPort}{uploadPath}{fileName}";
            var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
            // setting Method
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            // setting upload type
            ftpRequest.UseBinary = true;
            // setting user & PW
            ftpRequest.Credentials = new NetworkCredential(config.FtpUserName, config.FtpPassword);
            // setting keepAlive
            ftpRequest.KeepAlive = false;
            // setting passive
            ftpRequest.UsePassive = true;
            // setting requestStream
            var reqStream = ftpRequest.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            reqStream.Dispose();
            // Upload File
            var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            var IsSuccess = ftpResponse.StatusCode == FtpStatusCode.ClosingData;
            // Release
            ftpResponse.Close();
            ftpRequest.Abort();
            return IsSuccess;
        }
    }
}

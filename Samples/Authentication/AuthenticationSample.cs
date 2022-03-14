using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Izibiz;
using Izibiz.Adapter;
using Izibiz.AuthenticationWS;
using Izibiz.Operations;
using NUnit.Framework;

namespace Samples.Authentication
{
    
    class AuthenticationSample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();

       [Test,Order(1)]
        public void Loginn()
        {     
            var request = new LoginRequest
            {
                
                REQUEST_HEADER = new REQUEST_HEADERType
                {
                   SESSION_ID = "-1",
                    APPLICATION_NAME = "Izibiz_dotnet_soap_client.Application"              
                },
                USER_NAME = "KULLANICI ADINI GİRİNİZ...",
                PASSWORD = "ŞİFRE GİRİNİZ..."

            };

            LoginResponse response = _izibizClient.Auth().Login(request);
            Assert.NotNull(response.SESSION_ID);
            

        }

        [Test,Order(2)]
        public void CheckUserInvoice()
        {   //e-fatura mükellefi mi kontrol etme 
            var request = new CheckUserRequest
            {

                REQUEST_HEADER = new REQUEST_HEADERType
                {
                    SESSION_ID = BaseAdapter.SessionId,
                    APPLICATION_NAME = "Izibiz_dotnet_soap_client.Application"
                },
                USER = new GIBUSER
                {
                    IDENTIFIER = "4840847211"
                },
                 DOCUMENT_TYPE="INVOICE"
                 };

            CheckUserResponse response = _izibizClient.Auth().CheckUserInvoice(request);
            Assert.AreEqual(response.USER.Length, 7);
        }

       [Test, Order(3)]
          public void CheckUserDespatchadvice()
          {   //e-irsaliye mükellefi mi kontrol etme 
              var request = new CheckUserRequest
              {

                  REQUEST_HEADER = new REQUEST_HEADERType
                  {
                      SESSION_ID = BaseAdapter.SessionId,
                      APPLICATION_NAME = "Izibiz_dotnet_soap_client.Application"
                  },
                  USER = new GIBUSER
                  {
                      IDENTIFIER = "4840847211"
                  },
                  DOCUMENT_TYPE = "DESPATCHADVICE"
              };

              CheckUserResponse response = _izibizClient.Auth().CheckUserDespatchadvice(request);
              Assert.AreEqual(response.USER.Length, 6);
          }

        [Test, Order(4)]
        public void GetGibUserList()
        {
            var request = new GetGibUserListRequest
            {

                REQUEST_HEADER = new REQUEST_HEADERType
                {
                    SESSION_ID = BaseAdapter.SessionId,
                    APPLICATION_NAME = "Izibiz_dotnet_soap_client.Application"
                },
                TYPE = "XML",
                DOCUMENT_TYPE = "DESPATCHADVICE",
                REGISTER_TIME_START = DateTime.Now,
                ALIAS_TYPE = ALIAS_TYPE.GB,
                ALIAS_TYPESpecified=true,
            };

            GetGibUserListResponse response = _izibizClient.Auth().GetGibUserList(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.NotNull(response.CONTENT);
            Assert.NotNull(response.CONTENT.Value);
            Assert.IsTrue(response.CONTENT.Value.Length > 0);
            byte[] gibUserList= Compress.ZipFile(response.CONTENT.Value);           
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\GibUserList.xml", gibUserList);
        }

    //    [Test,Order(5)]
        public void Logout()
        {
            var request = new LogoutRequest
            {

                REQUEST_HEADER = new REQUEST_HEADERType
                {
                    SESSION_ID = BaseAdapter.SessionId,
                    APPLICATION_NAME = "Izibiz_dotnet_soap_client.Application"
                }
                
            };
            LogoutResponse response = _izibizClient.Auth().Logout(request);       
            Assert.Null(response.ERROR_TYPE);
            
        }

    }
}

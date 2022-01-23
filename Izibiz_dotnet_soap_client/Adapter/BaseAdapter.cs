using Izibiz_dotnet_soap_client.AuthenticationWS;
using System;
using System.Collections.Generic;
using Izibiz_dotnet_soap_client.EArchiveInvoiceWS;
using Izibiz_dotnet_soap_client.EIrsaliyeWS;

namespace Izibiz_dotnet_soap_client.Adapter
{
    public static class BaseAdapter
    {
        public const string UUIDArchive = "0055215d-f3c1-46b1-bc91-dbbb58e495ea";
        private static String _sessionId;
        public static EInvoiceWS.INVOICE[] invoiceToMarkInvoice;//GetInvoice ile çekilen faturalarý markinvoice ile alýndý alýnmadý olarak iþaretlemek için

        public static String SessionId
        {
            get
            {
                if (_sessionId == null)
                {
                    AuthenticationServicePortClient authenticationPortClient = new AuthenticationServicePortClient();
                    LoginRequest loginRequest = new LoginRequest();
                    loginRequest.USER_NAME = "izibiz-test2";
                    loginRequest.PASSWORD = "izi321";
                    LoginResponse loginResponse = authenticationPortClient.Login(loginRequest);
                    _sessionId = loginResponse.SESSION_ID;
                }
                return _sessionId;
            }
        }

        public static EArchiveInvoiceWS.REQUEST_HEADERType EArchiveRequestHeaderType()
        {
            return new EArchiveInvoiceWS.REQUEST_HEADERType
            {
                SESSION_ID = BaseAdapter.SessionId,
                COMPRESSED = nameof(EI.YesNo.Y),
                APPLICATION_NAME = "izibiz.Application"
            };
        }

        public static AuthenticationWS.REQUEST_HEADERType AuntWSRequestHeaderType()
        {
            return new AuthenticationWS.REQUEST_HEADERType
            {
                SESSION_ID = BaseAdapter.SessionId,
                COMPRESSED = nameof(EI.YesNo.Y),
                APPLICATION_NAME = "izibiz.Application"
            };
        }

        public static EInvoiceWS.REQUEST_HEADERType EInvoiceWSRequestHeaderType()
        {
            return new EInvoiceWS.REQUEST_HEADERType
            {
                SESSION_ID = BaseAdapter.SessionId,
                COMPRESSED = nameof(EI.YesNo.Y),
                APPLICATION_NAME = "izibiz.Application"

            };
        }

        public static EIrsaliyeWS.REQUEST_HEADERType EDespatchWSRequestHeaderType()
        {
            return new EIrsaliyeWS.REQUEST_HEADERType
            {
                SESSION_ID = BaseAdapter.SessionId,
                COMPRESSED = nameof(EI.YesNo.Y),
                APPLICATION_NAME = "izibiz.Application"
            };
        }

        public static CreditNoteWS.REQUEST_HEADERType CreditNoteWSRequestHeaderType()
        {
            return new CreditNoteWS.REQUEST_HEADERType
            {
                SESSION_ID = BaseAdapter.SessionId,
                COMPRESSED = nameof(EI.YesNo.Y),
                APPLICATION_NAME = "izibiz.Application"
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Izibiz;
using Izibiz.Adapter;
using NUnit.Framework;
using Izibiz.UblSerializer;
using Izibiz.UblCreate;
using Izibiz.Operations;
using Izibiz.CreditNoteWS;
using UblCreditNote;
using System.Xml;

namespace Samples.CreditNote
{
  //  [Ignore("Waiting for Joe to fix his bugs", Until = "2022-07-31 12:00:00Z")]
    class CreditNote_Sample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();
        public static List<string> uuidList = new List<string>();
        public static List<string> uuidListMarkToGet = new List<string>();

        CreditNoteUBL creditNote = new CreditNoteUBL();
        CreditNoteType creditNoteType;
        [Test,Order(2)]
        public void SendCreditNote()
        {
            creditNoteType = creditNote.baseCreditNoteUBL;

            String xmlString = null;

            xmlString = XmlSerializerr.XmlSerializeCreditNote(creditNoteType);

            byte[] zipFile = Compress.compressFile(xmlString);


            base64Binary base64binary = new base64Binary();
            base64binary.contentType = nameof(EI.DocumentType.XML);
            base64binary.Value = zipFile;
            var request = new SendCreditNoteRequest
            {
                REQUEST_HEADER = BaseAdapter.CreditNoteWSRequestHeaderType(),
                CREDITNOTE = new CREDITNOTE[]
                {
                    new CREDITNOTE
                    {
                    ID= creditNoteType.ID.Value,
                    UUID= creditNoteType.UUID.Value,
                    CONTENT=base64binary
                    }
                },
                CREDITNOTE_PROPERTIES = new CREDITNOTE_PROPERTIES
                {
                    EMAIL_FLAG = FLAG_VALUE.Y,
                    EMAIL = new string[] { "b@gmail.com" },
                    SENDING_TYPE = SENDING_TYPE.ELEKTRONIK,
                }
            };
            SendCreditNoteResponse response = _izibizClient.CreditNote().SendCreditNoteResponse(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            Assert.Null(response.ERROR_TYPE);
            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                FileOperations.SendAndLoadSaveToDisk(nameof(EI.FileName.CREDITNOTESEND), creditNoteType.ID.Value, creditNoteType.UUID.Value, zipFile, nameof(EI.Type.CREDITNOTE));
                uuidList.Add(creditNoteType.UUID.Value);
            }
          
        }

        [Test,Order(3)]
        public void GetCreditNote_BetweenDate()
        {
            var request = new GetCreditNoteRequest
            {

                REQUEST_HEADER = BaseAdapter.CreditNoteWSRequestHeaderType(),
                CREDITNOTE_SEARCH_KEY = new GetCreditNoteRequestCREDITNOTE_SEARCH_KEY
                {
                    LIMIT = 10,
                    LIMITSpecified = true,
                    START_DATE = Convert.ToDateTime("2021-12-17"),
                    START_DATESpecified = true,
                    END_DATE = Convert.ToDateTime("2022-01-17"),
                    END_DATESpecified = true,
                    READ_INCLUDED = FLAG_VALUE.Y,//Daha önce okunmamış olan belgeleri getirmesi için N verilmeli Eğer okunmuş belgelerin dahil edilmesi isteniyorsa Y gönderilmeli.
                    READ_INCLUDEDSpecified = true,

                },
                HEADER_ONLY = FLAG_VALUE.N,
                CONTENT_TYPE = CONTENT_TYPE.PDF,
            };
            GetCreditNoteResponse response = _izibizClient.CreditNote().GetCreditNoteResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.NotNull(response.CREDITNOTE);
            Assert.IsTrue(response.CREDITNOTE.Length > 0);
            if (request.HEADER_ONLY.ToString() == nameof(FLAG_VALUE.N))
            {
                foreach (CREDITNOTE cnote in response.CREDITNOTE)
                {
                    FileOperations.SaveToDisk(cnote.CONTENT.Value, cnote.UUID, "", request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.CREDITNOTE), request.CONTENT_TYPE.ToString(), cnote.ID);
                    uuidListMarkToGet.Add(cnote.UUID);
                }
            }

            foreach (CREDITNOTE cnote in response.CREDITNOTE)
            {
                uuidList.Add(cnote.UUID);
            }
        }


        [Test,Order(4)]
        public void GetCreditNoteStatus()
        {
            var request = new GetCreditNoteStatusRequest
            {

                REQUEST_HEADER = BaseAdapter.CreditNoteWSRequestHeaderType(),
                //UUID = new string[] { "06727BF4-B521-4C56-82F8-43368E19C8D0" }//Statusunu sorgulamak istenilen mühtahsilin UUID'sini yandaki şekilde yazılarak sorgulama yapılır. 
                UUID = uuidList.ToArray(),
            };
            GetCreditNoteStatusResponse response = _izibizClient.CreditNote().GetGetCreditNoteStatusResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.IsTrue(response.CREDITNOTE_STATUS.Length > 0);
        }

        [Test,Order(1)]
        public void LoadCreditNote()
        {

            creditNoteType = creditNote.baseCreditNoteUBL;

            String xmlString = null;

            xmlString = XmlSerializerr.XmlSerializeCreditNote(creditNoteType);

            byte[] zipFile = Compress.compressFile(xmlString);

            base64Binary base64binary = new base64Binary();
            base64binary.contentType = nameof(EI.DocumentType.XML);
            base64binary.Value = zipFile;

            var request = new LoadCreditNoteRequest
            {
                REQUEST_HEADER = BaseAdapter.CreditNoteWSRequestHeaderType(),
                CREDITNOTE = new CREDITNOTE[1]
                {
                    new CREDITNOTE
                    {
                    ID=creditNoteType.ID.Value,
                    UUID=creditNoteType.UUID.Value,
                    CONTENT =base64binary,

                    },
                },
                CREDITNOTE_PROPERTIES = new CREDITNOTE_PROPERTIES { },
            };

            LoadCreditNoteResponse response = _izibizClient.CreditNote().LoadCreditNoteResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);

            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                FileOperations.SendAndLoadSaveToDisk(nameof(EI.FileName.CREDITNOTELOAD), creditNoteType.ID.Value, creditNoteType.UUID.Value, zipFile,nameof(EI.Type.CREDITNOTE));
            }
            
        }

          [Test,Order(5)]
        public void MarkCreditNote_Read()
        {
            var request = new MarkCreditNoteRequest
            {
                REQUEST_HEADER = new REQUEST_HEADERType
                {
                    SESSION_ID = BaseAdapter.SessionId,
                },
                MARK = new MarkCreditNoteRequestMARK
                {
                    value = nameof(EI.DocumentStatus.READ),
                    UUID = uuidListMarkToGet.ToArray(),//getCreditNote ile çekilen müstahsilleri okundu işaretler
                }
            };

            MarkCreditNoteResponse response = _izibizClient.CreditNote().MarkCreditNoteResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            GetCreditNote_BetweenDate();

        }


       [Test,Order(6)]
        public void MarkCreditNote_Unread()
        {
            var request = new MarkCreditNoteRequest
            {
                REQUEST_HEADER = new REQUEST_HEADERType
                {
                    SESSION_ID = BaseAdapter.SessionId,
                },
                MARK = new MarkCreditNoteRequestMARK
                {
                    value = nameof(EI.DocumentStatus.UNREAD),
                    UUID = uuidListMarkToGet.ToArray(),//getCreditNote ile çekilen müstahsilleri okunmadı işaretler              
                }
            };

            MarkCreditNoteResponse response = _izibizClient.CreditNote().MarkCreditNoteResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            GetCreditNote_BetweenDate();
        }

    }
}


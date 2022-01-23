using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Izibiz_dotnet_soap_client;
using Izibiz_dotnet_soap_client.UblCreate;
using Izibiz_dotnet_soap_client.Adapter;
using NUnit.Framework;
using Izibiz_dotnet_soap_client.EIrsaliyeWS;
using Izibiz_dotnet_soap_client.UblSerializer;
using Izibiz_dotnet_soap_client.Operations;
using UblReceipt;

namespace Samples.Receipt
{
    //[Ignore("Waiting for Joe to fix his bugs", Until = "2022-07-31 12:00:00Z")]
    class ReceiptSample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();
        public static List<string> uuidList = new List<string>();
        public static RECEIPTADVICEINFO[] receiptToMarkRange;
        ReceiptUBL receipt = new ReceiptUBL();
        ReceiptAdviceType receiptType;

         [Test,Order(2)]
        public void SendReceipt()
      {// 1 irsaliye 1 kez yanıt gönderilir. yanıt gönderilebilmesi için irsaliyenin ıd ve uuisi xmlde ilgili alana yazılır.
            receiptType = receipt.baseReceiptUBL;

            String xmlString = null;

            using (var stringwriter = new Utf8StringWriter())
            {
                var serializer = new XmlSerializer(receiptType.GetType());
                serializer.Serialize(stringwriter, receiptType, ReceiptSerializer.GetXmlSerializerNamespace());
                xmlString = stringwriter.ToString();
            }

            base64Binary base64binary = new base64Binary();
            base64binary.contentType = nameof(EI.DocumentType.XML);

            byte[] zipFile =  Compress.compressFile(xmlString);
            base64binary.Value = zipFile;

            var request = new SendReceiptAdviceRequest
            {
                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                RECEIPTADVICE=new RECEIPTADVICE[]
                {
                    new RECEIPTADVICE
                    {
                        CONTENT=base64binary,
                    }
                }
            };

            SendReceiptAdviceResponse response = _izibizClient.EDespatch().SendReceiptAdviceResponse(request);
            Assert.Null(response.ERROR_TYPE);
            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                // FolderPath.FileYesNo(nameof(EI.FileName.RECEIPTSEND), receiptType.ID.Value, receiptType.UUID.Value, zipFile);
                FolderPath.SendAndLoadSaveToDisk(nameof(EI.FileName.RECEIPTSEND), receiptType.ID.Value, receiptType.UUID.Value, zipFile,nameof(EI.Type.RECEIPT));
                uuidList.Add(receiptType.UUID.Value);
            }
        }

        [Test,Order(1)]
        public void LoadReceipt()
        {//gelen irsaliyenin bilgileri BaseReceiptUBL.cs de doldurulur.
            receipt = new ReceiptUBL();
            receiptType = receipt.baseReceiptUBL;

            String xmlString = null;

            using (var stringwriter = new Utf8StringWriter())
            {
                var serializer = new XmlSerializer(receiptType.GetType());
                serializer.Serialize(stringwriter, receiptType, ReceiptSerializer.GetXmlSerializerNamespace());
                xmlString = stringwriter.ToString();
            }

            byte[] zipFile = Compress.compressFile(xmlString);

            base64Binary base64binary = new base64Binary();
            base64binary.contentType = nameof(EI.DocumentType.XML);
            base64binary.Value = zipFile;

            var request = new LoadReceiptAdviceRequest
            {
                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                RECEIPTADVICE = new RECEIPTADVICE[]
                {
                   new RECEIPTADVICE{
                    CONTENT =base64binary,
                   }
                },
                
            };


            LoadReceiptAdviceResponse response = _izibizClient.EDespatch().LoadReceiptAdviceResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);

            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                FolderPath.SendAndLoadSaveToDisk(nameof(EI.FileName.RECEIPTLOAD), receiptType.ID.Value, receiptType.UUID.Value, zipFile,nameof(EI.Type.RECEIPT));
                uuidList.Add(receiptType.UUID.Value);
            }

        }


        [Test, Order(3)]
        public void GetReceiptAdviceStatus()
        {
            var request = new GetReceiptAdviceStatusRequest
            {
                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                UUID = uuidList.ToArray(),
            };

            GetReceiptAdviceStatusResponse response = _izibizClient.EDespatch().GetReceiptAdviceStatusResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.NotNull(response.RECEIPTADVICE_STATUS);

        }


        [Test,Order(5)]
        public void MarkReceiptAdvice_Read()
        {
            var request = new MarkReceiptAdviceRequest
            {
                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                MARK = new MarkReceiptAdviceRequestMARK
                {
                    value = nameof(EI.DocumentStatus.READ),
                    RECEIPTADVICEINFO = receiptToMarkRange,
                }
            };

            MarkReceiptAdviceResponse response = _izibizClient.EDespatch().MarkReceiptAdviceResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
        }


       [Test, Order(6)]
        public void MarkReceiptAdvice_Unread()
        {
            var request = new MarkReceiptAdviceRequest
            {
                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                MARK = new MarkReceiptAdviceRequestMARK
                {
                    value = nameof(EI.DocumentStatus.UNREAD),
                    RECEIPTADVICEINFO = receiptToMarkRange,
                }
            };

            MarkReceiptAdviceResponse response = _izibizClient.EDespatch().MarkReceiptAdviceResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
        }


        [Test, Order(4)]
        public void GetReceipt()
        {
            var request = new GetReceiptAdviceRequest
            {
                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                SEARCH_KEY = new GetReceiptAdviceRequestSEARCH_KEY
                {
                    LIMIT = 5,
                    LIMITSpecified = true,
                    READ_INCLUDED = false,
                    READ_INCLUDEDSpecified = true,
                    START_DATE = Convert.ToDateTime("2021-12-28"),
                    START_DATESpecified = true,
                    END_DATE = Convert.ToDateTime("2021-12-30"),
                    END_DATESpecified = true,
                    DIRECTION = nameof(EI.Direction.OUT)
                },
                HEADER_ONLY = nameof(EI.YesNo.N)
            };
            GetReceiptAdviceResponse response = _izibizClient.EDespatch().GetReceiptAdviceResponse(request);
            Assert.NotNull(response.RECEIPTADVICE);
            Assert.Null(response.ERROR_TYPE);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            {
                foreach (RECEIPTADVICE inv in response.RECEIPTADVICE)
                {
                    FolderPath.SaveToDisk(inv.CONTENT.Value, inv.UUID, request.SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.RECEIPT), nameof(EI.DocumentType.NULL), inv.ID);
                    uuidList.Add(inv.UUID);
                }
            }
            receiptToMarkRange = new RECEIPTADVICEINFO[response.RECEIPTADVICE.Length];
            receiptToMarkRange = response.RECEIPTADVICE.Select(a => new RECEIPTADVICE() {UUID = a.UUID }).ToList().ToArray();
        }

    }
}


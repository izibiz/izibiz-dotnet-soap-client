using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Izibiz;
using Izibiz.UblCreate;
using Izibiz.Adapter;
using Izibiz.EInvoiceWS;
using NUnit.Framework;
using UblInvoice;
using Izibiz.UblSerializer;
using Izibiz.Operations;

namespace Samples.EInvoice
{
  
    class A10EInvoiceSample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();
        public static List<string> uuidList = new List<string>();
        InvoiceUBL invoice;
        InvoiceType invoiceType;

        [Test,Order(1)]
        public void LoadInvoice()
         {
            invoice = new InvoiceUBL();
            invoiceType = invoice.baseInvoiceUBL;

            string xmlString= XmlSerializerr.XmlSerializeInvoice(invoiceType);

           base64Binary base64binary = new base64Binary();
            base64binary.contentType = nameof(EI.DocumentType.XML);

            byte[] zipFile =  Compress.compressFile(xmlString);
            base64binary.Value = zipFile;

            var request = new LoadInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE = new INVOICE[1]
                {
                     new INVOICE
                     {
                          CONTENT = base64binary
                     }
                }
            };
            LoadInvoiceResponse response = _izibizClient.EInvoice().loadInvoice(request);
            Assert.Null(response.ERROR_TYPE);
            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                FileOperations.SendAndLoadSaveToDisk(nameof(EI.FileName.LOADINVOICE), invoiceType.UUID.Value, invoiceType.ID.Value,zipFile,nameof(EI.Type.INVOICE));
            }
            uuidList.Add(invoiceType.UUID.Value);//Gönderim başarılı ise statusunu sorgulamak için yüklenilen belgelen uuidsi diziye atanır.
        }

       [Test,Order(2)]
        public void SendInvoice()
        {
           
            base64Binary[] base64binary = new base64Binary[1];

       
            invoice = new InvoiceUBL();
            invoiceType = invoice.baseInvoiceUBL;
            
            string xmlString = XmlSerializerr.XmlSerializeInvoice(invoiceType);

            byte[] zipFile = Compress.compressFile(xmlString);
            base64binary[0] = new base64Binary
            {
                contentType = nameof(EI.DocumentType.XML),
                Value = zipFile
            };

            var request = new SendInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                SENDER = new SendInvoiceRequestSENDER
                {
                    vkn = "4840847211",
                    alias = "urn:mail:defaultgb@izibiz.com.tr"

                },
                RECEIVER = new SendInvoiceRequestRECEIVER
                {
                    vkn = "4840847211",
                    alias = "urn:mail:defaultpk@izibiz.com.tr"
                },
                INVOICE = new INVOICE[]
                {

                    new INVOICE
                     {
                             CONTENT = base64binary[0]
                     },
                    
                }
            };
            SendInvoiceResponse response = _izibizClient.EInvoice().sendInvoice(request);
            Assert.Null(response.ERROR_TYPE);

            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                FileOperations.SendAndLoadSaveToDisk(nameof(EI.FileName.SENDINVOICE), invoiceType.UUID.Value, invoiceType.ID.Value, zipFile,nameof(EI.Type.INVOICE));           
            }
            uuidList.Add(invoiceType.UUID.Value);
        }

         [Test]
        public void SendInvoiceResponseWithServerSign()
        {// gelen ticari faturalara  uygulama yanıtı göndermeyi sağlayan servistir.(red veya kabul)

            var request = new SendInvoiceResponseWithServerSignRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                STATUS = nameof(EI.Status.KABUL),//kabul veya red seçimi yapılır.
                INVOICE = new INVOICE[]
                {
                     new INVOICE
                     {
                             ID="TEE2022000000015"//ticari faturanın fatura ıd'si yazılır.
                     },
                    
                },
                DESCRIPTION = new String[] { "Onaylandı. Denemedir" },

            };
            SendInvoiceResponseWithServerSignResponse response = _izibizClient.EInvoice().sendInvoiceResponseWithServerSign(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
        }


    }
}


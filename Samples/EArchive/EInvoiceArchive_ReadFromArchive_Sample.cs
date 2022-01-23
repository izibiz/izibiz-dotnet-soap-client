using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Izibiz_dotnet_soap_client;
using Izibiz_dotnet_soap_client.Adapter;
using Izibiz_dotnet_soap_client.EArchiveInvoiceWS;
using Izibiz_dotnet_soap_client.Operations;
using NUnit.Framework;

namespace Samples.EArchive
{
    // [Ignore("Waiting for Joe to fix his bugs", Until = "2022-07-31 12:00:00Z")]
    class EInvoiceArchive_ReadFromArchive_Sample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();

        [Test]
        public void ReadFromArchive_PDF()
        {//Compressed alanımız N de olsa Y de olsa pdf te 1 kere zipleme yapar
            var request = new ArchiveInvoiceReadRequest
            {
                REQUEST_HEADER = BaseAdapter.EArchiveRequestHeaderType(),
                INVOICEID = "0055215d-f3c1-46b1-bc91-dbbb58e495ea",//E ARŞİV faturalarda hangi faturanın okunmasını isteniyorsa
                PORTAL_DIRECTION = nameof(EI.Direction.OUT),
                PROFILE = nameof(EI.DocumentType.PDF)
            };
            ArchiveInvoiceReadResponse response = _izibizClient.EInvoiceArchive().ArchiveRead(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            Assert.IsTrue(response.INVOICE.Length > 0);
            string yeni = Convert.ToBase64String(response.INVOICE[0].Value);

            if (response.INVOICE[0].Value != null)
            {
                FolderPath.SaveToDisk(response.INVOICE[0].Value, request.INVOICEID, request.PORTAL_DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.EARCHIVEINVOICE), request.PROFILE);

            }
        }


          [Test]
        public void ReadFromArchive_XML()
        {//XML DE COMPRESSED ALANI N VEYA Y OLSADA ZİPLİ GELMİYOR.
            var deger = BaseAdapter.EArchiveRequestHeaderType();
            deger.COMPRESSED = nameof(EI.YesNo.N);
            var request = new ArchiveInvoiceReadRequest
            {
                REQUEST_HEADER = deger,
                INVOICEID = "0055215d-f3c1-46b1-bc91-dbbb58e495ea",//E ARŞİV faturalarda hangi faturanın okunmasını isteniyorsa
                PORTAL_DIRECTION = nameof(EI.Direction.OUT),
                PROFILE = nameof(EI.DocumentType.XML)
            };

            ArchiveInvoiceReadResponse response = _izibizClient.EInvoiceArchive().ArchiveRead(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            Assert.IsTrue(response.INVOICE.Length > 0);

            string yeni = Convert.ToBase64String(response.INVOICE[0].Value);

            if (response.INVOICE[0].Value != null)
            {
                FolderPath.SaveToDisk(response.INVOICE[0].Value, request.INVOICEID, request.PORTAL_DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.EARCHIVEINVOICE), request.PROFILE);
            }
        }

         [Test]
        public void ReadFromArchive_HTML()
        {//Compressed alanımız Y İKEN 2 KERE ZİPLİ GELİYOR N İKEN RESPONSE.INVOİCE VERİMİZ ZİPSİZ HALDE GELİR.
            var deger = BaseAdapter.EArchiveRequestHeaderType();
            deger.COMPRESSED = nameof(EI.YesNo.Y);
            var request = new ArchiveInvoiceReadRequest
            {
                REQUEST_HEADER = deger,
                INVOICEID = "0055215d-f3c1-46b1-bc91-dbbb58e495ea",//E ARŞİV faturalarda hangi faturanın okunmasını isteniyorsa
                PORTAL_DIRECTION = nameof(EI.Direction.OUT),
                PROFILE = nameof(EI.DocumentType.HTML)
            };

            ArchiveInvoiceReadResponse response = _izibizClient.EInvoiceArchive().ArchiveRead(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            Assert.IsTrue(response.INVOICE.Length > 0);
            string decodedString = Encoding.UTF8.GetString(response.INVOICE[0].Value);

            string yeni = Convert.ToBase64String(response.INVOICE[0].Value);//Gelen byte değerini base64 string'e çeviriliyor.

            if (response.INVOICE[0].Value != null)
            {
                FolderPath.SaveToDisk(response.INVOICE[0].Value, request.INVOICEID, request.PORTAL_DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.EARCHIVEINVOICE), request.PROFILE);
            }

        }
    }
}


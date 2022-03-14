using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Izibiz;
using Izibiz.Adapter;
using Izibiz.EArchiveInvoiceWS;
using Izibiz.Operations;
using NUnit.Framework;

namespace Samples.EArchive
{
   
    class EInvoiceArchive_ReadFromArchive_Sample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();

        [Test]
        public void ReadFromArchive_PDF()
        {
            var request = new ArchiveInvoiceReadRequest
            {
                REQUEST_HEADER = BaseAdapter.EArchiveRequestHeaderType(),
                INVOICEID = "0055215d-f3c1-46b1-bc91-dbbb58e495ea",//E ARŞİV faturalarda hangi faturanın okunmasını isteniyorsa onun uuıd'si yazılmalı
                PORTAL_DIRECTION = nameof(EI.Direction.OUT),
                PROFILE = nameof(EI.DocumentType.PDF)
            };
            ArchiveInvoiceReadResponse response = _izibizClient.EInvoiceArchive().ArchiveRead(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            Assert.IsTrue(response.INVOICE.Length > 0);
            string yeni = Convert.ToBase64String(response.INVOICE[0].Value);

            if (response.INVOICE[0].Value != null)
            {
                FileOperations.SaveToDisk(response.INVOICE[0].Value, request.INVOICEID, request.PORTAL_DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.EARCHIVEINVOICE), request.PROFILE);

            }
        }


          [Test]
        public void ReadFromArchive_XML()
        {
            var deger = BaseAdapter.EArchiveRequestHeaderType();
            deger.COMPRESSED = nameof(EI.YesNo.N);
            var request = new ArchiveInvoiceReadRequest
            {
                REQUEST_HEADER = deger,
                INVOICEID = "0055215d-f3c1-46b1-bc91-dbbb58e495ea",//E ARŞİV faturalarda hangi faturanın okunmasını isteniyorsa onun uuıd'si yazılmalı
                PORTAL_DIRECTION = nameof(EI.Direction.OUT),
                PROFILE = nameof(EI.DocumentType.XML)
            };

            ArchiveInvoiceReadResponse response = _izibizClient.EInvoiceArchive().ArchiveRead(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            Assert.IsTrue(response.INVOICE.Length > 0);

            string yeni = Convert.ToBase64String(response.INVOICE[0].Value);

            if (response.INVOICE[0].Value != null)
            {
                FileOperations.SaveToDisk(response.INVOICE[0].Value, request.INVOICEID, request.PORTAL_DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.EARCHIVEINVOICE), request.PROFILE);
            }
        }

         [Test]
        public void ReadFromArchive_HTML()
        {
            var deger = BaseAdapter.EArchiveRequestHeaderType();
            deger.COMPRESSED = nameof(EI.YesNo.Y);
            var request = new ArchiveInvoiceReadRequest
            {
                REQUEST_HEADER = deger,
                INVOICEID = "0055215d-f3c1-46b1-bc91-dbbb58e495ea",//E ARŞİV faturalarda hangi faturanın okunmasını isteniyorsa onun uuıd'si yazılmalı
                PORTAL_DIRECTION = nameof(EI.Direction.OUT),
                PROFILE = nameof(EI.DocumentType.HTML)
            };

            ArchiveInvoiceReadResponse response = _izibizClient.EInvoiceArchive().ArchiveRead(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            Assert.IsTrue(response.INVOICE.Length > 0);
            string decodedString = Encoding.UTF8.GetString(response.INVOICE[0].Value);

            string yeni = Convert.ToBase64String(response.INVOICE[0].Value);

            if (response.INVOICE[0].Value != null)
            {
                FileOperations.SaveToDisk(response.INVOICE[0].Value, request.INVOICEID, request.PORTAL_DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.EARCHIVEINVOICE), request.PROFILE);
            }

        }
    }
}


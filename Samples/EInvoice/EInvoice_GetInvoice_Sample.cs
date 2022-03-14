using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Izibiz;
using Izibiz.Ubl;
using Izibiz.Adapter;
using Izibiz.AuthenticationWS;
using Izibiz.EInvoiceWS;
using NUnit.Framework;
using System.Xml;
using System.Xml.Xsl;
using System.Diagnostics;
using Izibiz.Operations;
using System.Collections;

namespace Samples.EInvoice
{

    class EInvoice_GetInvoice_Sample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();


        [Test]
        public void GetInvoice_Unread()
        {
            var request = new GetInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY
                {
                    LIMIT = 100,
                    LIMITSpecified = true,
                    READ_INCLUDED = false,
                    READ_INCLUDEDSpecified = true,
                    DIRECTION = nameof(EI.Direction.IN)
                },
                HEADER_ONLY = nameof(EI.YesNo.N)
            };
            GetInvoiceResponse response = _izibizClient.EInvoice().getInvoice(request);
            Assert.NotNull(response.INVOICE);
            Assert.IsTrue(response.INVOICE.Length > 0);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            {
                foreach (INVOICE inv in response.INVOICE)
                {
                    FileOperations.SaveToDisk(inv.CONTENT.Value, inv.UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), nameof(EI.DocumentType.NULL), inv.ID);

                }
            }

        }

        [Test]
        public void GetInvoice_SingleInvoice()
        {
            var request = new GetInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY
                {
                    UUID = "4EB503B7-F26D-46A2-B173-618A9507A469",
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true,
                    DIRECTION = nameof(EI.Direction.IN)
                },
                HEADER_ONLY = nameof(EI.YesNo.N)
            };
            GetInvoiceResponse response = _izibizClient.EInvoice().getInvoice(request);
            Assert.NotNull(response.INVOICE);
            Assert.AreEqual(request.INVOICE_SEARCH_KEY.UUID, response.INVOICE[0].UUID);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            {
                FileOperations.SaveToDisk(response.INVOICE[0].CONTENT.Value, response.INVOICE[0].UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), nameof(EI.DocumentType.NULL), response.INVOICE[0].ID);
            }
        }

        [Test]
        public void GetInvoice_BetweenDate()
        {
            // Lütfen buradaki yapıya göre tasarım yapınız https://dev.izibiz.com.tr/#dikkat-edilecek-hususlar
            // GetInvoice metodu ile firmaya gelen faturalar müşteri bilgisayarına aktarılır.
            //İzibiz sistemlerine gelen yeni faturaları almanız gerekmektedir.
            //Servis ile yeni gelen en fazla 100 adet faturayı çekebilirsiniz.
            //Eğer dönen listede 100 adet fatura varsa yeniden getinvoice servisi çağırılarak başka fatura olup olmadığı kontrol edilmelidir.
            //Dönen listede 100den az fatura varsa tekrar sorgulama yapmaya gerek yoktur.
            //Fatura çekme zamanlayıcı ile yapılıyorsa en az 15 dk bir servis çağırılmalıdır.

            var request = new GetInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY
                {
                    LIMIT = 100,
                    LIMITSpecified = true,
                    START_DATE = Convert.ToDateTime("2021-11-01"),
                    START_DATESpecified = true,
                    END_DATE = Convert.ToDateTime("2022-02-20"),
                    END_DATESpecified = true,
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true,
                    DIRECTION = nameof(EI.Direction.IN)
                },
                HEADER_ONLY = nameof(EI.YesNo.N)
            };
            GetInvoiceResponse response = _izibizClient.EInvoice().getInvoice(request);
            Assert.NotNull(response.INVOICE);
            Assert.IsTrue(response.INVOICE.Length > 0);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            {
                foreach (INVOICE inv in response.INVOICE)
                {
                    FileOperations.SaveToDisk(inv.CONTENT.Value, inv.UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), nameof(EI.DocumentType.NULL), inv.ID);
                }
            }
            BaseAdapter.invoiceToMarkInvoice = new INVOICE[response.INVOICE.Length];
            BaseAdapter.invoiceToMarkInvoice = response.INVOICE.Select(a => new INVOICE() { ID = a.ID, UUID = a.UUID }).ToList().ToArray();
        }

        [Test]
        public void GetInvoice_Outgoing()
        {
            var request = new GetInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY
                {
                    LIMIT = 100,
                    LIMITSpecified = true,
                    READ_INCLUDED = false,
                    READ_INCLUDEDSpecified = true,
                    DIRECTION = nameof(EI.Direction.OUT)
                },
                HEADER_ONLY = nameof(EI.YesNo.N)
            };
            GetInvoiceResponse response = _izibizClient.EInvoice().getInvoice(request);
            Assert.NotNull(response.INVOICE);
            Assert.IsTrue(response.INVOICE.Length > 0);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            {
                foreach (INVOICE inv in response.INVOICE)
                {
                    FileOperations.SaveToDisk(inv.CONTENT.Value, inv.UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), nameof(EI.DocumentType.NULL), inv.ID);
                }
            }
        }

    }
}


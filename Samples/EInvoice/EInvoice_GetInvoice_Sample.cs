using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Izibiz_dotnet_soap_client;
using Izibiz_dotnet_soap_client.Ubl;
using Izibiz_dotnet_soap_client.Adapter;
using Izibiz_dotnet_soap_client.AuthenticationWS;
using Izibiz_dotnet_soap_client.EInvoiceWS;
using NUnit.Framework;
using System.Xml;
using System.Xml.Xsl;
using System.Diagnostics;
using Izibiz_dotnet_soap_client.Operations;
using System.Collections;

namespace Samples.EInvoice
{
 //[Ignore("Waiting for Joe to fix his bugs", Until = "2022-07-31 12:00:00Z")]
    class EInvoice_GetInvoice_Sample
    {//HEADER_ONLY Y İken content alanı gelmez N iken foreach döngüsüne GİRER.
        private readonly IzibizClient _izibizClient = new IzibizClient();


        [Test]
        public void GetInvoice_Unread()
        {
            var request = new GetInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY
                {
                    LIMIT = 10,
                    LIMITSpecified=true,
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
                    FolderPath.SaveToDisk(inv.CONTENT.Value, inv.UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), nameof(EI.DocumentType.NULL), inv.ID);

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
                FolderPath.SaveToDisk(response.INVOICE[0].CONTENT.Value, response.INVOICE[0].UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), nameof(EI.DocumentType.NULL), response.INVOICE[0].ID);
            }
        }

        [Test]
        public void GetInvoice_BetweenDate()
        {
              var request = new GetInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY
                {
                    LIMIT = 10,
                    LIMITSpecified = true,
                    START_DATE = Convert.ToDateTime("2018-12-01"),
                    END_DATE = Convert.ToDateTime("2022-01-06"),
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
                    FolderPath.SaveToDisk(inv.CONTENT.Value, inv.UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), nameof(EI.DocumentType.NULL), inv.ID);
                }
            }
            BaseAdapter.invoiceToMarkInvoice = new INVOICE[response.INVOICE.Length];
            //BaseAdapter.invoiceToMarkInvoice = response.INVOICE.ToList().Select(x => new INVOICE()
            //{
            //    ID = x.ID,
            //    UUID = x.UUID
            //}).ToArray();
             BaseAdapter.invoiceToMarkInvoice = response.INVOICE.Select(a => new INVOICE() { ID = a.ID, UUID = a.UUID }).ToList().ToArray();
          
            //InvoiceMap clasında ıd ve uuid propertyleri mevcut
            //BaseAdapter.inv = response.INVOICE.ToList().Select(x => new InvoiceMap()
            //{
            //    ID = x.ID,
            //    UUID = x.UUID
            //}).ToArray();

            //BaseAdapter.invoiceToMarkInvoice = BaseAdapter.inv.ToList().Select(o =>
            //      new INVOICE
            //       {
            //          ID = o.ID,
            //          UUID = o.UUID
            //        }).ToArray();
        }

          [Test]
        public void GetInvoice_Outgoing()
        {
            var request = new GetInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY
                {
                    LIMIT = 10,
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
                    FolderPath.SaveToDisk(inv.CONTENT.Value, inv.UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), nameof(EI.DocumentType.NULL), inv.ID);
                }
            }
        }

    }
}


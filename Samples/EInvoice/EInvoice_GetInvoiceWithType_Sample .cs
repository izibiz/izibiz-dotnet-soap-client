using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Izibiz;
using Izibiz.Adapter;
using Izibiz.AuthenticationWS;
using Izibiz.EInvoiceWS;
using Izibiz.Operations;
using NUnit.Framework;

namespace Samples.EInvoice
{
   
    class EInvoice_GetInvoiceWithType_Sample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();
        public string SingleUUID;

        [Test,Order(1)]
        public void GetInvoice_SingleInvoice()
        {
            var request = new GetInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY
                {
                    LIMIT=1,
                    LIMITSpecified=true,
                    START_DATE = Convert.ToDateTime("2022-02-20"),
                    START_DATESpecified = true,
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true,
                    DIRECTION = nameof(EI.Direction.IN)
                },
                HEADER_ONLY = nameof(EI.YesNo.N)
            };
            GetInvoiceResponse response = _izibizClient.EInvoice().getInvoice(request);
            SingleUUID = response.INVOICE[0].UUID;
        }



       [Test,Order(2)]
        public void GetInvoiceWithType_XML()
        {
            var request = new GetInvoiceWithTypeRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceWithTypeRequestINVOICE_SEARCH_KEY
                {
                    UUID=SingleUUID,
                    TYPE = nameof(EI.DocumentType.XML),
                    DIRECTION = nameof(EI.Direction.IN),
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true
                },
                HEADER_ONLY = nameof(EI.YesNo.N),
            };
            GetInvoiceWithTypeResponse response = _izibizClient.EInvoice().getInvoiceWThtml(request);
            Assert.NotNull(response.INVOICE);
            Assert.IsTrue(response.INVOICE.Length > 0);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            { FileOperations.SaveToDisk(response.INVOICE[0].CONTENT.Value, response.INVOICE[0].UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), request.INVOICE_SEARCH_KEY.TYPE, response.INVOICE[0].ID); }

        }

       [Test,Order(2)]
        public void GetInvoiceWithType_HTML()
        {
            var request = new GetInvoiceWithTypeRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceWithTypeRequestINVOICE_SEARCH_KEY
                {
                    UUID = SingleUUID,
                    TYPE = nameof(EI.DocumentType.HTML),
                    DIRECTION = nameof(EI.Direction.IN),
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true
                },
                HEADER_ONLY = nameof(EI.YesNo.N),
            };
            GetInvoiceWithTypeResponse response = _izibizClient.EInvoice().getInvoiceWThtml(request);
            Assert.NotNull(response.INVOICE);
            Assert.IsTrue(response.INVOICE.Length > 0);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            { FileOperations.SaveToDisk(response.INVOICE[0].CONTENT.Value, response.INVOICE[0].UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), request.INVOICE_SEARCH_KEY.TYPE, response.INVOICE[0].ID); }
        }

       [Test,Order(2)]
        public void GetInvoiceWithType_PDF()
        {
            var request = new GetInvoiceWithTypeRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceWithTypeRequestINVOICE_SEARCH_KEY
                {
                    UUID = SingleUUID,
                    TYPE = nameof(EI.DocumentType.PDF),
                    DIRECTION = nameof(EI.Direction.IN),
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true

                },
                HEADER_ONLY = nameof(EI.YesNo.N),
            };
            GetInvoiceWithTypeResponse response = _izibizClient.EInvoice().getInvoiceWTpDF(request);
            Assert.NotNull(response.INVOICE);
            Assert.IsTrue(response.INVOICE.Length > 0);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            { FileOperations.SaveToDisk(response.INVOICE[0].CONTENT.Value, response.INVOICE[0].UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), request.INVOICE_SEARCH_KEY.TYPE, response.INVOICE[0].ID); }
        }


       [Test, Order(3)]
        public void GetInvoice_SingleInvoice_Out()
        {
            var request = new GetInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceRequestINVOICE_SEARCH_KEY
                {
                    LIMIT = 1,
                    LIMITSpecified = true,
                    START_DATE = Convert.ToDateTime("2022-02-20"),
                    START_DATESpecified=true,
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true,
                    DIRECTION = nameof(EI.Direction.OUT)
                },
                HEADER_ONLY = nameof(EI.YesNo.N)
            };
            GetInvoiceResponse response = _izibizClient.EInvoice().getInvoice(request);
            SingleUUID = response.INVOICE[0].UUID;
        }

        [Test,Order(4)]
        public void GetInvoiceWithType_HTML_OUT()
        {
            var request = new GetInvoiceWithTypeRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceWithTypeRequestINVOICE_SEARCH_KEY
                {
                    UUID = SingleUUID,
                    TYPE = nameof(EI.DocumentType.HTML),
                    DIRECTION = nameof(EI.Direction.OUT),
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true
                },
                HEADER_ONLY = nameof(EI.YesNo.N),
            };
            GetInvoiceWithTypeResponse response = _izibizClient.EInvoice().getInvoiceWThtmlOut(request);
            Assert.NotNull(response.INVOICE);
            Assert.IsTrue(response.INVOICE.Length > 0);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            { FileOperations.SaveToDisk(response.INVOICE[0].CONTENT.Value, response.INVOICE[0].UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), request.INVOICE_SEARCH_KEY.TYPE, response.INVOICE[0].ID); }
        }

        [Test, Order(4)]
        public void GetInvoiceWithType_PDF_OUT()
        {
            var request = new GetInvoiceWithTypeRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                INVOICE_SEARCH_KEY = new GetInvoiceWithTypeRequestINVOICE_SEARCH_KEY
                {
                    UUID = SingleUUID,
                    TYPE = nameof(EI.DocumentType.PDF),
                    DIRECTION = nameof(EI.Direction.OUT),
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true
                },
                HEADER_ONLY = nameof(EI.YesNo.N),
            };
            GetInvoiceWithTypeResponse response = _izibizClient.EInvoice().getInvoiceWTpdfOut(request);
            Assert.NotNull(response.INVOICE);
            Assert.IsTrue(response.INVOICE.Length > 0);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            { FileOperations.SaveToDisk(response.INVOICE[0].CONTENT.Value, response.INVOICE[0].UUID, request.INVOICE_SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.INVOICE), request.INVOICE_SEARCH_KEY.TYPE, response.INVOICE[0].ID); }
        }
    }
}

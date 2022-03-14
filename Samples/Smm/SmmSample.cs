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
using NUnit.Framework;
using Izibiz.SmmWs;
using Izibiz.UblSerializer;
using Izibiz.Operations;
using UblInvoice;

namespace Samples.Smm
{
    class SmmSample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();
        public static List<string> uuidList = new List<string>();
        SmmUBL smm = new SmmUBL();
        InvoiceType invoiceType;

        [Test, Order(1)]
        public void SendSmm()
        {
            invoiceType = smm.baseInvoiceUBL;

            string xmlString = null;

            xmlString = XmlSerializerr.XmlSerializeInvoice(invoiceType);

            base64Binary base64binary = new base64Binary();
            base64binary.contentType = nameof(EI.DocumentType.XML);

            byte[] zipFile = Compress.compressFile(xmlString);
            base64binary.Value = zipFile;

            var request = new SendSmmRequest
            {
                REQUEST_HEADER = BaseAdapter.SmmWSRequestHeaderType(),
                SMM = new SMM[]
                {
                    new SMM
                    {
                        ID=invoiceType.ID.Value,
                        UUID=invoiceType.UUID.Value,
                        CONTENT=base64binary,
                    }
                },
                SMM_PROPERTIES = new SMM_PROPERTIES
                {
                    SENDING_TYPE = SENDING_TYPE.KAGIT,
                    SENDING_TYPESpecified = true
                }

            };

            SendSmmResponse response = _izibizClient.Smm().SendSmmResponse(request);
            Assert.Null(response.ERROR_TYPE);

            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                FileOperations.SendAndLoadSaveToDisk(nameof(EI.FileName.SMMSEND), invoiceType.ID.Value, invoiceType.UUID.Value, zipFile, nameof(EI.Type.SMM));
                uuidList.Add(invoiceType.UUID.Value);
            }
       }


        [Test]
        public void GetSmm()
        {
            var request = new GetSmmRequest
            {
                REQUEST_HEADER=BaseAdapter.SmmWSRequestHeaderType(),
                SMM_SEARCH_KEY=new GetSmmRequestSMM_SEARCH_KEY
                {
                    LIMIT=4,
                    LIMITSpecified=true,
                    START_DATE=Convert.ToDateTime("2020-01-20"),
                    START_DATESpecified=true,
                    END_DATE=Convert.ToDateTime("2022-01-24"),
                    END_DATESpecified=true,
                    READ_INCLUDED=FLAG_VALUE.Y,
                    READ_INCLUDEDSpecified=true,
                },
                HEADER_ONLY=FLAG_VALUE.Y,
                CONTENT_TYPE=CONTENT_TYPE.PDF,
            };

            GetSmmResponse response = _izibizClient.Smm().GetSmmResponse(request);
            Assert.Null(response.ERROR_TYPE);
        }

         [Test, Order(2)]
        public void GetSmmStatus()
        {
            var request = new GetSmmStatusRequest
            {
                REQUEST_HEADER = BaseAdapter.SmmWSRequestHeaderType(),
                UUID=uuidList.ToArray(),
            };

            GetSmmStatusResponse response = _izibizClient.Smm().GetSmmStatusResponse(request);
            Assert.Null(response.ERROR_TYPE);
        }


        [Test]
        public void GetSmmReport()
        {
            var request = new GetSmmReportRequest
            {
                REQUEST_HEADER = BaseAdapter.SmmWSRequestHeaderType(),
                START_DATE=Convert.ToDateTime("2021-01-01"),
                START_DATESpecified=true,
                END_DATE= Convert.ToDateTime("2022-01-24"),
                END_DATESpecified=true,
                HEADER_ONLY=FLAG_VALUE.N,
                HEADER_ONLYSpecified=true,
            };

            GetSmmReportResponse response = _izibizClient.Smm().GetSmmReportResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.NotNull(response.SMM_REPORT);
        }

    }
}


﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Izibiz_dotnet_soap_client;
using Izibiz_dotnet_soap_client.Adapter;
using Izibiz_dotnet_soap_client.EArchiveInvoiceWS;
using Izibiz_dotnet_soap_client.Operations;
using Izibiz_dotnet_soap_client.UblCreate;
using Izibiz_dotnet_soap_client.UblSerializer;
using NUnit.Framework;
using UblInvoice;

namespace Samples.EArchive
{
   
    class EInvoiceArchiveSample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();
        ArchiveUBL archive = new ArchiveUBL();
        InvoiceType invoiceType;
        [Test,Order(1)]
        public void WriteToArchieveExtended()
        {
            invoiceType = archive.baseInvoiceUBL;
            invoiceType.ProfileID.Value = nameof(EI.Type.EARCHIVEINVOICE);
            invoiceType.AccountingCustomerParty.Party = BaseInvoiceUBL.createParty("BUKET DURU", "ADANA", "1235567899", "a@gmail.com", "4850899211");

            String xmlString = null;

            xmlString = XmlSerializerr.XmlSerializeInvoice(invoiceType);


            byte[] zipFile = Compress.compressFile(xmlString);
           

            base64Binary base64binary = new base64Binary();
            base64binary.contentType = nameof(EI.DocumentType.XML);
            base64binary.Value = zipFile;

            EARSIV_PROPERTIES earsiv = new EARSIV_PROPERTIES();
            earsiv.EARSIV_TYPE = EARSIV_TYPE_VALUE.NORMAL;
            earsiv.EARSIV_EMAIL_FLAG = FLAG_VALUE.N;
            earsiv.SUB_STATUS = SUB_STATUS_VALUE.NEW;//New ile arşiv faturalara yüklenir taslaklara yüklemek istenilirse DRAFT yazılması gerekmektedir.

            ArchiveInvoiceExtendedContentINVOICE_PROPERTIES[] EInvoiceArchiveProperties = new ArchiveInvoiceExtendedContentINVOICE_PROPERTIES[5];
            EInvoiceArchiveProperties[0] = new ArchiveInvoiceExtendedContentINVOICE_PROPERTIES
            {
                EARSIV_FLAG = FLAG_VALUE.Y,
               
                EARSIV_PROPERTIES=earsiv,
                
                INVOICE_CONTENT = base64binary,
            };
            var request = new ArchiveInvoiceExtendedRequest
            {   
                REQUEST_HEADER = BaseAdapter.EArchiveRequestHeaderType(),
                ArchiveInvoiceExtendedContent =EInvoiceArchiveProperties,                          
            };
            ArchiveInvoiceExtendedResponse response = _izibizClient.EInvoiceArchive().WritetoArchiveExtendedd(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            Assert.NotNull(response.INVOICE_ID);

            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                if (earsiv.SUB_STATUS == SUB_STATUS_VALUE.NEW)
                {
                    FolderPath.SendAndLoadSaveToDisk(nameof(EI.FileName.EARCHIVEINVOICE), invoiceType.UUID.Value, invoiceType.ID.Value, zipFile,nameof(EI.Type.EARCHIVEINVOICE));
                }
                else
                {
                    FolderPath.SendAndLoadSaveToDisk(nameof(EI.FileName.EARCHIVEINVOICEDRAFT), invoiceType.UUID.Value, invoiceType.ID.Value, zipFile,nameof(EI.Type.EARCHIVEINVOICE));
                }
            }
        }
        [Test]//bir veya birden çok faturanın durumunu sorgulamayı sağlar.
        public void GetEArchiveStatus()
        {
            var request = new GetEArchiveInvoiceStatusRequest
            {
                REQUEST_HEADER = BaseAdapter.EArchiveRequestHeaderType(),
                UUID = new String[] { "1fa2c9d5-e60c-489d-ae48-f628eddc8411", "ee401add-a36a-4207-8d0f-74cfd26d7298" },
            };
            GetEArchiveInvoiceStatusResponse response = _izibizClient.EInvoiceArchive().GetEArchiveInvoiceStatusResponse(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE,0);
            Assert.NotNull(response.INVOICE);
            Assert.Null(response.ERROR_TYPE);
        }

        [Test]//earşiv faturalardaki raporlandı ve raporlanacak durumundakilerin iptali
        public void CancelEArchiveInvoice()
        {
            CancelEArchiveInvoiceRequestCancelEArsivInvoiceContent EArchiveContent = new CancelEArchiveInvoiceRequestCancelEArsivInvoiceContent
            {
                FATURA_ID= "NEA2021002000013",
                FATURA_UUID= "1fa2c9d5-e60c-489d-ae48-f628eddc8411",
                IPTAL_NOTU="Deneme yapma"
            };

            var request = new CancelEArchiveInvoiceRequest
            {
                REQUEST_HEADER = BaseAdapter.EArchiveRequestHeaderType(),
                CancelEArsivInvoiceContent=new CancelEArchiveInvoiceRequestCancelEArsivInvoiceContent[] 
                {
                    EArchiveContent
                },
            };
            CancelEArchiveInvoiceResponse response = _izibizClient.EInvoiceArchive().CancelEArchiveInvoiceResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
        }

       [Test] 
        public void GetEmailEarchiveInvoice()
        {
            var request = new GetEmailEarchiveInvoiceRequest
            {
                REQUEST_HEADER = BaseAdapter.EArchiveRequestHeaderType(),
                FATURA_UUID= "2779eff5-e8fa-4132-9d62-27a038434e7a",
                EMAIL= "meryem.aksu@izibiz.com.tr,meryemaksu.5869@gmail.com"
              };
            GetEmailEarchiveInvoiceResponse response = _izibizClient.EInvoiceArchive().GetEmailEarchiveInvoiceResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
        }

        [Test] 
        public void GetEArchiveReport()
        {
            var request = new GetEArchiveReportRequest
            {
                REQUEST_HEADER = BaseAdapter.EArchiveRequestHeaderType(),
                REPORT_PERIOD ="202001",
               REPORT_STATUS_FLAG="N",//Rapor durumunun sonuca eklenmesi isteniyorsa Y, değilse N değeri gönderilmelidir.
            };
            GetEArchiveReportResponse response = _izibizClient.EInvoiceArchive().GetEArchiveReportResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
        }

        [Test]
        public void ReadEArchiveReport()
        {
            var request = new ReadEArchiveReportRequest
            {
                REQUEST_HEADER = BaseAdapter.EArchiveRequestHeaderType(),
                RAPOR_NO = "15ffb255-a1a0-4efd-81a3-2e9a35371f3f",
            };
            ReadEArchiveReportResponse response = _izibizClient.EInvoiceArchive().ReadEArchiveReportResponse(request);
           
        }
    }
}


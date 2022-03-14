using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Izibiz;
using Izibiz.Adapter;
using Izibiz.EIrsaliyeWS;
using NUnit.Framework;
using UblDespatch;
using Izibiz.UblSerializer;
using Izibiz.UblCreate;
using Izibiz.Operations;

namespace Samples.DespatchS
{
  
    class EDespatch_Sample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();
        public static List<string> uuidList = new List<string>();
        public static DESPATCHADVICEINFO[] despatchAdviceInfo;
        DespatchUBL despatch=new DespatchUBL();
        DespatchAdviceType despatchType;

        [Test,Order(2)]
        public void SendDespatchAdvice()
        {
            despatchType = despatch.baseDespatchUBL;

            String xmlString = null;
            
            xmlString = XmlSerializerr.XmlSerializeDespatch(despatchType);

            byte[] zipFile = Compress.compressFile(xmlString);
           

            base64Binary base64binary = new base64Binary();
            base64binary.contentType = nameof(EI.DocumentType.XML);          
            base64binary.Value = zipFile;
            var request = new SendDespatchAdviceRequest
            {
                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                SENDER = new SendDespatchAdviceRequestSENDER
                {
                    vkn = "4840847211",
                    alias = "urn:mail:defaultgb@izibiz.com.tr"

                },
                RECEIVER = new SendDespatchAdviceRequestRECEIVER
                {
                    vkn = "4840847211",
                    alias = "urn:mail:defaultpk@izibiz.com.tr"
                },
                DESPATCHADVICE = new DESPATCHADVICE[1]
                {
                    new DESPATCHADVICE
                    {
                        CONTENT = base64binary
                    },
                },
            };
            SendDespatchAdviceResponse response = _izibizClient.EDespatch().SendDespatchAdviceResponse(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            Assert.Null(response.ERROR_TYPE);
            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                FileOperations.SendAndLoadSaveToDisk(nameof(EI.FileName.SENDDESPATCH), despatchType.ID.Value, despatchType.UUID.Value, zipFile,nameof(EI.Type.DESPATCH));
            }
            uuidList.Add(despatchType.UUID.Value);

          
        }


        [Test, Order(1)]
        public void LoadDespatchAdvice()
        {// birden fazla irsaliye yüklenecekse basebinary de dizi olarak tanımlanmalıdır.
           
            despatchType = despatch.baseDespatchUBL;
          
            String xmlString = null;

            xmlString = XmlSerializerr.XmlSerializeDespatch(despatchType);

            byte[] zipFile = Compress.compressFile(xmlString);


            base64Binary base64binary = new base64Binary();
            base64binary.contentType = nameof(EI.DocumentType.XML);
            base64binary.Value = zipFile;
          
            var request = new LoadDespatchAdviceRequest
            {

                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                DESPATCHADVICE = new DESPATCHADVICE[1]
                {
                    new DESPATCHADVICE{
                    ID = despatchType.ID.Value,
                    UUID = despatchType.UUID.Value,
                    CONTENT=base64binary
                    },
                },
            };
            LoadDespatchAdviceResponse response = _izibizClient.EDespatch().LoadDespatchAdviceResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
            if (response.REQUEST_RETURN.RETURN_CODE == 0)
            {
                FileOperations.SendAndLoadSaveToDisk(nameof(EI.FileName.LOADDESPATCH), despatchType.ID.Value, despatchType.UUID.Value, zipFile,nameof(EI.Type.DESPATCH));
            }
           uuidList.Add(despatchType.UUID.Value);
        }


        [Test,Order(2)]
        public void GetDespatch()
        {
            var request = new GetDespatchAdviceRequest
            {
                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                SEARCH_KEY = new GetDespatchAdviceRequestSEARCH_KEY
                {
                    LIMIT = 10,
                    LIMITSpecified = true,
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true,
                    START_DATE = Convert.ToDateTime("2021-12-28"),
                    START_DATESpecified = true,
                    END_DATE = Convert.ToDateTime("2021-12-30"),
                    END_DATESpecified = true,
                    DIRECTION = nameof(EI.Direction.OUT)
                },
                HEADER_ONLY = nameof(EI.YesNo.Y)
            };
            GetDespatchAdviceResponse response = _izibizClient.EDespatch().GetDespatchAdviceResponse(request);
            Assert.NotNull(response.DESPATCHADVICE);
            Assert.Null(response.ERROR_TYPE);
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            {
                foreach (DESPATCHADVICE inv in response.DESPATCHADVICE)
                {
                    FileOperations.SaveToDisk(inv.CONTENT.Value, inv.UUID, request.SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.DESPATCH), nameof(EI.DocumentType.NULL), inv.ID);
                }
            }
            despatchAdviceInfo = new DESPATCHADVICEINFO[response.DESPATCHADVICE.Length];
            despatchAdviceInfo = response.DESPATCHADVICE.Select(a => new DESPATCHADVICE() { UUID = a.UUID }).ToList().ToArray();

            foreach (DESPATCHADVICE inv in response.DESPATCHADVICE)
            {
                uuidList.Add(inv.UUID);
            }
        }

   


       [Test, Order(2)]
        public void GetDespatch_Single()
        {
            var request = new GetDespatchAdviceRequest
            {

                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                SEARCH_KEY = new GetDespatchAdviceRequestSEARCH_KEY
                {
                    ID= "XZE2021000000096",//çekmek istenilen faturanın ıdsi ve uuid'si yazılır.
                    UUID= "B537A0C8-9150-DE37-11AD-04D29850DA1B",
                    READ_INCLUDED = true,
                    READ_INCLUDEDSpecified = true,
                    DIRECTION = nameof(EI.Direction.IN)
                },
                HEADER_ONLY = nameof(EI.YesNo.N)
            };
            GetDespatchAdviceResponse response = _izibizClient.EDespatch().GetDespatchAdviceResponse(request);
            Assert.NotNull(response.DESPATCHADVICE);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(request.HEADER_ONLY, nameof(EI.YesNo.N));
            if (request.HEADER_ONLY == nameof(EI.YesNo.N))
            {
                foreach (DESPATCHADVICE inv in response.DESPATCHADVICE)
                {
                    FileOperations.SaveToDisk(inv.CONTENT.Value, inv.UUID, request.SEARCH_KEY.DIRECTION, request.REQUEST_HEADER.COMPRESSED, nameof(EI.Type.DESPATCH), nameof(EI.DocumentType.NULL), inv.ID);
                }
            }
            uuidList.Add(request.SEARCH_KEY.UUID);
        }


        [Test, Order(3)]
        public void GetDespatchAdviceStatus()
        {//Durumunu sorgulamak istediğimiz irsaliyelerin(gelen , giden veya taslak) uuid sini UUID alanına yazarak sorgulama yapılır.
            var request = new GetDespatchAdviceStatusRequest
            {

                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                // UUID = new String[] { "B537A0C8-9150-DE37-11AD-04D29850DA1B", "d802b4dc-9f6b-488d-8e4b-8f0cc24d8510" },
                UUID = uuidList.ToArray(),
            };
            GetDespatchAdviceStatusResponse response = _izibizClient.EDespatch().GetDespatchAdviceStatusResponse(request);
            Assert.NotNull(response.DESPATCHADVICE_STATUS);
        }

        [Test, Order(4)]
        public void MarkDespatchAdvice()
        {
            var request = new MarkDespatchAdviceRequest
            {
                REQUEST_HEADER = BaseAdapter.EDespatchWSRequestHeaderType(),
                MARK = new MarkDespatchAdviceRequestMARK
                {
                    value = nameof(EI.DocumentStatus.READ),
                    DESPATCHADVICEINFO = despatchAdviceInfo,
                }
            };

            MarkDespatchAdviceResponse response = _izibizClient.EDespatch().MarkDespatchAdviceResponse(request);
            Assert.Null(response.ERROR_TYPE);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE,0);
          //  GetDespatch();   //çekilen irsaliyeler listede bir daha gelmemesi için okundu  işaretlendi tekrar irsaliyeleri çekince farklı irsaliyeler gelir. Çalışabilmesi için getdespatchteki read included false olmalı
        }

    }
}


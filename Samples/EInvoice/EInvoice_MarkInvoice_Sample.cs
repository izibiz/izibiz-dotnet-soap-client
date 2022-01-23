using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Izibiz_dotnet_soap_client;
using Izibiz_dotnet_soap_client.Adapter;
using Izibiz_dotnet_soap_client.EInvoiceWS;
using NUnit.Framework;


namespace Samples.EInvoice
{
   // [Ignore("Waiting for Joe to fix his bugs", Until = "2024-07-31 12:00:00Z")]
    class EInvoice_MarkInvoice_Sample
    {
        private readonly IzibizClient _izibizClient = new IzibizClient();

        [Test]
        public void MarkInvoice_Read()
        { //GetInvoice servisi ile müşteri ortamına başarı alınan faturaların tekrar sorgulandığında listede
            //gelmemesi için MarkInvoice servisi ile alındı olarak işaretlenmeli.
            // Daha önce alındı olarak işaretlenen bir faturayı tekrar çekmeden önce UNREAD olarak gönderilebilir.
            //GetInvoice ile alınan faturaların id ve uuidsini vererek tekrar sorgulandığında gelmez.
            var request = new MarkInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                MARK = new MarkInvoiceRequestMARK
                {
                    value = MarkInvoiceRequestMARKValue.READ,
                    valueSpecified = true,
                    INVOICE = BaseAdapter.invoiceToMarkInvoice
                },

            };
            MarkInvoiceResponse response = _izibizClient.EInvoice().markInvoice(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
           // EInvoice_GetInvoice_Sample eInvoice_GetInvoice_Sample = new EInvoice_GetInvoice_Sample();
            //eInvoice_GetInvoice_Sample.GetInvoice_BetweenDate();//Alındı olarak işaretlendi faturalar getinvoice tekrar istekte bulunursak farklı okunmamış faturaları getirir.
        }

        [Test]
        public void MarkInvoice_UnRead()
        {

            var request = new MarkInvoiceRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),
                MARK = new MarkInvoiceRequestMARK
                {
                    value = MarkInvoiceRequestMARKValue.UNREAD,
                    valueSpecified = true,
                    INVOICE = BaseAdapter.invoiceToMarkInvoice,
                },

            };
            MarkInvoiceResponse response = _izibizClient.EInvoice().markInvoice(request);
            Assert.AreEqual(response.REQUEST_RETURN.RETURN_CODE, 0);
        }



    }
}


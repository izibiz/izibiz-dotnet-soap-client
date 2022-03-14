using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Izibiz;
using Izibiz.Adapter;
using Izibiz.EInvoiceWS;
using NUnit.Framework;


namespace Samples.EInvoice
{
   
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


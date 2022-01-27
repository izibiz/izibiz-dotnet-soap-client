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
  
    class A11EInvoice_GetInvoiceStatus_Sample
    { 
        private readonly IzibizClient _izibizClient = new IzibizClient();
        
        [Test]
        public void GetInvoiceStatusAll()
        {//Durumunu sorgulamak istediğimiz faturaların(gelen , giden veya taslak) uuid sini UUID alanına yazarak sorgulama yapılır.
            var request = new GetInvoiceStatusAllRequest
            {

                REQUEST_HEADER = BaseAdapter.EInvoiceWSRequestHeaderType(),  
                UUID = A10EInvoiceSample.uuidList.ToArray(),
            };
            GetInvoiceStatusAllResponse response = _izibizClient.EInvoice().getInvoiceStatusAll(request);
            Assert.IsTrue(response.INVOICE_STATUS.Length > 0);
        }
    }
}


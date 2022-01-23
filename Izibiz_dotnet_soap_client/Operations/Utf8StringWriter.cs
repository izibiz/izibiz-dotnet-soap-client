using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izibiz_dotnet_soap_client.Operations
{
   public class Utf8StringWriter:StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Izibiz.UblSerializer
{
   public abstract class ReceiptSerializer
    {
        public static XmlSerializerNamespaces GetXmlSerializerNamespace()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

          //      ns.Add("xmlns", "urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2");
            ns.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            ns.Add("n4", "http://www.altova.com/samplexml/other-namespace");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("schemaLocation", "urn:oasis:names:specification:ubl:schema:xsd:ReceiptAdvice-2 ../xsdrt/maindoc/UBL-ReceiptAdvice-2.1.xsd");
            
            return ns;
        }

    }
}

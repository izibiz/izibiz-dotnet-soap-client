using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Izibiz.UblSerializer
{
   public abstract class CreditNoteSerializer
    {
        public static XmlSerializerNamespaces GetXmlSerializerNamespace()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //      ns.Add("xmlns", "urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2");
            ns.Add("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            ns.Add("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            ns.Add("ccts", "urn:un:unece:uncefact:documentation:2");
            ns.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
            ns.Add("ef", "http://www.efatura.gov.tr/package-namespace");
            ns.Add("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            ns.Add("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
            ns.Add("sch", "http://purl.oclc.org/dsdl/schematron");
            ns.Add("sh", "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader");
            ns.Add("ubltr", "urn:oasis:names:specification:ubl:schema:xsd:TurkishCustomizationExtensionComponents");
            ns.Add("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
            ns.Add("urn", "urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2");
            ns.Add("d3p1", "http://www.w3.org/2005/05/xmlmime");
            ns.Add("xades", "http://uri.etsi.org/01903/v1.3.2#");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
          
            
            return ns;
        }

    }
}

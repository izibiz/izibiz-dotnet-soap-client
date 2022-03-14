using Izibiz.Ubl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace Izibiz.Operations
{
    public class XmlProcess
    {

        public static string getXsltFromXml(string xmlContent)
        {

            XDocument doc = XDocument.Parse(xmlContent);

            foreach (XElement element in doc.Descendants())
            {
                    if (element.Name.LocalName.Equals("EmbeddedDocumentBinaryObject")
                        && element.Parent.Name.LocalName.Equals("Attachment")
                      && element.HasAttributes && element.Attribute("filename") != null
                         && element.Attribute("filename").Value.ToLower().Contains(".xslt")
                         && element.Parent.Parent.Descendants().ToList().Where(e => e.Name.LocalName.Equals("DocumentType") &&
                    e.Value.Equals("XSLT")).FirstOrDefault() != null
                        )
                    {
                        if (element.Value.Contains("77u/"))//Gönderilen xslt içinde bu değer varsa siler.Bu değer boşluk olduğunu belirtir.
                    {
                        return element.Value.Replace("77u/", "");
                    }
                   return element.Value;                    
                    
                }
            }

            return null;
        }


        public static string xmlToHtml(string xslEncoded, string inputXml)
        {
           
            byte[] data = System.Convert.FromBase64String(xslEncoded);
            string decodedXslt = System.Text.UTF8Encoding.UTF8.GetString(data);
            
         
            using (StringReader srt = new StringReader(decodedXslt)) // xslInput is a string that contains xsl
            using (StringReader sri = new StringReader(inputXml)) // xmlInput is a string that contains xml
            {
                using (XmlReader xrt = XmlReader.Create(srt))
                using (XmlReader xri = XmlReader.Create(sri))
                {
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(xrt);
                    using (StringWriter sw = new StringWriter())
                    using (XmlWriter xwo = XmlWriter.Create(sw, xslt.OutputSettings)) // use OutputSettings of xsl, so it can be output as HTML
                    {
                        xslt.Transform(xri, xwo);
                        return sw.ToString();
                    }
                }
            }
        }

    }
}

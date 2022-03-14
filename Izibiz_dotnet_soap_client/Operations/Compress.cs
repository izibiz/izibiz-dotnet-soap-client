using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Izibiz.UblCreate;

namespace Izibiz.Operations
{
    public static class Compress
    {
       
        
        public static byte[] ZipFile(byte[] xmlContent)
        {

            MemoryStream zippedStream = new MemoryStream(xmlContent);
            using (ZipArchive archive = new ZipArchive(zippedStream))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    MemoryStream ms = new MemoryStream();
                    Stream zipStream = entry.Open();
                    zipStream.CopyTo(ms);
                    xmlContent = ms.ToArray();
                }
            }
            return xmlContent;
        }

        public static byte[] compressFile(string xmlContent)
        {
            
            byte[] xml = Encoding.UTF8.GetBytes(xmlContent);
            string value=null;
            XDocument doc = XDocument.Parse(xmlContent);

            foreach (XElement element in doc.Descendants())
            {
                if(element.Name.LocalName.ToString().Equals("ID")
                   && element.Parent.Name.LocalName.ToString().Equals("Invoice") )
                {
                    value=element.Value;
                    break;
                }
                else if (element.Name.LocalName.ToString().Equals("ID") && element.Parent.Name.LocalName.ToString().Equals("DespatchAdvice"))
                {
                    value = element.Value;
                    break;
                }
                else if (element.Name.LocalName.ToString().Equals("ID") && element.Parent.Name.LocalName.ToString().Equals("CreditNote"))
                {
                    value = element.Value;
                    break;
                }
                else if (element.Name.LocalName.ToString().Equals("ID") && element.Parent.Name.LocalName.ToString().Equals("ReceiptAdvice"))
                {
                    value = element.Value;
                    break;
                }
            }

                MemoryStream zipStream = new MemoryStream();
            using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                ZipArchiveEntry zipElaman = zip.CreateEntry(value + ".xml");
                Stream entryStream = zipElaman.Open();
                entryStream.Write(xml, 0, xml.Length);
                entryStream.Flush();
                entryStream.Close();
            }
            zipStream.Position = 0;
            return zipStream.ToArray();
        }

    }
}

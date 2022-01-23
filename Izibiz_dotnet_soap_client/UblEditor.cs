using Izibiz_dotnet_soap_client.Ubl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace Izibiz_dotnet_soap_client
{
    public class UblEditor
    {

        public const string INVOICE_UUID= "%INVOICE_UUID%";
        public const string INVOICE_ID = "%INVOICE_ID%";
        public static List<string> uuidList = new List<string>();

        public static byte[] ReplaceInvoice(string path)
        {
            Random rastgele = new Random();
            var ublToReplace = File.ReadAllText(path);
            string RandomUuid = Guid.NewGuid().ToString();
            ublToReplace = ublToReplace.Replace(INVOICE_UUID,RandomUuid);
            
            string letters = "ABCDEFGHIJKLMNOPRSTUVYZ";
            string rndLetter = "";
            string sayi ="000000000";
            for (int i = 0; i < 3; i++)
            {
                rndLetter += letters[rastgele.Next(letters.Length)];
            }
            
            string RandomId = rndLetter + DateTime.Now.Year+sayi;
            ublToReplace = ublToReplace.Replace(INVOICE_ID, RandomId);
            uuidList.Add(RandomUuid);
            return Encoding.UTF8.GetBytes(ublToReplace);
           
        }
        
        
    }
}

using Izibiz.Ubl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izibiz.Operations
{
    public class FileOperations
    {
        public static string FilePath(string type, string Direction)
        {
            string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (type == nameof(EI.Type.INVOICE))
            {
                return string.Format("{0}{1}", folderpath, Direction.Equals(nameof(EI.Direction.IN)) ? @"\GelenFatura\" : Direction.Equals(nameof(EI.Direction.OUT)) ? @"\GidenFatura\" : @"\TaslakFatura\");
            }
            else if (type == nameof(EI.Type.DESPATCH))
            {
                return string.Format("{0}{1}", folderpath, Direction.Equals(nameof(EI.Direction.IN)) ? @"\Gelenİrsaliye\" : Direction.Equals(nameof(EI.Direction.OUT)) ? @"\Gidenİrsaliye\" : @"\Taslakİrsaliye\");
            }
            else if (type == nameof(EI.Type.EARCHIVEINVOICE))
            {
                return string.Format("{0}{1}", folderpath, @"\ArşivFatura\");
            }
            else if (type == nameof(EI.Type.CREDITNOTE))
            {
                return string.Format("{0}{1}", folderpath, @"\E-Mühtahsil\");
            }
            else if (type == nameof(EI.Type.RECEIPT))
            {
                return string.Format("{0}{1}", folderpath, @"\EIrsaliyeYanıt\");
            }
            return null;
        }

        public static string Files(string proces,string type)
        {
            if(type == nameof(EI.Type.INVOICE))
            {
                FileYesNo(FilePaths.InvoicePath);
               return string.Format("{0}{1}", FilePaths.InvoicePath, proces.Equals(nameof(EI.FileName.LOADINVOICE)) ? @"\LoadInvoice\" : @"\SendInvoice\");
            }
            else if (type == nameof(EI.Type.DESPATCH))
            {
                FileYesNo(FilePaths.InvoicePath);
                return string.Format("{0}{1}", FilePaths.DespatchPath, proces.Equals(nameof(EI.FileName.LOADDESPATCH)) ? @"\LoadDespatch\" : @"\SendDespatch\");
            }
            else if (type == nameof(EI.Type.EARCHIVEINVOICE))
            {
                FileYesNo(FilePaths.InvoicePath);
                return string.Format("{0}{1}", FilePaths.ArchiveInvoicePath, proces.Equals(nameof(EI.FileName.EARCHIVEINVOICEDRAFT)) ? @"\DraftEArchive\" : @"\EArchive\");
            }
            else if (type == nameof(EI.Type.CREDITNOTE))
            {
                FileYesNo(FilePaths.InvoicePath);
                return string.Format("{0}{1}", FilePaths.CreditNotePath, proces.Equals(nameof(EI.FileName.CREDITNOTELOAD)) ? @"\DraftCreditNote\" : @"\CreditNote\");
            }
            else if (type == nameof(EI.Type.RECEIPT))
            {
                FileYesNo(FilePaths.InvoicePath);
                return string.Format("{0}{1}", FilePaths.ReceiptDespatchPath, proces.Equals(nameof(EI.FileName.RECEIPTLOAD)) ? @"\LoadReceipt\" : @"\SendReceipt\");
            }
            else if (type == nameof(EI.Type.SMM))
            {
                FileYesNo(FilePaths.InvoicePath);
                return string.Format("{0}{1}", FilePaths.SmmPath, proces.Equals(nameof(EI.FileName.SMMSEND)) ? @"\SmmSend\" : @"\SmmLoad\");
            }

            return null;
        }


        public static void FileYesNo(String path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


        public static void SendAndLoadSaveToDisk(string process, string ID, string UUID, byte[] zipFile,string type)//Send ve load invoicelarda istek başarılı ise gönderilen(programdaki xml dosyası) diske yazmak için
        {
            string filePath = Files(process,type);

            FileYesNo(filePath);

            File.WriteAllBytes(filePath + ID + "-" + UUID + ".zip", zipFile);
        }


        public static void SaveToDisk(byte[] xmlContent, string Filename_UUID, string Direction, string Compressed, string type, string documentType, string Filename_ID = "")
        {

            var filePath = FilePath(type, Direction);

            FileYesNo(filePath);

             if (type != nameof(EI.Type.EARCHIVEINVOICE))
            {
                xmlContent = DocumentUnzip(xmlContent, Compressed);
            }
            if (type == nameof(EI.Type.EARCHIVEINVOICE))
            {
                xmlContent = ArchiveUnzip(xmlContent, Compressed, documentType);
            }

            string zippp = Convert.ToBase64String(xmlContent);
            if (documentType == "NULL")
            {
                Debug.WriteLine(filePath + Filename_ID + Filename_UUID + ".xml");
                string decodedString = Encoding.UTF8.GetString(xmlContent);
                File.WriteAllText(filePath + Filename_ID + "-" + Filename_UUID + ".xml", decodedString);

                string xslt = null;
                try
                {
                    xslt = XmlProcess.getXsltFromXml(decodedString);
                }
                catch
                {
                }
                finally
                {
                    if (xslt == null || xslt == "")
                    {
                        if (type== nameof(EI.Type.INVOICE)) {
                            xslt = Xslt.xsltGibInvoice;
                        }else if (type == nameof(EI.Type.DESPATCH))
                        {
                            xslt = Xslt.xsltGibDespatch;
                        }
                        else if (type == nameof(EI.Type.EARCHIVEINVOICE))
                        {
                            xslt = Xslt.xsltGibArchive;
                        }
                        else if (type == nameof(EI.Type.CREDITNOTE))
                        {
                            xslt = Xslt.xsltGibCreditNote;
                        }
                        else if (type == nameof(EI.Type.RECEIPT))
                        {
                            xslt = Xslt.xsltGibReceipt;
                        }                        
                    }
                }
                var temp = XmlProcess.xmlToHtml(xslt, decodedString);
                File.WriteAllText(filePath + Filename_ID + "-" + Filename_UUID + ".html", temp);
            }
            if (documentType != "NULL")
            {
                string file_extention;
                if (Filename_ID != "")
                {
                    file_extention = filePath + Filename_ID + "-" + Filename_UUID + "." + documentType.ToLower();
                }
                else
                {
                    file_extention = filePath + Filename_UUID + "." + documentType.ToLower();
                }
                File.WriteAllBytes(file_extention, xmlContent);
            }
        }


        public static byte[] DocumentUnzip(byte[] xmlContent, string compressed)
        {
            if (compressed.Equals(nameof(EI.YesNo.Y)))
            {
                return xmlContent = Compress.ZipFile(xmlContent);
            }
            return xmlContent;
        }

        public static byte[] ArchiveUnzip(byte[] xmlContent, string compressed, string documentType)
        {
            if (compressed.Equals(nameof(EI.YesNo.Y)) && documentType == "HTML")
            {
                return xmlContent = Compress.ZipFile(Compress.ZipFile(xmlContent));
            }
            else if (documentType == "PDF")
            {
                return xmlContent = Compress.ZipFile(xmlContent);
            }
            return xmlContent;
        }


    }
}

using System;
using System.Collections.Generic;


namespace Izibiz.Operations
{
    public static class FilePaths
    {
        public static string InvoicePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\invoice\";
        public static string DespatchPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Despatch\";
        public static string ArchiveInvoicePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ArchiveInvoice\";
        public static string CreditNotePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CreditNote\";
        public static string ReceiptDespatchPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ReceiptDespatch\";
        public static string SmmPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\SMM\";

    }
}
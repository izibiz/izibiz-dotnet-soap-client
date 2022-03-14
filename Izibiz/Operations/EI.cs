using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Izibiz.Operations
{
    public partial class EI
    {
        public enum Direction
        {
            IN,
            OUT,
            DRAFT
        }

        public enum DocumentType
        {
            PDF,
            XML,
            HTML,
            XSLT,
            NULL
        }

        public enum YesNo
        {
            N,
            Y,
        }

        public enum Status
        {
            RED,
            KABUL
        }

        public enum Type
        {
            EARCHIVEINVOICE,
            INVOICE,
            DESPATCH,
            CREDITNOTE,
            RECEIPT,
            SMM
        }

        public enum FileName
        {
            LOADINVOICE,
            SENDINVOICE,
            EARCHIVEINVOICE,
            EARCHIVEINVOICEDRAFT,
            SENDDESPATCH,
            LOADDESPATCH,
            CREDITNOTESEND,
            CREDITNOTELOAD,
            RECEIPTSEND,
            RECEIPTLOAD,
            SMMSEND,
            SMMLOAD,
        }

        public enum DocumentStatus
        {
            READ,
            UNREAD
        }

    }
}

using Izibiz_dotnet_soap_client.Adapter;

namespace Izibiz_dotnet_soap_client
{
    public class IzibizClient
    {
        private readonly AuthAdapter _authAdapter;
        private readonly EInvoiceAdapter _eInvoiceAdapter;
        private readonly EArchiveInvoiceAdapter _eInvoiceArchiveAdapter;
        private readonly EDespatchAdapter _eDespatchAdapter;
        private readonly CreditNoteAdapter _creditNoteAdapter;

        public IzibizClient() {
            _authAdapter = new AuthAdapter();
            _eInvoiceAdapter = new EInvoiceAdapter();
            _eInvoiceArchiveAdapter = new EArchiveInvoiceAdapter();
            _eDespatchAdapter = new EDespatchAdapter();
            _creditNoteAdapter = new CreditNoteAdapter();

        }


        public AuthAdapter Auth()
        {
            return _authAdapter;
        }

        public EInvoiceAdapter EInvoice()
        {
            return _eInvoiceAdapter;
        }
        public EArchiveInvoiceAdapter EInvoiceArchive()
        {
            return _eInvoiceArchiveAdapter;
        }
        public EDespatchAdapter EDespatch()
        {
            return _eDespatchAdapter;
        }

        public CreditNoteAdapter CreditNote()
        {
            return _creditNoteAdapter;
        }


    }
}
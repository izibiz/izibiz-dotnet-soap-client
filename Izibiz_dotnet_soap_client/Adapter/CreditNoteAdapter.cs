using Izibiz.CreditNoteWS;

namespace Izibiz.Adapter
{
    public class CreditNoteAdapter
    {


        public SendCreditNoteResponse SendCreditNoteResponse(SendCreditNoteRequest sendCreditNoteRequest)
        {
            CreditNoteServicePortClient creditNoteServicePortClient = new CreditNoteServicePortClient();
            return creditNoteServicePortClient.SendCreditNote(sendCreditNoteRequest);
        }

        public GetCreditNoteResponse GetCreditNoteResponse(GetCreditNoteRequest getCreditNoteRequest)
        {
            CreditNoteServicePortClient creditNoteServicePortClient = new CreditNoteServicePortClient();
            return creditNoteServicePortClient.GetCreditNote(getCreditNoteRequest);
        }

        public GetCreditNoteStatusResponse GetGetCreditNoteStatusResponse(GetCreditNoteStatusRequest getCreditNoteStatusRequest)
        {
            CreditNoteServicePortClient creditNoteServicePortClient = new CreditNoteServicePortClient();
            return creditNoteServicePortClient.GetCreditNoteStatus(getCreditNoteStatusRequest);
        }

        public LoadCreditNoteResponse LoadCreditNoteResponse(LoadCreditNoteRequest loadCreditNoteRequest)
        {
            CreditNoteServicePortClient creditNoteServicePortClient = new CreditNoteServicePortClient();
            return creditNoteServicePortClient.LoadCreditNote(loadCreditNoteRequest);
        }


        public MarkCreditNoteResponse MarkCreditNoteResponse(MarkCreditNoteRequest markCreditNoteRequest)
        {
            CreditNoteServicePortClient creditNoteServicePortClient = new CreditNoteServicePortClient();
            return creditNoteServicePortClient.MarkCreditNote(markCreditNoteRequest);
        }

    }
}
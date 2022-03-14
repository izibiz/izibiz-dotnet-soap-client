using Izibiz.EIrsaliyeWS;

namespace Izibiz.Adapter
{
    public class EDespatchAdapter
    {


        public SendDespatchAdviceResponse SendDespatchAdviceResponse(SendDespatchAdviceRequest sendDespatchAdviceRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.SendDespatchAdvice(sendDespatchAdviceRequest);
        }

        public GetDespatchAdviceResponse GetDespatchAdviceResponse(GetDespatchAdviceRequest getDespatchAdviceRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.GetDespatchAdvice(getDespatchAdviceRequest);
        }

        public GetDespatchAdviceStatusResponse GetDespatchAdviceStatusResponse(GetDespatchAdviceStatusRequest getDespatchAdviceStatusRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.GetDespatchAdviceStatus(getDespatchAdviceStatusRequest);
        }

        public LoadDespatchAdviceResponse LoadDespatchAdviceResponse(LoadDespatchAdviceRequest loadDespatchAdviceRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.LoadDespatchAdvice(loadDespatchAdviceRequest);
        }

        public MarkDespatchAdviceResponse MarkDespatchAdviceResponse(MarkDespatchAdviceRequest markDespatchAdviceRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.MarkDespatchAdvice(markDespatchAdviceRequest);
        }

        public SendReceiptAdviceResponse SendReceiptAdviceResponse(SendReceiptAdviceRequest sendReceiptAdviceRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.SendReceiptAdvice(sendReceiptAdviceRequest);
        }

        public LoadReceiptAdviceResponse LoadReceiptAdviceResponse(LoadReceiptAdviceRequest loadReceiptAdviceRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.LoadReceiptAdvice(loadReceiptAdviceRequest);
        }

        public GetReceiptAdviceStatusResponse GetReceiptAdviceStatusResponse(GetReceiptAdviceStatusRequest getReceiptAdviceStatusRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.GetReceiptAdviceStatus(getReceiptAdviceStatusRequest);
        }

        public GetReceiptAdviceResponse GetReceiptAdviceResponse(GetReceiptAdviceRequest getReceiptAdviceRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.GetReceiptAdvice(getReceiptAdviceRequest);
        }

        public MarkReceiptAdviceResponse MarkReceiptAdviceResponse(MarkReceiptAdviceRequest markReceiptAdviceRequest)
        {
            EIrsaliyeServicePortClient eIrsaliyePortClient = new EIrsaliyeServicePortClient();
            return eIrsaliyePortClient.MarkReceiptAdvice(markReceiptAdviceRequest);
        }

    }
}
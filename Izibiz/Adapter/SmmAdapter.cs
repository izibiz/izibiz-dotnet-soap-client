using Izibiz.SmmWs;

namespace Izibiz.Adapter
{
    public class SmmAdapter
    {

        public SendSmmResponse SendSmmResponse(SendSmmRequest sendSmmRequest)
        {
            SmmServicePortClient smmWSPort = new SmmServicePortClient();
            return smmWSPort.SendSmm(sendSmmRequest);
        }

        public GetSmmResponse GetSmmResponse(GetSmmRequest getSmmRequest)
        {
            SmmServicePortClient smmWSPort = new SmmServicePortClient();
            return smmWSPort.GetSmm(getSmmRequest);
        }

        public GetSmmStatusResponse GetSmmStatusResponse(GetSmmStatusRequest getSmmStatusRequest)
        {
            SmmServicePortClient smmWSPort = new SmmServicePortClient();
            return smmWSPort.GetSmmStatus(getSmmStatusRequest);
        }

        public GetSmmReportResponse GetSmmReportResponse(GetSmmReportRequest getSmmReportRequest)
        {
            SmmServicePortClient smmWSPort = new SmmServicePortClient();
            return smmWSPort.GetSmmReport(getSmmReportRequest);
        }

    }
}
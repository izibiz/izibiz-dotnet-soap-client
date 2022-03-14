using Izibiz.EArchiveInvoiceWS;

namespace Izibiz.Adapter
{
    public class EArchiveInvoiceAdapter
    {
      

        public ArchiveInvoiceExtendedResponse WritetoArchiveExtendedd(ArchiveInvoiceExtendedRequest writetoArchiveExtendedReq)
        {
            EFaturaArchivePortClient eFaturaArchivePortClient = new EFaturaArchivePortClient();
            return eFaturaArchivePortClient.WriteToArchiveExtended(writetoArchiveExtendedReq);           
        }


        public ArchiveInvoiceReadResponse ArchiveRead(ArchiveInvoiceReadRequest archiveread)
        {
            EFaturaArchivePortClient eFaturaArchivePortClient = new EFaturaArchivePortClient();
            return eFaturaArchivePortClient.ReadFromArchive(archiveread);
        }

        public GetEArchiveInvoiceStatusResponse GetEArchiveInvoiceStatusResponse(GetEArchiveInvoiceStatusRequest getEArchiveInvoiceStatusRequest)
        {
            EFaturaArchivePortClient eFaturaArchivePortClient = new EFaturaArchivePortClient();
            return eFaturaArchivePortClient.GetEArchiveInvoiceStatus(getEArchiveInvoiceStatusRequest);
        }

        public GetEArchiveInvoiceListResponse GetEArchiveInvoiceList(GetEArchiveInvoiceListRequest getEArchiveInvoiceListRequest)
        {
            EFaturaArchivePortClient eFaturaArchivePortClient = new EFaturaArchivePortClient();
            return eFaturaArchivePortClient.GetEArchiveInvoiceList(getEArchiveInvoiceListRequest);
        }

        public CancelEArchiveInvoiceResponse CancelEArchiveInvoiceResponse(CancelEArchiveInvoiceRequest cancelEArchiveInvoiceRequest)
        {
            EFaturaArchivePortClient eFaturaArchivePortClient = new EFaturaArchivePortClient();
            return eFaturaArchivePortClient.CancelEArchiveInvoice(cancelEArchiveInvoiceRequest);
        }

        public GetEmailEarchiveInvoiceResponse GetEmailEarchiveInvoiceResponse(GetEmailEarchiveInvoiceRequest getEmailEarchiveInvoiceRequest)
        {
            EFaturaArchivePortClient eFaturaArchivePortClient = new EFaturaArchivePortClient();
            return eFaturaArchivePortClient.GetEmailEarchiveInvoice(getEmailEarchiveInvoiceRequest);
        }

        public GetEArchiveReportResponse GetEArchiveReportResponse(GetEArchiveReportRequest getEArchiveReportRequest)
        {
            EFaturaArchivePortClient eFaturaArchivePortClient = new EFaturaArchivePortClient();
            return eFaturaArchivePortClient.GetEArchiveReport(getEArchiveReportRequest);
        }

        public ReadEArchiveReportResponse ReadEArchiveReportResponse(ReadEArchiveReportRequest readEArchiveReportRequest)
        {
            EFaturaArchivePortClient eFaturaArchivePortClient = new EFaturaArchivePortClient();
            return eFaturaArchivePortClient.ReadEArchiveReport(readEArchiveReportRequest);
        }

    }
}
using Izibiz;
using Izibiz.Ubl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UblReceipt;


namespace Izibiz.UblCreate
{
    public class BaseReceiptUBL
    {
        public ReceiptAdviceType baseReceiptUBL { get; protected set; }
        private List<ReceiptLineType> listReceiptLine = new List<ReceiptLineType>();
        public List<DocumentReferenceType> docRefList = new List<DocumentReferenceType>();


        public BaseReceiptUBL()
        {
            baseReceiptUBL = new ReceiptAdviceType();
            createDespatchHeader();
            DespatchDocumentReference();
            createSignature();
            CreateDelivery();
            DespatchSupplierParty();
            Shipment();
            addReceiptLine();


        }

        public void createDespatchHeader()
        {
            Random random = new Random();
            var receiptId = random.Next(100000000, 999999999);
            baseReceiptUBL.UBLVersionID = new UBLVersionIDType { Value = "2.1" };
            baseReceiptUBL.CustomizationID = new CustomizationIDType { Value = "TR1.2.1" };
            //uluslararası fatura standardı 2.1
            baseReceiptUBL.ProfileID = new ProfileIDType { Value = "TEMELIRSALIYE" };
            baseReceiptUBL.ID = new IDType { Value = "RES2022"+receiptId };
            baseReceiptUBL.CopyIndicator = new CopyIndicatorType { Value = false };
            baseReceiptUBL.UUID = new UUIDType { Value = Guid.NewGuid().ToString() };
            baseReceiptUBL.IssueDate = new IssueDateType { Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) };
            baseReceiptUBL.IssueTime = new IssueTimeType { Value = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss.")) };
            baseReceiptUBL.ReceiptAdviceTypeCode = new ReceiptAdviceTypeCodeType { Value = "SEVK" };
            baseReceiptUBL.Note = new NoteType[] { new NoteType { Value = "Denemedir" } };
            baseReceiptUBL.LineCountNumeric = new LineCountNumericType { Value =1 };
        }


        public static PartyType createParty(string partyName, string cityName, string streetName, string citySub, string postalzone, string country, string taxScheme, string telephone, string telefax, string mail, string VKN)
        {
            PartyType party = new PartyType();
            party.WebsiteURI = new WebsiteURIType { Value = "https://www.izibiz.com.tr" };
            party.PartyName = new PartyNameType { Name = new NameType1 { Value = partyName } };
            party.PostalAddress = new AddressType
            {
                StreetName = new StreetNameType { Value = streetName },
                CitySubdivisionName = new CitySubdivisionNameType { Value = citySub },
                CityName = new CityNameType { Value = cityName },
                PostalZone = new PostalZoneType { Value = postalzone },
                Country = new CountryType { Name = new NameType1 { Value = country } }
            };
            party.PartyIdentification = new[]
                        {
                            new PartyIdentificationType
                            {
                                ID = new IDType { schemeID = "VKN", Value = VKN }
                            }
                        };
            party.PartyTaxScheme = new PartyTaxSchemeType
            {
                TaxScheme = new TaxSchemeType
                {
                    Name = new NameType1 { Value = taxScheme }
                }
            };
            party.Contact = new ContactType();
            party.Contact.Telephone = new TelephoneType { Value = telephone };
            party.Contact.Telefax = new TelefaxType { Value = telefax };
            party.Contact.ElectronicMail = new ElectronicMailType { Value = mail };

            return party;
        }

        public void DespatchDocumentReference()
        {
            var documentReference = new DocumentReferenceType();
            documentReference.ID = new IDType { Value = "293f0d1c-836e-4d26-a055-2e443b35cd4b" };//yanıt verilen irsaliyenin uıque ıdsi yazılmalıdır.
            documentReference.IssueDate = new IssueDateType { Value = DateTime.Parse("2022-01-18") };//yanıt verilecek irsaliyenin tarihi
            baseReceiptUBL.DespatchDocumentReference = documentReference;
        }

        public void addAdditionalDocumentReference()
        {

            var recRef = new DocumentReferenceType();
            recRef.ID = new IDType { Value = "KEI2022000000001" };//Hangi irsaliyeye yanıt ise o ıd yazılır.
            recRef.IssueDate = baseReceiptUBL.IssueDate;
            recRef.DocumentType = new DocumentTypeType { Value = "DespatchAdviceID" };
            recRef.DocumentTypeCode = new DocumentTypeCodeType { Value = "DespatchAdviceID" };
            docRefList.Add(recRef);

            baseReceiptUBL.AdditionalDocumentReference = docRefList.ToArray();
        }

        public void createSignature()
        {//irsaliyedeki customer alanı
            var signature = new[]
            {
                new SignatureType
                {
                    ID = new IDType { schemeID = "VKN_TCKN", Value = "4840847211" },
                    SignatoryParty = BaseReceiptUBL.createParty("akbil teknolojileri","ANKARA","DENEME ADRES BİLGİLERİ","KAHRAMANKAZAN","","TÜRKİYE","KURUMLAR","(800) 811-1199","","yazilim@izibiz.com.tr","4840847211"),
                                                                   
                    DigitalSignatureAttachment=new AttachmentType
                    {
                        ExternalReference=new ExternalReferenceType
                        {
                            URI=new URIType
                            {
                                Value="#Signature_"+baseReceiptUBL.ID,
                            }
                        }
                    }
                }
             };

            baseReceiptUBL.Signature = signature;
        }

        public void DespatchSupplierParty()
        {
            var despatchSupplierParty = new SupplierPartyType //göndericinin irsaliye üzerindeki bilgileri
            {
                Party = BaseReceiptUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ ANONİM ŞİRKETİ", "ISTANBUL", "Yıldız Teknik Üniversitesi Teknoloji Geliştirme Bölgesi D2 Blok Z07", "MALTEPE", "34220", "TR", "KÜÇÜKYALI", "8508111199", "", "operasyonekibi@izibiz.com.tr", "4840847211"),
                DespatchContact = new ContactType
                {
                    //  ID = new IDType { Value = "" },
                    Name = new NameType1 { Value = "" }
                }
            };
            baseReceiptUBL.DespatchSupplierParty = despatchSupplierParty;
        }

        public void CreateDelivery()
        {
            var customerPartyType = new CustomerPartyType //Alıcının irsaliye üzerindeki bilgileri
            {
                Party = BaseReceiptUBL.createParty("akbil teknolojileri", "ANKARA", "DENEME ADRES BİLGİLERİ", "KAHRAMANKAZAN", "", "TÜRKİYE", "KURUMLAR", "(800) 811-1199", "", "yazilim@izibiz.com.tr", "4840847211"),
            };
            baseReceiptUBL.DeliveryCustomerParty = customerPartyType;

        }

        public void Shipment()
        {
            var shipment = new ShipmentType
            {


                ID = new IDType { },

                Delivery = new DeliveryType
                {

                    ActualDeliveryDate = new ActualDeliveryDateType { Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) },
                    ActualDeliveryTime = new ActualDeliveryTimeType { Value = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss")) }
                }
            };
            baseReceiptUBL.Shipment = shipment;
        }


        public static ReceiptLineType Linetype(string ıd, string orderline, string linereference, string name)
        {
            ReceiptLineType receiptLine = new ReceiptLineType();
            receiptLine.ID = new IDType { Value = ıd };
            receiptLine.Note = new NoteType[] { };
            receiptLine.ReceivedQuantity = new ReceivedQuantityType { unitCode = "C62", Value = 1.0m };
            receiptLine.OrderLineReference = new OrderLineReferenceType { LineID = new LineIDType { Value = orderline } };
            receiptLine.DespatchLineReference = new LineReferenceType { LineID = new LineIDType { Value = linereference } };

            receiptLine.Item = new ItemType
            {
                Name = new NameType1 { Value = name },
                SellersItemIdentification = new ItemIdentificationType { ID = new IDType { } },
            };

            receiptLine.Shipment = new ShipmentType[]
                 {
                 new ShipmentType
                 {
                     ID=new IDType{},
                    TotalGoodsItemQuantity=new TotalGoodsItemQuantityType { Value=1.0M},
                 }
                 };


            return receiptLine;
        }

        public void addReceiptLine()
        {
      
var receiptLine = BaseReceiptUBL.Linetype("1", "1", "", "DEFTER");
            //  var receiptLine2 = BaseReceiptUBL.Linetype("2", "2", "2", "K2000 MAT KROM 16 KALİBRE 890 AAAAA FSASF FASFA");
            listReceiptLine.Add(receiptLine);
            //  listReceiptLine.Add(receiptLine2);
            baseReceiptUBL.ReceiptLine = listReceiptLine.ToArray();
        }
    }
}
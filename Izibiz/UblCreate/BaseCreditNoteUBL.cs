using Izibiz;
using Izibiz.Ubl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UblCreditNote;


namespace Izibiz.UblCreate
{
    public class BaseCreditNoteUBL
    {
        public CreditNoteType baseCreditNoteUBL { get; protected set; }
        private List<CreditNoteLineType> listCreditNoteLine = new List<CreditNoteLineType>();
        public List<DocumentReferenceType> docRefList = new List<DocumentReferenceType>();


        public BaseCreditNoteUBL()
        {
            baseCreditNoteUBL = new CreditNoteType();
            createCreditNotehHeader();
            createSignature();
            SupplierParty();
            CustomerParty();
            CreateDelivery();
            TaxTotal();
            legalMonetaryTotal();
            addCreditNoteLine();


        }

        public void createCreditNotehHeader()
        {
            Random random = new Random();
            var invoiceId= random.Next(100000000,999999999);
            baseCreditNoteUBL.UBLVersionID = new UBLVersionIDType { Value = "2.1" };
            baseCreditNoteUBL.CustomizationID = new CustomizationIDType { Value = "TR1.2" };
            baseCreditNoteUBL.ProfileID = new ProfileIDType { Value = "EARSIVBELGE" };
            baseCreditNoteUBL.ID = new IDType { Value = "MUH2022"+ invoiceId };
            baseCreditNoteUBL.CopyIndicator = new CopyIndicatorType { Value = false };
            baseCreditNoteUBL.UUID = new UUIDType { Value = Guid.NewGuid().ToString() };
            baseCreditNoteUBL.IssueDate = new IssueDateType { Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) };
            baseCreditNoteUBL.IssueTime = new IssueTimeType { Value = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss.")) };
            baseCreditNoteUBL.CreditNoteTypeCode = new CreditNoteTypeCodeType { Value = "MUSTAHSILMAKBUZ" };
            baseCreditNoteUBL.Note = new NoteType[] { new NoteType { Value = "Yazi ile Yekun:Yalnız YirmiDört TL Elli Kuruş." } };
            baseCreditNoteUBL.Note = new NoteType[] { new NoteType { Value = "Bilgi:e-Müstahsil izni kapsamında oluşturulmuştur." } };
            baseCreditNoteUBL.Note = new NoteType[] { new NoteType { Value = "Bilgi:e-Müstahsil izni kapsamında elektronik ortamda iletilmiştir." } };
            baseCreditNoteUBL.Note = new NoteType[] { new NoteType { Value = "CustomerEmail:deneme@deneme.com" } };
            baseCreditNoteUBL.DocumentCurrencyCode = new DocumentCurrencyCodeType { Value = "TRY" };
            baseCreditNoteUBL.LineCountNumeric = new LineCountNumericType { Value = 1 };
        }


        public static PartyType createParty(string partyName,string StreetName,string BuildingNumber,string CitySubD,string District, string cityName,string Postalzone, string Country,string VKN)
        {
            PartyType party = new PartyType();
            party.PartyName = new PartyNameType { Name = new NameType1 { Value = partyName } };
            party.PostalAddress = new AddressType
            {
                ID = new IDType { },
                Room = new RoomType { },
                StreetName = new StreetNameType { Value = StreetName},
                BuildingNumber = new BuildingNumberType { Value = BuildingNumber },
                CitySubdivisionName = new CitySubdivisionNameType { Value = CitySubD },
                District = new DistrictType { Value = District },
                CityName = new CityNameType { Value = cityName },
                PostalZone = new PostalZoneType { Value = Postalzone },
                Country = new CountryType { Name = new NameType1 { Value = Country } }
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
                    Name = new NameType1 { Value = "DAVUTPAŞA" }
                }
            };

            return party;
        }



        public void createSignature()
        {
            var signature = new[]
            {
                new SignatureType
                {
                    ID = new IDType { schemeID = "VKN_TCKN", Value = "4840847211" },
                    SignatoryParty = BaseCreditNoteUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ","Yıldız Teknik üniversitesi Teknopark B Blok Kat:2","412","Esenler","Davutpaşa","İstanbul","34065","Turkey","4840847211"),
                                                        
                    DigitalSignatureAttachment=new AttachmentType
                    {
                        ExternalReference=new ExternalReferenceType
                        {
                            URI=new URIType
                            {
                                Value="#Signature_"+baseCreditNoteUBL.ID,
                            }
                        }
                    }
                }
             };

            baseCreditNoteUBL.Signature = signature;
        }

        public void SupplierParty()
        {
            var supplierParty = new SupplierPartyType //göndericinin irsaliye üzerindeki bilgileri
            {
                Party = BaseCreditNoteUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ", "Yıldız Teknik üniversitesi Teknopark B Blok Kat:2", "412", "Esenler", "Davutpaşa", "İstanbul", "34065", "Turkey", "4840847211"),
            };
            baseCreditNoteUBL.AccountingSupplierParty = supplierParty;
        }

        public void CustomerParty()
        {
            var customerParty = new CustomerPartyType //alıcının irsaliye üzerindeki bilgileri
            {
                Party = BaseCreditNoteUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ", "Yıldız Teknik üniversitesi Teknopark B Blok Kat:2", "412", "Esenler", "Davutpaşa", "İstanbul", "34065", "Turkey", "4840847211"),
            };
            baseCreditNoteUBL.AccountingCustomerParty = customerParty;
        }

        public void CreateDelivery()
        {
            var delivery = new DeliveryType[] //Alıcının irsaliye üzerindeki bilgileri
            {
                new DeliveryType{
                ActualDeliveryDate=new ActualDeliveryDateType { Value= Convert.ToDateTime("2022-01-09"),
                } }
            };
            baseCreditNoteUBL.Delivery = delivery;

        }
    

        public void TaxTotal()
        {
            var taxTotal = new TaxTotalType[]
            {
                new TaxTotalType
                {
                    TaxAmount=new TaxAmountType{currencyID="TRY",Value=0.5M},
                    TaxSubtotal=new TaxSubtotalType[]
                    {
                        new TaxSubtotalType
                        {
                            TaxableAmount=new TaxableAmountType{currencyID="TRY",Value=25.0M},
                            TaxAmount=new TaxAmountType{currencyID="TRY",Value=0.5M},
                            CalculationSequenceNumeric=new CalculationSequenceNumericType{Value=1.0m},
                            Percent=new PercentType1{format="%",Value=2.0m},
                            TaxCategory=new TaxCategoryType
                            {
                                TaxScheme=new TaxSchemeType
                                {
                                    Name=new NameType1{languageLocaleID="bağkur",Value="Bağkur(SGK_PRIM)"},
                                    TaxTypeCode=new TaxTypeCodeType{Value="SGK_PRIM"},
                                }
                            }
                        }
                    }
                }
            };
            baseCreditNoteUBL.TaxTotal = taxTotal;
        }


        public void legalMonetaryTotal()
        {
            var monetaryTotal = new MonetaryTotalType
            {
                LineExtensionAmount = new LineExtensionAmountType { currencyID = "TRY", Value = 25.0M },
                TaxExclusiveAmount = new TaxExclusiveAmountType { currencyID = "TRY", Value = 25.0M },
                TaxInclusiveAmount = new TaxInclusiveAmountType { currencyID = "TRY", Value = 24.5M },
                PayableAmount = new PayableAmountType { currencyID = "TRY", Value = 24.5M },
            };
            baseCreditNoteUBL.LegalMonetaryTotal = monetaryTotal;
        }

        public void addCreditNoteLine()
        {
            var creditNoteLine = new CreditNoteLineType
            {
                ID = new IDType { Value = "1" },
                Note = new NoteType[] { },
                CreditedQuantity = new CreditedQuantityType { unitCode = "C62", Value = 1.0m },
                LineExtensionAmount = new LineExtensionAmountType { currencyID = "TRY", Value = 25.0M },
                OrderLineReference = new OrderLineReferenceType[] { new OrderLineReferenceType { LineID = new LineIDType { Value = "1" } } },
                TaxTotal = new TaxTotalType[]
                {

                new TaxTotalType
                {
                    TaxAmount=new TaxAmountType{currencyID="TRY",Value=-0.5M},
                    TaxSubtotal=new TaxSubtotalType[]
                    {
                        new TaxSubtotalType
                        {
                            TaxableAmount=new TaxableAmountType{currencyID="TRY",Value=25.0M},
                            TaxAmount=new TaxAmountType{currencyID="TRY",Value=0.5M},
                            CalculationSequenceNumeric=new CalculationSequenceNumericType{Value=1.0m},
                            Percent=new PercentType1{format="%",Value=2.0m},
                            TaxCategory=new TaxCategoryType
                            {
                                TaxScheme=new TaxSchemeType
                                {
                                    Name=new NameType1{languageLocaleID="bağkur",Value="Bağkur(SGK_PRIM)"},
                                    TaxTypeCode=new TaxTypeCodeType{Value="SGK_PRIM"},
                                }
                            }
                        }
                    }
                }
                },
                Item = new ItemType
                {
                    Name = new NameType1 { Value = "Müstahsil" },
                    SellersItemIdentification = new ItemIdentificationType { ID = new IDType { Value = "011" } }
                },
               Price=new PriceType
               {
                   PriceAmount=new PriceAmountType { currencyID = "TRY", Value = 25.0M }
               }
            };
            listCreditNoteLine.Add(creditNoteLine);
            baseCreditNoteUBL.CreditNoteLine = listCreditNoteLine.ToArray();
        }
    }
}
using Izibiz;
using Izibiz.Ubl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UblDespatch;


namespace Izibiz.UblCreate
{
    public class BaseDespatchUBL
    {
        public DespatchAdviceType baseDespatchUBL { get; protected set; }
        private List<DespatchLineType> listDespatchLine = new List<DespatchLineType>();
        public List<DocumentReferenceType> docRefList = new List<DocumentReferenceType>();


        public BaseDespatchUBL()
        {
            baseDespatchUBL = new DespatchAdviceType();
            createDespatchHeader();
            createSignature();
            DespatchSupplierParty();
            CreateDelivery();
            Shipment();
            addDespatchLine();


        }

        public void createDespatchHeader()
        {
            Random random = new Random();
            var despatchId = random.Next(100000000, 999999999);
            baseDespatchUBL.UBLVersionID = new UBLVersionIDType { Value = "2.1" };
            baseDespatchUBL.CustomizationID = new CustomizationIDType { Value = "TR1.2.1" };
            //uluslararası fatura standardı 2.1
            baseDespatchUBL.CustomizationID = new CustomizationIDType { Value = "TR1.2" };
            baseDespatchUBL.ProfileID = new ProfileIDType { Value = "TEMELIRSALIYE" };
            baseDespatchUBL.ID = new IDType { Value = "IRS2022"+ despatchId };
            baseDespatchUBL.CopyIndicator = new CopyIndicatorType { Value = false };
            baseDespatchUBL.UUID = new UUIDType { Value = Guid.NewGuid().ToString() };
            baseDespatchUBL.IssueDate = new IssueDateType { Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) };
            baseDespatchUBL.IssueTime = new IssueTimeType { Value = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss.")) };
            baseDespatchUBL.DespatchAdviceTypeCode = new DespatchAdviceTypeCodeType { Value = "SEVK" };
            baseDespatchUBL.Note = new NoteType[] { new NoteType { Value = "Denemedir" } };
            baseDespatchUBL.LineCountNumeric = new LineCountNumericType { Value = 1 };
        }


        public static PartyType createParty(string partyName, string cityName, string telephone, string telefax, string mail, string VKN)
        {
            PartyType party = new PartyType();
            party.WebsiteURI = new WebsiteURIType { Value = "https://www.izibiz.com.tr" };
            party.PartyName = new PartyNameType { Name = new NameType1 { Value = partyName } };
            party.PostalAddress = new AddressType
            {
                StreetName = new StreetNameType { Value = "Yıldız Teknik üniversitesi Teknopark B Blok Kat:2 No:412 Davutpaşa -Esenler /İstanbul" },
                CitySubdivisionName = new CitySubdivisionNameType { Value = "ATABEY" },
                CityName = new CityNameType { Value = cityName },
                PostalZone = new PostalZoneType { Value = "34521" },
                Country = new CountryType { Name = new NameType1 { Value = "Turkey" } }
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
            party.Contact = new ContactType();
            party.Contact.Telephone = new TelephoneType { Value = telephone };
            party.Contact.Telefax = new TelefaxType { Value = telefax };
            party.Contact.ElectronicMail = new ElectronicMailType { Value = mail };

            return party;
        }



        public void createSignature()
        {
            var signature = new[]
            {
                new SignatureType
                {
                    ID = new IDType { schemeID = "VKN_TCKN", Value = "4840847211" },
                    SignatoryParty = BaseDespatchUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ","İSTANBUL","2122121212","21211111111","defaultgb@izibiz.com.tr","4840847211"),
                    DigitalSignatureAttachment=new AttachmentType
                    {
                        ExternalReference=new ExternalReferenceType
                        {
                            URI=new URIType
                            {
                                Value="#Signature_"+ baseDespatchUBL.ID
                            }
                        }
                    }
                }
             };

            baseDespatchUBL.Signature = signature;
        }

        public void DespatchSupplierParty()
        {
            var despatchSupplierParty = new SupplierPartyType //göndericinin irsaliye üzerindeki bilgileri
            {
                Party = BaseDespatchUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ", "İSTANBUL", "2122121212", "2121111111", "defaultgb@izibiz.com.tr", "4840847211"),
                DespatchContact = new ContactType
                {
                  //  ID = new IDType { Value = "" },
                    Name = new NameType1 { Value = "" }
                }
            };
            baseDespatchUBL.DespatchSupplierParty = despatchSupplierParty;
        }

        public void CreateDelivery()
        {
            var customerPartyType = new CustomerPartyType //Alıcının irsaliye üzerindeki bilgileri
            {
                Party = BaseDespatchUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ", "İSTANBUL", "2122121212", "2122221111", "defaultgb@izibiz.com.tr", "4840847211"),
            };
            baseDespatchUBL.DeliveryCustomerParty = customerPartyType;

        }

        public void Shipment()
        {
            var shipment = new ShipmentType
            {
                ID = new IDType {},
                NetWeightMeasure = new NetWeightMeasureType { unitCode = "C62", Value = 50.0M },
                TotalGoodsItemQuantity = new TotalGoodsItemQuantityType { Value = 1.0m },
                GoodsItem = new GoodsItemType[]
                {
                    new GoodsItemType{
                    ValueAmount=new ValueAmountType { currencyID="TRY", Value=8000.0m}
                                       }
                },
                ShipmentStage = new ShipmentStageType[]
                {
                    new ShipmentStageType
                    {
                        ID=new IDType{Value="1"},
                        TransportModeCode=new TransportModeCodeType{},
                        Instructions=new InstructionsType[]{},
                        TransportMeans=new TransportMeansType
                        {
                            RoadTransport=new RoadTransportType
                            {
                                LicensePlateID=new LicensePlateIDType{schemeID="PLAKA", Value="34FB69"}
                            }
                        },
                        DriverPerson=new PersonType[]
                        {
                            new PersonType
                            {
                                FirstName=new FirstNameType{Value="DEMET"},
                                FamilyName=new FamilyNameType{Value="DERE"},
                                NationalityID=new NationalityIDType{Value="11111111111"}
                            }
                        },
                    },

                },
                Delivery = new DeliveryType
                {
                    DeliveryAddress = new AddressType
                    {
                        CitySubdivisionName = new CitySubdivisionNameType { Value = "info@bicycleworld.com" },
                        CityName = new CityNameType { Value = "ISTANBUL" },
                        PostalZone = new PostalZoneType { Value = "34065" },
                        Country = new CountryType
                        {
                            Name = new NameType1 { Value = "TURKIYE" }
                        }
                    },
                    CarrierParty = BaseDespatchUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ", "ISTANBUL", "2122121212", "2122221111", "defaultgb@izibiz.com.tr", "4840847211"),
                    Despatch = new DespatchType
                    {
                        ActualDespatchDate = new ActualDespatchDateType { Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) },
                        ActualDespatchTime = new ActualDespatchTimeType { Value = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss")) }

                    }
                }
            };
            baseDespatchUBL.Shipment = shipment;
        }

        public void addDespatchLine()
        {
            var despatchLine = new DespatchLineType
            {
                ID = new IDType { Value = "1" },
                Note = new NoteType[] { },
                DeliveredQuantity = new DeliveredQuantityType { unitCode = "C62", Value = 1.0m },
                OrderLineReference = new OrderLineReferenceType { LineID = new LineIDType { } },
                Item = new ItemType
                {
                    Name = new NameType1 { Value = "Laptop" },
                    SellersItemIdentification = new ItemIdentificationType { ID = new IDType { Value = "BP0001" } }
                },
                Shipment = new ShipmentType[]
                 {
                    new ShipmentType
                    {
                    ID=new IDType { Value=""},
                    GoodsItem=new GoodsItemType[]
                        {
                            new GoodsItemType
                            {
                                InvoiceLine=new InvoiceLineType[]
                                {new InvoiceLineType{
                                    ID=new IDType{},
                                    InvoicedQuantity=new InvoicedQuantityType{unitCode="C62",Value=0m},
                                    LineExtensionAmount=new LineExtensionAmountType
                                    {
                                        currencyID="TRY",
                                        Value=8000.0m
                                    },
                                    Item=new ItemType{Name=new NameType1{Value="BP0001"}},
                                    Price=new PriceType
                                    {
                                        PriceAmount=new PriceAmountType{currencyID="TRY",Value=8000.0m}
                                    }
                                }
                                }
                            }
                        }
                    }

                 }
            };
            listDespatchLine.Add(despatchLine);
            baseDespatchUBL.DespatchLine = listDespatchLine.ToArray();
        }
    }
}
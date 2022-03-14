using Izibiz;
using Izibiz.Ubl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UblInvoice;


namespace Izibiz.UblCreate
{
    public class BaseInvoiceUBL
    {
        public InvoiceType baseInvoiceUBL { get; protected set; }
        private List<InvoiceLineType> listInvLine = new List<InvoiceLineType>();
        public List<DocumentReferenceType> docRefList = new List<DocumentReferenceType>();
       // public MonetaryTotalType baseMonetaryTotal { get; set; }

        
        public BaseInvoiceUBL()
        {
            baseInvoiceUBL = new InvoiceType();
            //baseMonetaryTotal = new MonetaryTotalType();
            //extensionsHeader();
            createInvoiceHeader();
            createSignature();
            AccountSupplierParty();
            AccountCustomerParty();
            CreateDelivery();
            createTaxTotal();
            createLegalMonetarTotal();
            addInvoiceLine();
        }


        //public void extensionsHeader()
        //{
        //    System.Xml.XmlElement element;
        //    var ublext = new[]
        //    {
        //        new UBLExtensionType
        //        {
        //        }              
        //    };
          
        //    baseInvoiceUBL.UBLExtensions = ublext;
        //}


        public void createInvoiceHeader()
        {
            baseInvoiceUBL.UBLVersionID = new UBLVersionIDType { Value = "2.1" }; //uluslararası fatura standardı 2.1
            baseInvoiceUBL.CustomizationID = new CustomizationIDType { Value = "TR1.2" }; //fakat GİB UBLTR olarak isimlendirdiği Türkiye'ye özgü 1.2 efatura formatını kullanıyor.
            baseInvoiceUBL.ProfileID = new ProfileIDType { Value = "TEMELFATURA" };
            baseInvoiceUBL.ID = new IDType { Value = "DAA2022000000002" };//id yi simdilik unıqıe bır deger verıyoruz ,servise gond. degıstırılecek
            baseInvoiceUBL.CopyIndicator = new CopyIndicatorType { Value = false };
            baseInvoiceUBL.UUID = new UUIDType { Value = Guid.NewGuid().ToString() };
            baseInvoiceUBL.IssueDate = new IssueDateType { Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))};
            baseInvoiceUBL.IssueTime = new IssueTimeType { Value = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss"))};
            baseInvoiceUBL.InvoiceTypeCode = new InvoiceTypeCodeType { Value = "SATIS" };
            baseInvoiceUBL.DocumentCurrencyCode = new DocumentCurrencyCodeType { Value = "TRY" };
            baseInvoiceUBL.LineCountNumeric = new LineCountNumericType { Value = 1 };

        }

        public void addAdditionalDocumentReference()
        {

            var arcRef = new DocumentReferenceType();
            arcRef.ID = new IDType { Value = Guid.NewGuid().ToString() };
            arcRef.IssueDate = baseInvoiceUBL.IssueDate;
            arcRef.DocumentType = new DocumentTypeType { Value = ("XSLT") };
            // arcRef.DocumentTypeCode = new DocumentTypeCodeType { Value = docTypeCode };
            docRefList.Add(arcRef);

            baseInvoiceUBL.AdditionalDocumentReference = docRefList.ToArray();
        }
        public static PartyType createParty(string partyName, string cityName, string telephone, string mail)
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
                                ID = new IDType { schemeID = "VKN", Value = "4840847211" }
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
                    SignatoryParty = BaseInvoiceUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ","ISPARTA","2122121212","defaultgb@izibiz.com.tr"),
                    DigitalSignatureAttachment=new AttachmentType
                    {
                        ExternalReference=new ExternalReferenceType
                        {
                            URI=new URIType
                            {
                                Value="#Signature_DAA2022000000002"
                            }
                        }
                    }
                }
             };

            baseInvoiceUBL.Signature = signature;
        }


        public void AccountSupplierParty()
        {
            var accountingSupplierParty = new SupplierPartyType //göndericinin fatura üzerindeki bilgileri
            {
                Party = BaseInvoiceUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ", "ISPARTA", "2122121212", "defaultgb@izibiz.com.tr"),
            };
            baseInvoiceUBL.AccountingSupplierParty = accountingSupplierParty;
        }


        public void AccountCustomerParty()
        {
            var accountingCustomerParty = new CustomerPartyType //Alıcının fatura üzerindeki bilgileri
            {
                Party = BaseInvoiceUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ", "ISPARTA", "2122121212", "defaultgb@izibiz.com.tr"),
            };
            baseInvoiceUBL.AccountingCustomerParty = accountingCustomerParty;
        }

        public void CreateDelivery()
        {
            var deliveryArr = new[]
              {
                  new DeliveryType
                  {
                     DeliveryParty=BaseInvoiceUBL.createParty("İZİBİZ BİLİŞİM TEKNOLOJİLERİ AŞ", "ISPARTA", "2122121212", "defaultgb@izibiz.com.tr"),
                     Despatch= new DespatchType {ActualDespatchDate=new ActualDespatchDateType { Value= DateTime.Now },
                     ActualDespatchTime=new ActualDespatchTimeType{Value=DateTime.Now}
                     }
                  }
              };
            baseInvoiceUBL.Delivery = deliveryArr;
        }

        public void addInvoiceLine()
        {

            InvoiceLineType invoiceLine = new InvoiceLineType
            {

                ID = new IDType { Value = "1" },
                Note = new NoteType[] { new NoteType { Value = "Note" } },
                InvoicedQuantity = new InvoicedQuantityType { unitCode = "EA", Value = 1.0m },
                LineExtensionAmount = new LineExtensionAmountType { currencyID = "TRY", Value = 17.8m },
                TaxTotal = new TaxTotalType
                {
                    TaxAmount = new TaxAmountType
                    {
                        currencyID = "TRY",
                        Value = 3.2m
                    },

                    TaxSubtotal = new[]
                        {
                                new TaxSubtotalType
                                {
                                    TaxableAmount = new TaxableAmountType
                                    {
                                        currencyID = "TRY",
                                        Value = 17.8m
                                    },

                                    TaxAmount = new TaxAmountType
                                    {
                                        currencyID = "TRY",
                                        Value = 3.2m
                                    },
                                    CalculationSequenceNumeric=new CalculationSequenceNumericType
                                    {
                                        Value=1
                                    },
                                    Percent = new PercentType1 { Value =18.0m },

                                    TaxCategory = new TaxCategoryType
                                    {
                                        TaxScheme = new TaxSchemeType
                                        {
                                            Name = new NameType1 { Value = "KDV"},
                                            TaxTypeCode = new TaxTypeCodeType { Value = "0015" }
                                        }
                                    }
                                }
                        }
                },
                Item = new ItemType
                {
                    Name = new NameType1 { Value = "PROSER.1028" },
                    Description = new DescriptionType { Value = "Fiyatı var" }
                },

                Price = new PriceType
                {
                    PriceAmount = new PriceAmountType
                    {
                        currencyID = "TRY",
                        Value = 17.8m
                    }
                }
            };
            listInvLine.Add(invoiceLine);
            baseInvoiceUBL.InvoiceLine = listInvLine.ToArray();
        }


        public void createTaxTotal()
        {
            TaxTotalType[] taxtotal = new TaxTotalType[]
            {   new TaxTotalType
             {

                TaxAmount = new TaxAmountType
                {
                    currencyID = "TRY",
                    Value = 5
                },
                TaxSubtotal = new[]
                        {
                                new TaxSubtotalType
                                {
                                    TaxableAmount = new TaxableAmountType
                                    {
                                        currencyID = "TRY",
                                        Value = 17.8m
                                    },

                                    TaxAmount = new TaxAmountType
                                    {
                                        currencyID = "TRY",
                                        Value = 3.2m
                                    },
                                    CalculationSequenceNumeric=new CalculationSequenceNumericType
                                    {
                                        Value=1
                                    },
                                    Percent = new PercentType1 { Value =18.0m },

                                    TaxCategory = new TaxCategoryType
                                    {
                                        TaxScheme = new TaxSchemeType
                                        {
                                            Name = new NameType1 { Value = "KDV"},
                                            TaxTypeCode = new TaxTypeCodeType { Value = "0015" }
                                        }
                                    }
                                }
                        }
            }

            };
            baseInvoiceUBL.TaxTotal = taxtotal;
        }

        public void createLegalMonetarTotal()
        {
            var legalMonetaryTotal = new MonetaryTotalType
            {
                LineExtensionAmount = new LineExtensionAmountType { currencyID = "TRY", Value = 0 },

                TaxExclusiveAmount = new TaxExclusiveAmountType { currencyID = "TRY", Value = 0 },

                TaxInclusiveAmount = new TaxInclusiveAmountType { currencyID = "TRY", Value = 0 },

                AllowanceTotalAmount = new AllowanceTotalAmountType { currencyID = "TRY", Value = 0 },

                PayableAmount = new PayableAmountType { currencyID = "TRY", Value = 21.0m }
            };
            baseInvoiceUBL.LegalMonetaryTotal = legalMonetaryTotal;
        }
    }
}
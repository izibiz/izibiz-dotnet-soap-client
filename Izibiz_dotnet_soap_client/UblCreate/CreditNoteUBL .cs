using Izibiz;
using Izibiz.Operations;
using Izibiz.Ubl;
using Izibiz.UblCreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using UblCreditNote;

namespace  Izibiz.UblCreate
{
    public class CreditNoteUBL : BaseCreditNoteUBL
    {



        public CreditNoteUBL()
            : base()
        {
            addAdinationalDocRefXslt();
        }



        private void addAdinationalDocRefXslt()
        {
            var idRef = new DocumentReferenceType();
            idRef.ID = new IDType { Value = Guid.NewGuid().ToString() };
            idRef.IssueDate = baseCreditNoteUBL.IssueDate;
            idRef.DocumentType = new DocumentTypeType { Value = nameof(EI.DocumentType.XSLT) };
            idRef.Attachment = new AttachmentType();
            idRef.Attachment.EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObjectType();
            idRef.Attachment.EmbeddedDocumentBinaryObject.characterSetCode = "UTF-8";
            idRef.Attachment.EmbeddedDocumentBinaryObject.encodingCode = "Base64";
            idRef.Attachment.EmbeddedDocumentBinaryObject.filename = baseCreditNoteUBL.ID.Value.ToString() + ".xslt";
            idRef.Attachment.EmbeddedDocumentBinaryObject.mimeCode = "application/xml";
            idRef.Attachment.EmbeddedDocumentBinaryObject.Value = Convert.FromBase64String(Xslt.xsltGibCreditNote);

            docRefList.Add(idRef);
            baseCreditNoteUBL.AdditionalDocumentReference = docRefList.ToArray();
        }


    }
}

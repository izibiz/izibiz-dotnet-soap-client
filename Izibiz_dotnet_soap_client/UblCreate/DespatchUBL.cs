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

    public class DespatchUBL : BaseDespatchUBL
    {


        public DespatchUBL()
           : base()
        {
            addAdinationalDocRefXslt();
        }



        private void addAdinationalDocRefXslt()
        {

            var idRef = new DocumentReferenceType();
            idRef.ID = new IDType { Value = Guid.NewGuid().ToString() };
            idRef.IssueDate =new IssueDateType { Value= DateTime.Now}; 
            idRef.DocumentType = new DocumentTypeType { Value = "XSLT" };
            idRef.Attachment = new AttachmentType();
            idRef.Attachment.EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObjectType();

            idRef.Attachment.EmbeddedDocumentBinaryObject.characterSetCode = "UTF-8";
            idRef.Attachment.EmbeddedDocumentBinaryObject.encodingCode = "Base64";
            idRef.Attachment.EmbeddedDocumentBinaryObject.filename = baseDespatchUBL.ID.Value.ToString() + ".xslt";
            idRef.Attachment.EmbeddedDocumentBinaryObject.mimeCode = "application/xml";
          
            idRef.Attachment.EmbeddedDocumentBinaryObject.Value = Convert.FromBase64String(Xslt.xsltGibDespatch);

            docRefList.Add(idRef);
            baseDespatchUBL.AdditionalDocumentReference = docRefList.ToArray();
        }




    }
}
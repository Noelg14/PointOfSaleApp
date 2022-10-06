using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using Color = MigraDoc.DocumentObjectModel.Color;

namespace Demo
{
    internal class ReportService
    {
        public static void createDocument()
        {
            Document doc = new Document();

            Section section = doc.AddSection();
            section.AddParagraph();

            Paragraph paragraph = section.AddParagraph();
            
            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
            
            paragraph.AddFormattedText("Hello, World!", TextFormat.Underline);

            //FormattedText ft = paragraph.AddFormattedText("Small text",TextFormat.Bold);
            //ft.Font.Size = 6;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = doc;
            pdfRenderer.RenderDocument();

            pdfRenderer.PdfDocument.Save("hello.pdf");
            //Process.Start("hello.pdf");

            

        }
    }
}

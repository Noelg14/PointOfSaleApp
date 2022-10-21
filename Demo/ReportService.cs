using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using System.Data;
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
            
            paragraph.AddFormattedText(Utils.getIndiviudalSetting("Name")+" \n", TextFormat.Underline);
            paragraph.AddFormattedText(Utils.getIndiviudalSetting("address").Replace("<br>", "\n") + " \n", TextFormat.Underline);
            paragraph.AddFormattedText(Utils.getIndiviudalSetting("message").Replace("<br>","\n") + " \n", TextFormat.Underline);

            //FormattedText ft = paragraph.AddFormattedText("Small text",TextFormat.Bold);
            //ft.Font.Size = 6;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = doc;
            pdfRenderer.RenderDocument();

            pdfRenderer.PdfDocument.Save(Directory.GetCurrentDirectory()+"\\Reports\\hello.pdf");
            //Process.Start("hello.pdf");
        }
        //Creates a PDF from the exports table.
        //TODO: Error catching
        //TODO : Add Table as this will Display data better 
        public static string createDocument(string reportFormat)
        {
            System.Data.DataTable data = Utils.GetExportData(reportFormat);
            Document doc = new Document();

            Section section = doc.AddSection();
            section.AddParagraph();

            Paragraph paragraph = section.AddParagraph();

            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
           // paragraph.AddFormattedText("PLU | Desc | QTY \n");
            
            //this is slow. but cant think of an easier way to do it. need to read down & across the data somehow.
            for (int i=0;i < data.Rows.Count; i++)
            {
                Object[] dataArray =  data.Rows[i].ItemArray;
                foreach(Object d in dataArray)
                {
                    paragraph.AddFormattedText(d.ToString());
                    paragraph.AddFormattedText("|");
                }

                paragraph.AddFormattedText("\n");
            }

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = doc;
            pdfRenderer.RenderDocument();

            pdfRenderer.PdfDocument.Save(Directory.GetCurrentDirectory() + $"\\Reports\\{reportFormat}.pdf");
            return Directory.GetCurrentDirectory() + $"\\Reports\\{reportFormat}.pdf";
            //Process.Start("hello.pdf");
        }
    }
}

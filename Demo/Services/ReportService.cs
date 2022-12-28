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
using System.Runtime.InteropServices;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;

namespace Demo
{
    internal class ReportService
    {
        public static string createRecDocument(string id="rec",Cart cartDetails = null,string pay ="none")
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Document doc = new Document();

            Section section = doc.AddSection();
            section.AddParagraph();

            Paragraph paragraph = section.AddParagraph();
            
            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
            
            paragraph.AddFormattedText(Utils.getIndiviudalSetting("Name")+" \n");
            paragraph.AddFormattedText(Utils.getIndiviudalSetting("address").Replace("<br>", "\n") + " \n");
            paragraph.AddFormattedText(Utils.getIndiviudalSetting("message").Replace("<br>","\n") + " \n");

            if(cartDetails != null)
            {
                id = cartDetails.id.ToString();
                //section = doc.AddSection();
                //section.AddParagraph();
                //Make a table 
                paragraph = section.AddParagraph();
                paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
                paragraph.AddFormattedText("\nDetails (PLU,Desc,Price)" + "\n",TextFormat.Bold);
                paragraph.AddFormattedText("\n");
                double s = 0;
                foreach (Product p in cartDetails.products)
                {
                    paragraph.AddFormattedText(p.PLU+"\n"+p.desc + " " +currentCulture.NumberFormat.CurrencySymbol+ p.price.ToString("0.00") + "\n", TextFormat.Bold);
                    s += p.price;
                }
                paragraph.AddFormattedText($"\nTotal : {currentCulture.NumberFormat.CurrencySymbol}"+s.ToString("#.##") + "\n");
                
                paragraph.AddFormattedText($"Paid By: {pay}\n");

                paragraph = section.AddParagraph();
                Stream fs = new FileStream($"{cartDetails.id}.jpeg", FileMode.OpenOrCreate);
                Image img = Utils.genQR(cartDetails.id.ToString());
                img.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                fs.Close();

                paragraph.AddImage($"{cartDetails.id}.jpeg");


            }
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = doc;
            pdfRenderer.RenderDocument();
            string filePath = Directory.GetCurrentDirectory() + $"\\Reports\\{id}.pdf";
            pdfRenderer.PdfDocument.Save(filePath);

            return filePath;
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
                    paragraph.AddFormattedText(" | ");
                }

                paragraph.AddFormattedText("\n");
            }

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = doc;
            pdfRenderer.RenderDocument();
            try
            {

                pdfRenderer.PdfDocument.Save(Directory.GetCurrentDirectory() + $"\\Reports\\{reportFormat}.pdf");
                return Directory.GetCurrentDirectory() + $"\\Reports\\{reportFormat}.pdf";
            }
            catch(Exception e ) {
                Utils.log("Failed to save file: " + reportFormat);
                return "";
            }

            //Process.Start("hello.pdf");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ClosedXML.Excel;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Demo
{
    public class excelFunctions
    {
        public static void createExcel(MySqlDataReader dataReader)
        {
            string fileName = "Report1";
            string currDep, currType;
            // Init Workbook
            XLWorkbook workbook = new XLWorkbook();
            workbook.Author = "Noel";
            var ws = workbook.AddWorksheet("Report");
            ws.Row(1).Style.Fill.SetBackgroundColor(XLColor.SteelBlue);
            ws.Row(1).Style.Font.SetFontColor(XLColor.White).Font.SetBold(true);

            int row = 2;
            while (dataReader.Read())
            {
                populateHeader(dataReader, ws);

                for (int col = 0; col <= dataReader.FieldCount - 1; col++)
                    {
                        ws.Cell(row, col + 1).Value = dataReader.GetValue(col);
                    }

                    row++;
            }
            ws.Columns().AdjustToContents();
            //ws.Column(1).Width = 50;
            //ws.Rows().Height = 50;
            #region Saving

            SaveFileDialog s = new SaveFileDialog();
            s.FileName = fileName + ".xlsx";
            s.InitialDirectory = checkFolder(Directory.GetCurrentDirectory() + "\\Reports"); // if not exists, creates
            s.DefaultExt = ".xlsx";
            s.Filter = "Excel Files (*.xlsx,*.xls)|*.xls;*.xlsx";

            if (s.ShowDialog() == DialogResult.OK)
            {
                fileName = s.FileName;
                if (!fileName.Contains(".xls"))
                {
                    fileName += ".xlsx";
                }
                workbook.SaveAs(fileName);
                //MessageBox.Show("Exported as " + fileName, "Export Sucessful");
                DialogResult res = MessageBox.Show($"Do you want to Open the file \n {fileName}", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (res == DialogResult.OK)
                {
                    ProcessStartInfo psi = new ProcessStartInfo(fileName);
                    psi.UseShellExecute = true;
                    psi.WindowStyle = ProcessWindowStyle.Minimized;
                    Process.Start(psi);
                }
                if (res == DialogResult.Cancel)
                {
                }
                //string fileToOpen = fileName.Split(Directory.GetCurrentDirectory())[1].Substring(1);

            }

            #endregion
        }
        private static void populateHeader(MySqlDataReader dr, IXLWorksheet ws)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                ws.Cell(1, i + 1).Value = dr.GetName(i);
            }

        }
        private static string checkFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }
    
    }
}

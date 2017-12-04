using DevExpress.XtraEditors;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCProShirts
{
    public static class ApplicationLibary
    {
        public static string ConvertImageToBase64(string path)
        {
            string base64String = string.Empty;
            if (!File.Exists(path))
                return base64String;
            // Convert Image to Base64
            using (Image img = Image.FromFile(path)) // Image Path from File Upload Controller
            {
                using (var memStream = new MemoryStream())
                {
                    img.Save(memStream, img.RawFormat);
                    byte[] imageBytes = memStream.ToArray();
                    // Convert byte[] to Base64 String
                    base64String = Convert.ToBase64String(imageBytes);
                }
                img.Dispose();
            }
            return base64String;
        }

        public static DataTable getDataExcelFromFileToDataTable(string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (data) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    //Get all Table
                    DataTableCollection tables = result.Tables;
                    DataTable dt = tables[0];
                    return dt;
                }

            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void writeLog(this ListBoxControl lsbox, string message, int status)
        {
            string space = "...................................." + (status == 1 ? "Done" : status == 2 ? "Faild" : "In Progress");
            string time = DateTime.Now.ToString("HH:mm:ss");
            lsbox.Items.Insert(0, "[" + time + "] " + message + space);
            lsbox.Refresh();
        }

        public static void writeLogThread(this ListBoxControl lsbox, string message, int status)
        {
            string space = "...................................." + (status == 1 ? "Done" : status == 2 ? "Faild" : "In Progress");
            lsbox.Invoke((MethodInvoker)delegate
            {
                string time = DateTime.Now.ToString("HH:mm:ss");
                lsbox.Items.Insert(0, "[" + time + "] " + message + space);
                lsbox.Refresh();
            });
        }

        public static string convertStringToJson(string text)
        {
            var result = "";
            var value = text.Split(',');
            foreach (string item in value)
            {
                if (item != "")
                    result += "\"" + item + "\",";
            }
            return result.TrimEnd(',');
        }
    }
}

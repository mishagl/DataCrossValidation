using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    using System.Data;
    using System.Data.OleDb;
    using System.Data.SqlClient;

    class ExcelReader
    {
        public List<object> readExcelFile(string fileName, DataReader csvReader)
        {
            List<object> data = new List<object>();
            long time1 = DateTime.Now.Millisecond;
            Console.WriteLine("start excel load" + DateTime.Now.ToString("h:mm:ss tt"));

            using (OleDbConnection conn = new OleDbConnection())
            {
                DataTable dt = new DataTable();
                conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;MAXSCANROWS=0'";
                using (OleDbCommand comm = new OleDbCommand())
                {
                    conn.Open();
                    DataTable dt1 = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    String[] excelSheets = new String[dt.Rows.Count];

                    if (excelSheets ==  null || excelSheets.Count() > 1)
                        throw new Exception("excel workbook has more sheets than 1 worksheet");
                    
                    string sheetName = null;

                    // Add the sheet name to the string array.
                    foreach (DataRow row in dt1.Rows)
                    {
                        sheetName = row["TABLE_NAME"].ToString();                        
                    }
                    sheetName = sheetName.Remove(sheetName.Length-1);
                    conn.Close();
                    comm.CommandText = "Select * from [" + sheetName + "]";
                    comm.Connection = conn;

                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                    }
                    this.initList(dt, data, csvReader);
                }
            }
            long time2 = DateTime.Now.Millisecond;
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
            Console.WriteLine("excel reading time = " + (time2 - time1));

            return data;
        }
        /*        public List<object> readExcelFile(string fileLocation) {
                    List<object> data = new List<object>();
                    Application xlApp = new Application();
                    Workbook xlWorkBook;
                    xlWorkBook = xlApp.Workbooks.Open(fileLocation, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    Worksheet excelSheet = xlWorkBook.ActiveSheet;
                    Range range = (Range)excelSheet.UsedRange;
                    int rCnt = 0;
                    int cCnt= 0;
                    int rw = 0;
                    int cl = 0;

                    rw = range.Rows.Count;
                    cl = range.Columns.Count;

                    long time1 = DateTime.Now.Millisecond;
                    Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
                    int count = 0;
                    for (rCnt = 1; rCnt <= rw; rCnt++)
                    {
                        for (cCnt = 1; cCnt <= cl; cCnt++)
                        {
                            count++;
                            object value = (range.Cells[rCnt, cCnt] as Range).Value;
                            if (value is string)
                            {
                                data.Add((string)value);
                            }
                            else if (value is double)
                            {
                                data.Add((Double)value);
                            }
                            else {
                                data.Add("NUL");
                            }
                        }
                    }
                    xlWorkBook.Close(true, null, null);
                    xlApp.Quit();

                    Marshal.ReleaseComObject(excelSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);
                    long time2 = DateTime.Now.Millisecond;
                    Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
                    Console.WriteLine("excel reading time = " + (time2 - time1));
                    return data;
                }*/
        private void initList(DataTable dt, List<object> list, DataReader reader) {

       //     SqlDataAdapter adapter = new SqlDataAdapter();
//            adapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                object[] dataFromFile = row.ItemArray;
                list.AddRange(dataFromFile);
            }
            if(dt.Rows.Count > 0)
                DataInformation.setLineBreakExcel((dt.Rows[0].ItemArray).Count());            
        }
    }
}

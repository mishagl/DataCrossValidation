using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class DataReader
    {



        public List<Object> readData(string fileName)
        {
            List<Object> list = new List<Object>();

            string extension = Auxiliary.getExtension(fileName);

            if (extension.Equals("txt"))
            {
                list = this.readTxtCSVFile(fileName, '\t');
            }
            else if (extension.Equals("csv"))
            {
                list = this.readTxtCSVFile(fileName, ',');
            }
            else if (extension.Equals("xls") || extension.Equals("xlsx"))
            {
                ExcelReader reader = new ExcelReader();
                DataInformation.setIfSpreadSheet(true);
                list = reader.readExcelFile(fileName, this);
            }

            return list;
        }

        const int excelLimit = 255;
        private List<Object> readTxtCSVFile(string file, char delimeter)
        {
            DataInformation.setNotExcelSel(true);
            List<Object> list = new List<Object>(); //add data one by one
            string[] lines = System.IO.File.ReadAllLines(file);
            int counterTopRow = 0;
            foreach (string str in lines) {
                if (counterTopRow == 0) {
                    counterTopRow++;
                    continue;
                }
                string[] items = str.Split(delimeter);
                DataInformation.setIfSpreadSheet(items.Length == 255);
                DataInformation.setLineBreakOther(items.Count()); //remove from here
                int counter = 0;
                foreach (string item in items) {
                    if (counter == excelLimit) //excel files are limited to 255 columns
                        break;
                    list.Add(guessValue(item));
                    counter++;
                }
            }
            return list;
        }

        private Object guessValue(string str) {
            if (str.Equals("") || str.Equals(" "))
            {
                return "NUL"; //to verify that blanks match
            }
            Object obj = new object();
            double r = 0;
            if (Double.TryParse(str, out r))
                obj = Double.Parse(str);
            else
                obj = str;
            return obj;
        }
    }
}

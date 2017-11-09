using System; 
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApplication1
{
    class ComparisonReportGenerator
    {
        private string fileName;
        private List<string> itemsToWrite;
        StreamWriter fileWriter;


        public ComparisonReportGenerator(String[] fileNames, String logName)
        {
            fileName = getLocation(fileNames) + "\\" + logName;
            if (this.fileName.Equals(""))
                throw new Exception("comparison report nothing to show");
            fileName = checkFileName(fileName);
            this.itemsToWrite = new List<string>();
            fileWriter = new StreamWriter(this.fileName);
        }

        private string checkFileName(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return fileName;
            }
            else
            {
                DateTime now = DateTime.Now;
                string extension = Auxiliary.getExtension(fileName);
                int lastPeriod = fileName.LastIndexOf('.');
                fileName = fileName.Substring(0,lastPeriod) + now.ToString("MM,dd,yyyy,H,mm,ss")+ "." + extension;
                return checkFileName(fileName);
            }
        }

        private string getLocation(String[] fileNames) {
            string location = "";
            if (fileNames != null && fileNames.Length > 0)
            {
                int lastBackSlash = fileNames[0].LastIndexOf('\\');

                location = fileNames[0].Substring(0, lastBackSlash);
                return location;
            }
            else
                return location;

        }

        public void createItemToWrite(object obj1, object obj2, bool passedTest) {
            string result = "";
            if (passedTest)
                result = passedTest + ":" + obj1 + ":"+ obj2 + "\t";
            else
                result = "!!!" + passedTest + ":" + obj1 + ":" + obj2 + "\t";
            itemsToWrite.Add(result);
        }

        public void writeSingleLine(string str1)
        {
            fileWriter.WriteLine(str1);
        }


        public void writeToFileStrs(bool ifSpreadsheet)
        {
            
            int counter = 0;
            int lineBreak = DataInformation.getLineBreak();

            foreach (string str in itemsToWrite) {

                if (counter == lineBreak)
                {
                    fileWriter.WriteLine("");
                    counter = 0;
                }
                fileWriter.Write(str);
                counter++;
            }
            fileWriter.WriteLine("END");
            fileWriter.Flush();
        }

    }
}

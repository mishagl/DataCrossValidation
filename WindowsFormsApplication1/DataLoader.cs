using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class DataLoader
    {
        public void getFiles() {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "c:\\";
            dialog.Filter = "Our files (*.xlsx, *.csv, *txt, *.xls)|*.xlsx; *.csv; *txt; *.xls";
            dialog.Multiselect = true;
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] selectedFiles = dialog.FileNames;
                bool ifCorrectFilesSelected = this.analyzeExtensions(selectedFiles);

                if (ifCorrectFilesSelected) {
                    DataReader dataReader = new DataReader();
                    Dictionary<string, List<Object>> fileExtDataMap = new Dictionary<string, List<object>>();
                    foreach (string fileLocation in selectedFiles)
                    {
                        Auxiliary.IsFileLocked(fileLocation);
                        string extension = Auxiliary.getExtension(fileLocation);
                        fileExtDataMap.Add(extension, dataReader.readData(fileLocation));
                    }
                    compareData(selectedFiles, dataReader, fileExtDataMap);
                }
            }
        }

        private void compareData(string[] selectedFiles, DataReader dataReader, Dictionary<string, List<Object>> fileExtDataMap)
        {
            int firstRecord = 0;
            KeyValuePair<string, List<Object>> previous = new KeyValuePair<string, List<object>>();

            foreach (KeyValuePair<string, List<Object>> entry in fileExtDataMap)
            {
                if (firstRecord == 0)
                {
                    firstRecord++; //skip first file
                    previous = entry;
                    continue;
                }
                ComparisonReportGenerator comparison = new ComparisonReportGenerator(selectedFiles, "log.txt");
                DataComparison dataComparison = new DataComparison(comparison);

                dataComparison.compareObj(previous.Value, entry.Value, previous.Key, entry.Key, DataInformation.getIfSpreadSheet());
                previous = entry;
                firstRecord++;
            }

        }

        public bool analyzeExtensions(string[] fileNames) {
            //if less than 2 files selected
            if (fileNames.Length < 2 || fileNames.Length > 2 )
            {
                MessageBox.Show("inusfficient number of files selected");
                return false;
            }

            //check if the different types were selected, if 2 similar types were selected 
            HashSet<string> set = new HashSet<string>();
            foreach (string fileName in fileNames)
            {
                string extension = Auxiliary.getExtension(fileName);
                if (!set.Add(extension)) {
                    MessageBox.Show("same type of files were selected");
                    return false;
                }
            }
            return true;
        }
    }
}

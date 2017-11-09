using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Auxiliary
    {
        public static string getExtension(string fullName)
        {
            if (fullName == null || fullName.Equals("")) {
                throw new Exception("file doesn't exist");
            }
            int lastPeriod = fullName.LastIndexOf('.');
            string extension = fullName.Substring((lastPeriod + 1), (fullName.Length - lastPeriod - 1)); //-1 for last period as pure extensions are used in the comparison
            return extension;
        }

        
        public static string getFileName(string fileName, bool withExtension)
        {
            if (fileName == null || fileName.Equals(""))
            {
                throw new Exception("file doesn't exist");
            }
            int lastPeriod = fileName.LastIndexOf('.');
            int lastSlash = fileName.LastIndexOf('\\');
            int extensionSize = fileName.Length - lastPeriod - 1;
            int sizeOfName = 0;

            if (! withExtension) 
                sizeOfName = fileName.Length - lastSlash - 2 - extensionSize;
            else
                sizeOfName = fileName.Length - lastSlash - 2;

            string fileNameStr = fileName.Substring(lastSlash + 1, sizeOfName);
            return fileNameStr;
        }

        public static void IsFileLocked(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                throw new Exception("the file is used by another application");
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                //return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }
    }
}

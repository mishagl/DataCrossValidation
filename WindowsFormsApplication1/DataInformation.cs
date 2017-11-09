using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class DataInformation
    {
        private static bool ifSpreadSheet = false;
        private static int lineBreakExcel = -1;
        private static int lineBreakOther = -1;
        private static bool nonExcel = false;
        public static int getLineBreak()
        {
            if (ifSpreadSheet && nonExcel)
            {
                if (lineBreakOther == 0 || lineBreakExcel == 0) //they were initialized, but file doesn't contain data
                    throw new Exception("line break equals 0");

                if (lineBreakOther < 256 && lineBreakOther != lineBreakExcel)
                         throw new Exception("number of items in a row is different");

                if (lineBreakExcel > lineBreakOther)
                    return lineBreakOther;
                else
                    return lineBreakExcel;
            }
            return lineBreakExcel;
        }

        public static void setNotExcelSel(bool state)
        {
            nonExcel = state;
        }

        public static void setIfSpreadSheet(bool value)
        {
            ifSpreadSheet = value;
        }

        public static void setLineBreakExcel(int value)
        {
            lineBreakExcel = value;
        }

        public static void setLineBreakOther(int value)
        {
            lineBreakOther = value;
        }

        public static bool getIfSpreadSheet()
        {
            return ifSpreadSheet;
        }

    }
}

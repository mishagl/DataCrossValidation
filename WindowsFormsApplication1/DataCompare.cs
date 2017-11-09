using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class DataComparison
    {
        private ComparisonReportGenerator comparison;

        public DataComparison(ComparisonReportGenerator comparisonIn)
        {
            this.comparison = comparisonIn;
        }

        public bool compareObj(List<Object> list1, List<Object> list2, string key1, 
            string key2, bool ifSpreadSheet)
        {
            if (list1.Count != list2.Count && list1.Count > 0) {
                throw new Exception("DataComparison/compareObj: there are no items in the list or the lists dont equal");
            }

            int counterList2 = 0;
            int listSize = list1.Count;
            
            foreach (Object obj1 in list1)
            {
                object obj2 = list2[counterList2];
                compareAndWriteObj(obj1, obj2);
                counterList2++;
            }

            comparison.writeToFileStrs(ifSpreadSheet);
            Console.WriteLine("SUCCESS");
            return true; 
        }

        private void compareAndWriteObj(object obj1, object obj2)
        {
            object compValue1 = null;
            object compValue2 = null;


            if (obj2 == null && obj1 == null)
            {
                compValue1 = "null";
                compValue2 = "null";
                this.comparison.createItemToWrite(compValue1, compValue2, compareObj(compValue1, compValue2));
                return;
            }
            if (obj1 is String && (obj2 is Double && (Double)obj2 == 0))
            {
                compValue1 = "Str Nul";
                compValue2 = "Double 0";
            }
            else if (obj2 is String && (obj1 is Double && (Double)obj1 == 0))
            {
                compValue1 = "Double 0";
                compValue2 = "Str Nul";
            }
            else
            {
                compValue1 = obj1;
                compValue2 = obj2;
            }

            if (compValue2 is DBNull)
                compValue2 = "NUL";

            if (compValue1 is DBNull)
                compValue1 = "NUL";

            Type type = (compValue2.GetType()); //TODO will it be needed

            bool bTemp = compareObj(compValue1, compValue2);
            this.comparison.createItemToWrite(compValue1, compValue2, bTemp);

        }

        private bool compareObj(Object obj1, Object obj2) {
            if(obj1 is string && obj2 is string)
                return this.compareStr(obj1, obj2);

            if (obj1 is Double && obj2 is Double)
                return this.compareDouble(obj1, obj2);

            if ((obj1 is Double && obj2 is String) || (obj1 is String && obj2 is Double))
                return this.compareStrAndDouble(obj1, obj2);

            if (obj1 is DateTime || obj2 is DateTime)
                return compareDates(obj1, obj2);

            return false;            
        }

        private bool compareDates(object obj1, object obj2)
        {
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now;
            if (obj1 is DateTime)
            {
                date1 = (DateTime)obj1;
                if (!DateTime.TryParse((string)obj2, out date2))
                    return false;
            }
            if (obj2 is DateTime)
            {
                date2 = (DateTime)obj2;
                if (!DateTime.TryParse((string)obj1, out date1))
                    return false;
            }

            if (date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day)
                    return true;
           
            return false;

        }

        private bool compareStrAndDouble(object obj1, object obj2)
        {
            if (obj1 is String && obj2 is Double)
            {
                double outD = 0;
                if (double.TryParse((string)obj1, out outD))
                    return compareDouble(obj2, outD);
            }

            if (obj2 is String && obj1 is Double)
            {
                double outD = 0;
                if (double.TryParse((string)obj2, out outD))
                    return compareDouble(obj1, outD);
            }

            return false;
        }

        private bool compareDouble(object obj1, object obj2)
        {
            double maxAllowedDiff = 0.05;

            double d1 = ((Double)obj1);
            double d2 = ((Double)obj2);
            if (d1 == d2)
                return true;

            d1 = Math.Round(d1, 2);
            d2 = Math.Round(d2, 2);

            if ((d2 != 0 || d1 != 0) && (Math.Abs(1 - d1 / d2) <= maxAllowedDiff))
                return true;

            //zero exceptions
            if (d1 == 0 && (Math.Abs(d2) <= maxAllowedDiff))
                return true;

            if (d2 == 0 && (Math.Abs(d1) <= maxAllowedDiff))
                return true;


            return false;

        }

        private bool compareStr(object obj1, object obj2)
        {
            if (obj1 is String)
            {
                string str1 = ((string)obj1).Trim().ToLower();
                string str2 = ((string)obj2).Trim().ToLower();
                return str1.Equals(str2);
            }
            return false;
        }
    }
}

using System;
using PolarDB;

namespace PolarTableIndex
{
    public class Scale
    {
        private readonly Func<object[], long> keyFromIndexRow;
        private readonly Diapason[] diapasons;
        private readonly long minkey = long.MaxValue;
        private readonly long maxkey = long.MinValue;
        private long differece;
        private const int diapasonsCount = 10000;

        public Scale(Func<object[],long> keyFromIndexRow , PaCell indexCell)
        {
            this.keyFromIndexRow = keyFromIndexRow;
            minkey = Convert.ToInt32(keyFromIndexRow((object[])indexCell.Root.Element(0).Get()));
            maxkey = Convert.ToInt32(keyFromIndexRow((object[])indexCell.Root.Element(indexCell.Root.Count() - 1).Get()));
            
            diapasons=new Diapason[diapasonsCount];
            differece = (maxkey - minkey);
            FillDiapasonsNumb(indexCell);
            FillDiapasonsStart();
        }

        private void FillDiapasonsStart()
        {
            long sum = 0;
            for (int i = 0; i < diapasons.Length; i++)
            {
                diapasons[i] = diapasons[i].AddStart(sum); //start must be 0
                sum += diapasons[i].numb;
            }
        }

        /// <summary>
        /// ���������� �������, �������� ������� ����� �� <c>minCombinedkey</c>   �� <c>maxCombinedkey</c>    � �������� �� 0 ��  diapasonsCount - 1
        /// </summary>
        /// <param name="combined">����� ���������� � Int64 Convert.ToInt64</param>
        /// <returns></returns>
        public int GetSubKey(long x)
        {
            var diffX = 1.0*(x - minkey);
            var t = (diapasons.Length - 1)*diffX;
            var d = (t/differece);
            var i = (int) d;
            if (i < 0)
            {
                
            }
            return i;
        }

        public void FillDiapasonsNumb(PaCell indexCell)
        {
            indexCell.Root.Scan(o =>
            {
               
               var subKey = GetSubKey(Convert.ToInt64(keyFromIndexRow((object[]) o)));
               diapasons[subKey] = diapasons[subKey].AddNumb(1);
                return true;
            });
        }

        public Diapason Search(long key)
        {
            var subKey = GetSubKey(key);
            if (subKey >= 0 && subKey < diapasonsCount)
                return diapasons[subKey];
           return new Diapason();
        }
    }
}
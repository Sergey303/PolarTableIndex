using System;
using PolarDB;

namespace PolarTableIndex
{
    public class Scale<Tkey> where Tkey : IComparable
    {
        private readonly Func<object[], Tkey> keyFromIndexRow;
        private readonly Diapason[] diapasons;
        private readonly int minkey = int.MaxValue;
        private readonly int maxkey = 0;
        private const int diapasonsCount = 10000;

        public Scale(Func<object[],Tkey> keyFromIndexRow , PaCell indexCell)
        {
            this.keyFromIndexRow = keyFromIndexRow;
minkey = Convert.ToInt32(keyFromIndexRow((object[])indexCell.Root.Element(0).Get()));
            maxkey = Convert.ToInt32(keyFromIndexRow((object[])indexCell.Root.Element(indexCell.Root.Count() - 1).Get()));
            
            diapasons=new Diapason[diapasonsCount];
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
        /// монотонная функция, сужающая длинные числа от <c>minCombinedkey</c>   до <c>maxCombinedkey</c>    в диапазон от 0 до  diapasonsCount - 1
        /// </summary>
        /// <param name="combined">число приводимое к Int64 Convert.ToInt64</param>
        /// <returns></returns>
        public int GetSubKey(Tkey combined)
        {
            return (int)((diapasonsCount - 1) * Convert.ToDouble(((Convert.ToInt32(combined) - minkey)) / (maxkey - minkey)));
        }
        public void FillDiapasonsNumb(PaCell indexCell)
        {
            indexCell.Root.Scan(o =>
            {
                var subKey = GetSubKey(keyFromIndexRow((object[]) o));
                diapasons[subKey] = diapasons[subKey].AddNumb(1);
                return true;
            });
        }

        public Diapason Search(Tkey key)
        {
            var subKey = GetSubKey(key);
            if (subKey >= 0 && subKey < diapasonsCount)
                return diapasons[subKey];
           return new Diapason();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using PolarDB;

namespace Task15UniversalIndex
{
    public class ScaleCell : IScale
    {
        private long n = 0; // размер шкалы
        private long min, max;
        private PaCell index_cell;
        public PaCell IndexCell {
            get { return index_cell; }
            set
            {
                index_cell = value;
                if (!scell.IsEmpty)
                {
                    this.n = this.scell.Root.Count();
                    min = (int)index_cell.Root.Element(0).Field(0).Get();
                    max = (int)index_cell.Root.Element(index_cell.Root.Count() - 1).Field(0).Get();
                    ToPosition = (int key) => (int)(((long)key - min) * (long)(n - 1) / (max - min));
                }
            } 
        }
        private PaCell scell;
        public ScaleCell(string index_path_name) 
        { 
            //this.index_cell = index_cell;
            
            this.scell = new PaCell(new PTypeSequence(new PType(PTypeEnumeration.longinteger)),
                index_path_name + "_scale.pac", false);
            if (!this.scell.IsEmpty) this.n = this.scell.Root.Count();
        }
        private Func<int, int> ToPosition { get; set; }
        public void Build()
        {
            if (n == 0) Build(index_cell.Root.Count() / 64);
            else Build(n);
        }
        public void Build(long n)
        {
            this.n = n;
            // Вычисление минимума и максимума
            min = (int)index_cell.Root.Element(0).Field(0).Get();
            max = (int)index_cell.Root.Element(index_cell.Root.Count() - 1).Field(0).Get();
            //diapasons = new Diapason[n];
            long[] numbers = new long[n];
            ToPosition = (int key) => (int)(((long)key - min) * (long)(n - 1) / (max - min));
            // Заполнение количеств элементов в диапазонах
            index_cell.Root.Scan((long off, object val) =>
            {
                object[] pair = (object[])val;
                int position = ToPosition((int)pair[0]);
                // Предполагаю, что начальная разметка массива - нули
                numbers[position] += 1;
                return true;
            });
            // Заполнение начал диапазонов
            this.scell.Clear();
            this.scell.Fill(new object[0]);
            long sum = 0;
            for (int i = 0; i < n; i++)
            {
                long start = sum;
                sum += numbers[i];
                scell.Root.AppendElement(start);
            }
            scell.Flush();
        }
        public Diapason GetDiapason(int key)
        {
            int ind = ToPosition(key);
            if (ind < 0 || ind >= n)
            {
                return new Diapason() { start = 0, numb = 0 };
            }
            else
            {
                //return diapasons[ind];
                long start = (long)scell.Root.Element(ind).Get();
                long number = ind < n - 1 ? (long)scell.Root.Element(ind + 1).Get() - start : index_cell.Root.Count() - start;
                return new Diapason() { start = start, numb = number };
            }
        }
        public void Warmup()
        {
            foreach (var v in scell.Root.ElementValues()) ;
        }

    }
}

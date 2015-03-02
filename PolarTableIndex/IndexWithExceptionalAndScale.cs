using System;
using System.Collections.Generic;
using System.Linq;
using PolarDB;

namespace PolarTableIndex
{
    internal class IndexWithExceptionalAndScale
    {

        private readonly PaCell table;
      //  private Func<object, int> fullkey;
        private PaCell fullKeyAndOffset;

        #region scale

        //private PaCell subkeyANdDiapason;
        private Func<int, int> fullkey2Subkey;
        private Diapason[] diapasons;

        #endregion


        #region exceptional keys

        private PaCell exceptionalOffsets;
        private Dictionary<int, Diapason> exceptionalDiapasons = new Dictionary<int, Diapason>();
        private int diapasonsShift;

        /// <summary>
        /// если диапазон короче, то поиск сканированием, если длинее, то используется бинарный поиск.
        /// </summary>
        private static long maxDiapason4Binarysearch=100000;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="table"></param>
        /// <param name="fullkey"></param>
        /// <param name="fullkey2Subkey"> должна быть монотонной</param>
        /// <param name="fullKeyCollisionsMax4Exceptional"></param>
        public IndexWithExceptionalAndScale(string path, PaCell table, Func<object, int> fullkey, Func<int, int> fullkey2Subkey,
            int fullKeyCollisionsMax4Exceptional)
        {
            this.table = table;
       //     this.fullkey = fullkey;
            this.fullkey2Subkey = fullkey2Subkey;
            fullKeyAndOffset = new PaCell(
                new PTypeSequence(
                    new PTypeRecord(
                        new NamedType("fullkey", new PType(PTypeEnumeration.integer)),
                        new NamedType("offset", new PType(PTypeEnumeration.longinteger)))),
                path + @"\fullKeyAndOffset.pac", false);
            fullKeyAndOffset.Clear();
            exceptionalOffsets = new PaCell(new PTypeSequence(new PType(PTypeEnumeration.longinteger)),
                path + @"exceptionalOffsets.pac", false);
             exceptionalOffsets.Clear();
            Dictionary<int, int> counts = new Dictionary<int, int>();


            table.Root.Scan((offset, obj) =>
            {
                var key = fullkey(obj);

                if (counts.ContainsKey(key))
                    counts[key]++;
                else counts.Add(key, 1);
                return true;
            });


            var diapasonsListPositives = new List<Diapason>();
            var diapasonsListNegatives = new List<Diapason>();
            long sum = 0;


            foreach (var countPair in counts)
                if (countPair.Value >= fullKeyCollisionsMax4Exceptional)
                {
                    exceptionalDiapasons.Add(countPair.Key, new Diapason() {numb = countPair.Value, start = sum});
                    sum += countPair.Value;
                }
                else
                {
                    var subkey = fullkey2Subkey(countPair.Key);
                  
                    if (subkey >= 0)
                    {
                        if (diapasonsListPositives.Count <= subkey)
                            diapasonsListPositives.AddRange(Enumerable.Range(0,
                                subkey - diapasonsListPositives.Count + 1)
                                .Select(i => new Diapason()));
                        diapasonsListPositives[subkey] = diapasonsListPositives[subkey].AddNumb(countPair.Value);      
                    }
                    else
                    {
                        if (diapasonsListNegatives.Count <= -subkey)
                            diapasonsListNegatives.AddRange(Enumerable.Range(0,
                                -subkey - diapasonsListNegatives.Count + 1)
                                .Select(i => new Diapason()));
                        diapasonsListNegatives[-subkey] = diapasonsListNegatives[-subkey].AddNumb(countPair.Value);
                    }     
                }
            if (diapasonsListNegatives.Count > 0)
                diapasonsShift = diapasonsListNegatives.Count - 1;
            diapasons = new Diapason[diapasonsListPositives.Count + diapasonsShift];
            if (diapasonsListNegatives.Count > 0)
                Array.Copy(diapasonsListNegatives.ToArray().Reverse().ToArray(), 1, diapasons, 0, diapasonsShift);
                Array.Copy(diapasonsListPositives.ToArray(), 0, diapasons, diapasonsShift, diapasonsListPositives.Count);
           
            sum = 0;
            for (int i = 0; i < diapasons.Length; i++)
            {
                diapasons[i] = diapasons[i].AddStart(sum);   //start must be 0
                sum += diapasons[i].numb;            
            }
            long lastDiapasonCount = diapasons.Length > 0 ? diapasons[diapasons.Length - 1].numb : 0;

            fullKeyAndOffset.Fill(new object[0]);
            exceptionalOffsets.Fill(new object[0]);
            table.Root.Scan((offset, obj) =>
            {
                var key = fullkey(obj);
                Diapason d;
                if (exceptionalDiapasons.TryGetValue(key, out d))
                {
                    long position = d.start + d.numb - counts[key];
                    counts[key]--;
                    if (exceptionalOffsets.Root.Count() <= position)
                    {
                        for (int i = 0; i < position - exceptionalOffsets.Root.Count(); i++)
                            exceptionalOffsets.Root.AppendElement(0);
                        exceptionalOffsets.Root.AppendElement(offset);
                    }
                    else
                        exceptionalOffsets.Root.Element(position).Set(offset);
                }
                else
                {       //diapasons предполагается не пустое, раз есть хоть один не исключительный ключ.
                    var subkey = fullkey2Subkey(key);
                    var diapasonIndex = subkey + diapasonsShift;
                    long position;
                    if (diapasonIndex < diapasons.Length - 1)
                        position = diapasons[diapasonIndex + 1].start - diapasons[diapasonIndex].numb;
                    else position = diapasons[diapasonIndex].start + lastDiapasonCount - diapasons[diapasonIndex].numb;
                    diapasons[diapasonIndex] = diapasons[diapasonIndex].AddNumb(-1);
                
                    if (fullKeyAndOffset.Root.Count() <= position)
                    {
                        for (int i = 0; i < position - fullKeyAndOffset.Root.Count(); i++)
                            fullKeyAndOffset.Root.AppendElement(new object[] {0, 0l});
                        fullKeyAndOffset.Root.AppendElement(new object[] {key, offset});
                    }
                    else
                        fullKeyAndOffset.Root.Element(position).Set(new object[] {key, offset});
                }                
                return true;
            });
            exceptionalOffsets.Flush();
            fullKeyAndOffset.Flush();

            //exceptionalDiapasons.Reverse().Aggregate(exceptionalOffsets.Root.Count(), (i, pair) =>
            //{
            //    var diapason = pair.Value;
            //    diapason.numb = i - diapason.start;
            //    return diapason.start;
            //});
            for (int i = 0; i < diapasons.Length - 1; i++)
                diapasons[i] = diapasons[i].AddNumb(diapasons[i + 1].start - diapasons[i].start); //numb must be 0
            var count = fullKeyAndOffset.Root.Count();
            diapasons[diapasons.Length - 1]=diapasons[diapasons.Length - 1].AddNumb(count - diapasons[diapasons.Length - 1].start);  
            for (int i = 0; i < diapasons.Length; i++)
                fullKeyAndOffset.Root.SortByKey(diapasons[i].start, diapasons[i].numb, o => (long) ((object[]) o)[1],
                    null);
        }

        public IEnumerable<long> GetRowsOffsetsByKey(int fullKey)
        {
            Diapason diapason;
            if (exceptionalDiapasons.TryGetValue(fullKey, out diapason))
            {
                return
                    exceptionalOffsets.Root.Elements(diapason.start, diapason.numb)
                        .Reverse()
                        .Select(entry => (long) entry.Get());
            }
            else
            {
                var subkey = fullkey2Subkey(fullKey);
                if (subkey + diapasonsShift >= diapasons.Length) return Enumerable.Empty<long>();
                diapason = diapasons[subkey + diapasonsShift];
                if (diapason.numb >= maxDiapason4Binarysearch)
                    return fullKeyAndOffset.Root.BinarySearchAll(diapason.start, diapason.numb, entry =>
                    {
                        var l = (int) entry.Field(0).Get();
                        return l > fullKey ? -1 : l == fullKey ? 0 : 1;
                    }).Select(entry => (long) entry.Field(1).Get());
                else
                    return fullKeyAndOffset.Root.Elements(diapason.start, diapason.numb)
                        .Select(entry => (object[]) entry.Get())
                        .SkipWhile(pair => (int) pair[0] < fullKey)
                        .TakeWhile(pair => (int)pair[0] == fullKey)
                        .Select(pair => (long) pair[1]);
            }
        }

        public IEnumerable<PaEntry> GetRowsByKey(int fullKey)
        {
            var entryRow = table.Root.Element(0);
            return GetRowsOffsetsByKey(fullKey).Select(
                offset =>
                {
                    entryRow.offset = offset;
                    return entryRow;
                });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullKeyStart">диапазон включет  это число </param>
        /// <param name="fullKeyEnd">диапазон включет  это число</param>
        /// <returns></returns>
        public IEnumerable<long> GetRowsOffsetsByDiapasonsOfKeys(int fullKeyStart, int fullKeyEnd)
        {
            foreach (var exceptionalDiapason in
                    exceptionalDiapasons.Where(pair => pair.Key >= fullKeyStart && pair.Key <= fullKeyEnd))
            {
                foreach (var offsetEntry in
                        exceptionalOffsets.Root.Elements(exceptionalDiapason.Value.start, exceptionalDiapason.Value.numb)
                    )
                {
                    yield return (long) offsetEntry.Get();
                }
            }
            var subkeyStart = fullkey2Subkey(fullKeyStart) + diapasonsShift;
            var subkeyEnd = fullkey2Subkey(fullKeyEnd) + diapasonsShift;

            if (subkeyStart >= diapasons.Length || subkeyEnd < 0) yield break;
            if (subkeyStart >= 0)
                foreach (var pair in
                    fullKeyAndOffset.Root.Elements(diapasons[subkeyStart].start, diapasons[subkeyStart].numb)
                        .Select(entry => (object[]) entry.Get())
                        .SkipWhile(pair => (int) pair[0] < fullKeyStart)
                        .TakeWhile(pair => (int) pair[0] <= fullKeyEnd))
                    yield return (long) pair[1];
            else
                subkeyStart = -1;
            bool isTail = false;
            if (subkeyEnd < diapasons.Length)
                isTail = true;
            else
                subkeyEnd = diapasons.Length;
            for (int i = subkeyStart + 1; i < subkeyEnd - 1; i++)
                foreach (var offset in
                    fullKeyAndOffset.Root.Elements(diapasons[subkeyEnd].start, diapasons[subkeyEnd].numb)
                        .Select(entry => (long) entry.Field(1).Get()))
                    yield return offset;
            if (isTail)
                foreach (var pair in
                    fullKeyAndOffset.Root.Elements(diapasons[subkeyEnd].start, diapasons[subkeyEnd].numb)
                        .Select(entry => (object[]) entry.Get())
                        .SkipWhile(pair => (int) pair[0] < fullKeyStart)
                        .TakeWhile(pair => (int) pair[0] <= fullKeyEnd))
                    yield return (long) pair[1];
        }
  

    /// <summary>
        /// 
        /// </summary>
        /// <param name="fullKeyStart">диапазон включет  это число </param>
        /// <param name="fullKeyEnd">диапазон включет  это число</param>
        /// <returns></returns>
        public IEnumerable<PaEntry> GetRowsByDiapasonsOfKeys(int fullKeyStart, int fullKeyEnd)
        {
            var entryRow = table.Root.Element(0);
            return GetRowsOffsetsByDiapasonsOfKeys(fullKeyStart, fullKeyEnd).Select(
                    offset =>
                    {
                        entryRow.offset = offset;
                        return entryRow;
                    });
        }

    }
}

using System;
using System.Collections.Generic;
using PolarDB;

namespace PolarTableIndex
{
    internal static class PaEntryRowsByOffsets
    {
        public static void ScanRowsByOffsets(this PaEntry root, IEnumerable<long> offsets, Func<long, object, bool> forEachRow)
        {
            if (root.Count() == 0) return;
            var row = root.Element(0);
            foreach (var offset in offsets)
            {
                row.offset = offset;
                var o = row.Get();
                if (!forEachRow(offset,o)) return;

            }

        }
    
    }
}

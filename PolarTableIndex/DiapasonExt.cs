using PolarDB;

namespace PolarTableIndex
{
    /// <summary>
    /// <example>
    /// array of Diapason:
    /// diapason[i]= diapason[i].Add...
    /// </example>
    /// </summary>
    public static class DiapasonExt
    {
        public static Diapason AddStart(this Diapason d, long adds)
        {
            d.start += adds;
            return d;
        }
        public static Diapason AddNumb(this Diapason d, long adds)
        {
            d.numb += adds;
            return d;
        }
    }
}
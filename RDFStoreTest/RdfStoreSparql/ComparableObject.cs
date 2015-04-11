using System;


public class ComparableObject : IComparable, IComparable<ComparableObject>
    {
        public bool Equals(ComparableObject other)
        {
          //  if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            return tag == other.tag && Equals(Content, other.Content) && string.Equals(Lang, other.Lang) &&
                   Equals(uriCode, other.uriCode);
        }

        public int CompareTo(ComparableObject other)
        {
            if (other.tag != tag) return tag > other.tag ? 1 : -1;
            if (!Equals(uriCode, other.uriCode)) return uriCode.CompareTo(other.uriCode)> 0 ? 1 : -1;
            if (Content != null && !Equals(Content, other.Content)) return uriCode.CompareTo(other.uriCode) > 0 ? 1 : -1;
            if (Lang != null && Lang != other.Lang) return String.Compare(Lang, other.Lang, StringComparison.Ordinal) > 0 ? 1 : -1;
            return 0;
        }                               

        public static bool operator ==(ComparableObject left, ComparableObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ComparableObject left, ComparableObject right)
        {
            return !Equals(left, right);
        }

        public readonly int tag;
        public readonly IComparable Content;
        public readonly string Lang;
        public readonly IComparable uriCode;


        public ComparableObject(int tag, IComparable uriCode, IComparable content, string lang)
        {
            this.tag = tag;
            Content = content;
            Lang = lang;
            this.uriCode = uriCode;
        }

        public int CompareTo(object obj)
        {
            var lit = ((ComparableObject) obj);
            if (lit == null) throw new ArgumentException();

            return CompareTo(lit);
        }

      

    }

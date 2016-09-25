using System;

namespace IntervalTree
{
    public struct Range<T> : IComparable<Range<T>> where T : IComparable<T>
    {
        public T From { get; }
        public T To { get; }

        /// <summary>
        /// Initializes a new <see cref="Range&lt;T&gt;"/> instance.
        /// </summary>
        public Range(T from, T to) : this()
        {
            if (from.CompareTo(to) == 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(from)} cannot be larger than {nameof(to)}");
            }

            From = from;
            To = to;
        }

        /// <summary>
        /// Whether the value is contained in the range. 
        /// Border values are considered inside.
        /// </summary>
        public bool Contains(T value) => value.CompareTo(From) >= 0 && value.CompareTo(To) <= 0;

        /// <summary>
        /// Whether the value is contained in the range. 
        /// Border values are considered outside.
        /// </summary>
        public bool ContainsExclusive(T value) => value.CompareTo(From) > 0 && value.CompareTo(To) < 0;

        public bool Intersects(Range<T> other) => other.To.CompareTo(From) >= 0 && other.From.CompareTo(To) <= 0;
        public bool IntersectsExclusive(Range<T> other) => other.To.CompareTo(From) > 0 && other.From.CompareTo(To) < 0;

        public override string ToString()
        {
            return string.Format("{0} - {1}", From, To);
        }

        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + From.GetHashCode();
            hash = hash * 37 + To.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Returns -1 if this range's From is less than the other, 1 if greater.
        /// If both are equal, To is compared, 1 if greater, -1 if less.
        /// 0 if both ranges are equal.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int CompareTo(Range<T> other)
        {
            if (From.CompareTo(other.From) < 0)
                return -1;
            else if (From.CompareTo(other.From) > 0)
                return 1;
            else if (To.CompareTo(other.To) < 0)
                return -1;
            else if (To.CompareTo(other.To) > 0)
                return 1;
            else
                return 0;
        }
    }

    /// <summary>
    /// Static helper class to create Range instances.
    /// </summary>
    public static class Range
    {
        public static Range<T> Create<T>(T from, T to)
            where T : IComparable<T>
        {
            return new Range<T>(from, to);
        }
    }

    public interface IRangeProvider<T> where T : IComparable<T>
    {
        Range<T> Range { get; }
    }
}

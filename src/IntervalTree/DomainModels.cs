using System;
using System.Collections.Generic;

namespace IntervalTree
{
    public class Person
    {
        public readonly List<Meeting> Meetings;
        public readonly string Name;

        public Person(List<Meeting> meetings, int personNumber)
        {
            Name = $"Person-{personNumber}";
            Meetings = meetings;
        }
    }

    public class MeetingComparer : IComparer<Meeting>
    {
        public int Compare(Meeting x, Meeting y) => x.Range.CompareTo(y.Range);
    }

    public class Meeting : IRangeProvider<DateTime>
    {
        public Meeting(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
            Range = new Range<DateTime>(Start, End);
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public Range<DateTime> Range { get; }

        public override string ToString() => $"{Start.ToString()} {End.ToString()}";
    }

}

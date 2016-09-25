using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IntervalTree
{
    public class Program
    {
        private static Random Randy = new Random();
        public static void Log(string log) => Console.WriteLine(log);

        public static void Main(string[] args)
        {
            var existingMeetingFilter = DateTime.Today.AddDays(4).AddHours(15);
            var doesNotExistFilter = DateTime.Today.AddHours(5);

            WriteFilter(existingMeetingFilter);

            var people = new List<Person>();

            for (int i = 0; i < 100000; i++)
                people.Add(new Person(GenerateDatetimes(), i + 1));

            Time(() =>
            {
                Log("Pure Enumeration");
                var result = PureEnumeration(existingMeetingFilter, people);
                Log(result.ToString());
            });

            var tree = new IntervalTree<DateTime, Meeting>(people.SelectMany(o => o.Meetings), new MeetingComparer());

            Time(() =>
            {
                Log("Interval Tree Exists");
                var result = tree.Exists(new Range<DateTime>(existingMeetingFilter, existingMeetingFilter.AddHours(1)));
                Log(result.ToString());
            });

            Console.ReadLine();
        }

        public static bool PureEnumeration(DateTime filter, List<Person> peopleWithMeetings)
        {
            var from = filter;
            var to = filter.AddHours(1);

            foreach (var person in peopleWithMeetings)
            {
                foreach (var meeting in person.Meetings)
                {
                    if (!(from > meeting.End || to < meeting.Start))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void WriteFilter(DateTime filter) => Log($"Filter: {filter.ToString("u")} - {filter.AddHours(1).ToString("u")}");
        private static void Time(Action func)
        {
            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            func();
            sw.Stop();
            Log($"{sw.ElapsedMilliseconds}ms");
        }

        public static List<Meeting> GenerateDatetimes()
        {
            var list = new List<Meeting>();
            var seed = DateTime.Today.AddHours(8);

            for (int i = 0; i < 6; i++) // days
            {
                var day = seed.AddDays(i);

                var rangeStart = day;

                for (int j = 0; j < 8; j++) // hours
                {
                    var dayAndHour = day.AddHours(j);

                    if (dayAndHour.Hour % 4 == 0 || j == 7)
                    {
                        if (j == 0) continue;

                        var from = rangeStart;
                        var to = rangeStart.AddHours(j);

                        while (from < to)
                        {
                            list.Add(new Meeting(from, from.AddHours(1)));
                            from = from.AddHours(1);
                        }

                        rangeStart = to;
                    }
                }
            }

            return list;
        }
    }
}
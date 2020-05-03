namespace TotalRunTime
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public static class TrackProgress
    {
        public static void Run(Action action, TextWriter tw)
        {
            NullCheck(action);
            NullCheck(tw);

            var sp = new Stopwatch();
            sp.Start();
            action();
            var ts = sp.Elapsed;
            var ellapseTime = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            tw.WriteLine($"Completed action in : {ellapseTime}");

            void NullCheck<T>(T _value)
            {
                if (_value == null) throw new ArgumentNullException();
            }
        }
    }
}

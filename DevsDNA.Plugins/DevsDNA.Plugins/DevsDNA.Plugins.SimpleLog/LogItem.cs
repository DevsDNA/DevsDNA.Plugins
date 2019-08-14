namespace DevsDNA.Plugins.SimpleLog
{
    using System;


    public class LogItem
    {
        public string Message { get; set; }

        public LogLevel Level { get; set; }

        public string TAG { get; set; }

        public DateTimeOffset Date { get; set; }

		public string CallerMemberName { get; set; }
	}
}

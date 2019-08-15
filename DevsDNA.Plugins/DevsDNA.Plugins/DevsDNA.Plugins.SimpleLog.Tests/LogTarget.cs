namespace DevsDNA.Plugins.SimpleLog.Tests
{
	using DevsDNA.Plugins.SimpleLog;
	using System;
    using System.Collections.Generic;

    public class LogTarget : ILogTarget
	{
		public long LineNumber { get; set; } = 0;

		public List<string> LinesWrote { get; set; } = new List<string>();


		public void Write(LogItem logItem)
		{
			string prefix = GetConsoleDebugPrefix(logItem.Level);
			string line = $"\n{prefix} {logItem.Date.ToString("hh:mm:ss")} | {logItem.TAG} | {logItem.Level} | {logItem.CallerMemberName} | {logItem.Message} \n";
			LinesWrote.Add(line);
			Console.WriteLine(line);
		}

		private string GetConsoleDebugPrefix(LogLevel level)
		{
			string preFix = string.Empty;
			switch (level)
			{
				case LogLevel.Info:
					preFix = $"#### {LineNumber++} | INFO:";
					break;
				case LogLevel.Warning:
					preFix = $"#### {LineNumber++} | WARNING:";
					break;
				case LogLevel.Error:
					preFix = $"#### {LineNumber++} | ERROR:";
					break;
				default:
					preFix = string.Empty;
					break;
			}
			return preFix;
		}
	}
}
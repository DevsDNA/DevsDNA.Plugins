﻿namespace DevsDNA.Plugins.SimpleLog
{
    using System.Diagnostics;

	public class DebugTarget : ILogTarget
	{
		private long lineNumber = 0;

		public void Write(LogItem logItem)
		{
			string prefix = GetConsoleDebugPrefix(logItem.Level);
			string line = $"\n{prefix} {logItem.Date.ToString("hh:mm:ss")} | {logItem.TAG} | {logItem.Level} | {logItem.CallerMemberName} | {logItem.Message} \n";
			Debug.WriteLine(line, logItem.TAG);
		}

		private string GetConsoleDebugPrefix(LogLevel level)
		{
			string preFix = string.Empty;
			switch (level)
			{
				case LogLevel.Info:
					preFix = $"#### {lineNumber++} | INFO:";
					break;
				case LogLevel.Warning:
					preFix = $"#### {lineNumber++} | WARNING:";
					break;
				case LogLevel.Error:
					preFix = $"#### {lineNumber++} | ERROR:";
					break;
				default:
					preFix = string.Empty;
					break;
			}
			return preFix;
		}
	}
}
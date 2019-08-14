namespace DevsDNA.Plugins.SimpleLog
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public static class Log
	{
		private static ILogService logService;
		private static ILogTarget logTarget;

		public static ILogService Current
		{
			get
			{
				logService = logService ?? new LogService(logTarget);
				return logService;
			}
		}

		/// <summary>
		/// Change the Target to write. After call this method you must get Current again.
		/// </summary>
		/// <example>
		/// <code>
		/// logService = Log.Current;
		/// //[...]
		/// Log.SetLogTargetAndRestartLog(new LogTarget());
		/// logService = Log.Current;
		/// </code>
		/// </example>
		/// <param name="target">New target for writing log.</param>
		public static ILogService SetLogTargetAndRestartLog(ILogTarget target)
		{
			logTarget = target;
			logService?.Dispose();
			logService = new LogService(logTarget);
			return logService;
		}
	}
}

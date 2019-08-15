namespace DevsDNA.Plugins.SimpleLog
{
	using System;
	using System.Diagnostics;
	using System.Reactive.Subjects;
	using System.Runtime.CompilerServices;
	using System.Text;

	internal class LogService : ILogService
	{
		private IObservable<LogItem> loggerObserver;
		private Subject<LogItem> log;
		private ILogTarget target;
		private bool disposedValue;
		private IDisposable logSubscription;
		private object lockPublish;

		public LogService() : this(null)
		{
		}

		public LogService(ILogTarget target)
		{
			disposedValue = false;
			this.target = target ?? new DebugTarget();
			lockPublish = new object();
			log = new Subject<LogItem>();
			loggerObserver = log;
			SubscribeToLog();
		}
		
		~LogService()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}


		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		public void Log(string message, [CallerMemberName]string callerMemberName = null)
		{
			Log(message, string.Empty, callerMemberName);
		}

		public void Log(string message, string tag, [CallerMemberName]string callerMemberName = null)
		{
			Log(message, tag, LogLevel.Info, callerMemberName);
		}

		public void Log(string message, string tag, LogLevel level, [CallerMemberName]string callerMemberName = null)
		{
			LogItem logItem = CreateLogItem(message, tag, level, callerMemberName);
			Publish(logItem);
		}

		public void LogError(Exception ex, [CallerMemberName]string callerMemberName = null)
		{
			LogError(ex, string.Empty, callerMemberName);
		}

		public void LogError(Exception ex)
		{
			LogError(ex, string.Empty, string.Empty);
		}

		public void LogError(Exception ex, string tag, [CallerMemberName]string callerMemberName = null)
		{
			LogItem logItem = CreateLogItem(GetExceptionData(ex), tag, LogLevel.Error, callerMemberName);
			Publish(logItem);
			GetExceptionData(ex);
		}



		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					logSubscription?.Dispose();
				}
				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}




		private LogItem CreateLogItem(string message, string tag, LogLevel level, string callerMemberName)
		{
			LogItem logItem = new LogItem
			{
				Date = DateTimeOffset.Now,
				Level = level,
				TAG = tag,
				Message = message,
				CallerMemberName = callerMemberName
			};

			return logItem;
		}

		private void Publish(LogItem logItem)
		{
			lock (lockPublish)
			{
				log.OnNext(logItem);
			}
		}

		private void SubscribeToLog()
		{
			logSubscription = loggerObserver.Subscribe(target.Write,
				ex =>
				{
					string text = GetExceptionData(ex);
					Debug.WriteLine(text, "LOG ERROR");
				});
		}

		private string GetExceptionData(Exception ex, string title = "EXCEPTION")
		{
			if (ex == null)
			{
				return string.Empty;
			}

			StringBuilder st = new StringBuilder();
			st.AppendLine($"--{title}--");
			st.AppendLine($"MESSAGE: {ex.Message}");
			st.AppendLine($"TOSTRING: {ex.ToString()}");
			st.AppendLine($"STACKTRACE: {ex.StackTrace}");

			if (ex.InnerException != null)
			{
				st.AppendLine(GetExceptionData(ex.InnerException, "INNER EXCEPTION"));
			}
			return st.ToString();
		}
	}
}

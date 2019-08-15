namespace DevsDNA.Plugins.SimpleLog.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Splat;
    using System;
    using System.Threading.Tasks;

	[TestClass]
	public class SplatLogTests
	{
		[TestMethod]
		public void Given1000LinesOfLog_LogWriteInDefaultTarget()
		{
			// Arrange
			Log.AddServiceToSplat();
			ILogService logService = Locator.Current.GetService<ILogService>();
			Assert.IsNotNull(logService);

			// Act
			for (int i = 0; i < 1000; i++)
			{
				logService.Log($"Line {i.ToString("D4")}");
			}

			// Asset

		}

		[TestMethod]
		public async Task Given4000LinesOfLogIn4Task_LogWriteInDefaultTarget_ThenTasksNotFaulted()
		{
			// Arrange
			Log.AddServiceToSplat();
			ILogService logService = Locator.Current.GetService<ILogService>();
			Assert.IsNotNull(logService);

			// Act
			Task t = Task.WhenAll(
				Task.Run(() => Write1000Lines(logService, 1)),
				Task.Run(() => Write1000Lines(logService, 2)),
				Task.Run(() => Write1000Lines(logService, 3)),
				Task.Run(() => Write1000Lines(logService, 4)));
			await t;

			// Assert
			Assert.IsTrue(t.IsCompleted);
			Assert.IsFalse(t.IsFaulted);
		}

		[TestMethod]
		public void ChangedTargetAndGiven1000LinesOfLog_LogWriteInDefaultTarget_ThenNumberLinesInTargetIsCorrect()
		{
			// Arrange
			Log.AddServiceToSplat();
			LogTarget logTarget = new LogTarget();
			ILogService logService = Locator.Current.GetService<ILogService>();
			logService = Log.SetLogTargetAndRestartLog(logTarget);
			Assert.IsNotNull(logService);

			// Act
			for (int i = 0; i < 1000; i++)
			{
				logService.Log($"Line {i.ToString("D4")}");
			}

			// Asset
			Assert.AreEqual(1000, logTarget.LineNumber);
		}

		[TestMethod]
		public async Task ChangedTargetAndGiven4000LinesOfLogIn4Task_LogWriteInDefaultTarget_ThenNumberLinesInTargetIsCorrect()
		{
			// Arrange
			Log.AddServiceToSplat();
			LogTarget logTarget = new LogTarget();
			ILogService logService = Locator.Current.GetService<ILogService>();
			logService = Log.SetLogTargetAndRestartLog(logTarget);
			Assert.IsNotNull(logService);

			// Act
			Task t = Task.WhenAll(
				Task.Run(() => Write1000Lines(logService, 1)),
				Task.Run(() => Write1000Lines(logService, 2)),
				Task.Run(() => Write1000Lines(logService, 3)),
				Task.Run(() => Write1000Lines(logService, 4)));
			await t;
			await Task.Delay(TimeSpan.FromSeconds(5));

			// Assert
			Assert.IsTrue(t.IsCompleted);
			Assert.IsFalse(t.IsFaulted);
			Assert.AreEqual(logTarget.LineNumber, logTarget.LinesWrote.Count);
			Assert.AreEqual(4000, logTarget.LineNumber);
		}



		private void Write1000Lines(ILogService logService, int thread)
		{
			for (int i = 0; i < 1000; i++)
			{
				logService.Log($"Thread {thread}. Line {i.ToString("D4")}");
			}
		}
	}
}

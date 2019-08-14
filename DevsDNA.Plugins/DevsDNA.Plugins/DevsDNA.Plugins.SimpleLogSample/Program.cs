namespace DevsDNA.Plugins.SimpleLogSample
{
    using DevsDNA.Plugins.SimpleLog;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Program
	{
		static void Main(string[] args)
		{
			LogTest logTest = new LogTest();
			logTest.Write1000Lines();
			Console.WriteLine("-------------------------------------------------------------");
			Task.WaitAll(logTest.In4ThreadsWrite1000LineEachOne());
			Console.WriteLine("-------------------------------------------------------------");

			Console.WriteLine("-------------------------------------------------------------");
			logTest.ChangeTarget();
			Console.WriteLine("-------------------------------------------------------------");
			logTest.Write1000Lines();
			Console.WriteLine("-------------------------------------------------------------");
			Task.WaitAll(logTest.In4ThreadsWrite1000LineEachOne());
			Console.WriteLine("-------------------------------------------------------------");
			Console.ReadLine();
		}
	}

	public class LogTest
	{
		private ILogService logService;

		public LogTest()
		{
			logService = Log.Current;
		}

		public void Write1000Lines()
		{
			for (int i = 0; i < 1000; i++)
			{
				logService.Log($"Line {i.ToString("D4")}");
			}
		}

		public async Task In4ThreadsWrite1000LineEachOne()
		{
			await Task.WhenAll(Task.Run(() => Write1000Lines(1)), Task.Run(() => Write1000Lines(2)), Task.Run(() => Write1000Lines(3)), Task.Run(() => Write1000Lines(4)));
		}
		
		public void ChangeTarget()
		{			
			logService = Log.SetLogTargetAndRestartLog(new LogTarget());
		}



		private void Write1000Lines(int thread)
		{
			for (int i = 0; i < 1000; i++)
			{
				logService.Log($"Thread {thread}. Line {i.ToString("D4")}");
			}
		}
	}
}

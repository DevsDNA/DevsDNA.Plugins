namespace DevsDNA.Plugins.SimpleLog
{

	public interface ILogTarget
	{
		void Write(LogItem logItem);
	}
}

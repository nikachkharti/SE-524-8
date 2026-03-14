namespace Lecture28.InterfaceSegregation
{
    public interface IFileLogger
    {
        void Log(string filePath, string content);
    }

    public interface IConsoleLogger
    {
        void Log(string content);
    }

    public class ConsoleLogger : IFileLogger, IConsoleLogger
    {
        public void Log(string filePath, string content)
        {
            throw new NotImplementedException();
        }

        public void Log(string content)
        {
            throw new NotImplementedException();
        }
    }

}

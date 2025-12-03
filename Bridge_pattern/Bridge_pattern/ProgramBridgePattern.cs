using System;

namespace Bridge_pattern
{
    public interface ILogOutput
    {
        void OutputLog(string text);
    }

    public abstract class LogProcessor
    {
        protected ILogOutput _outputChannel;

        public LogProcessor(ILogOutput outputChannel) => _outputChannel = outputChannel;

        public abstract void ProcessLogEntry(string logMessage);
    }

    public class StoreLogProcessor : LogProcessor
    {
        private string _componentName;

        public StoreLogProcessor(ILogOutput outputChannel, string componentName) : base(outputChannel)
            => _componentName = componentName;

        public override void ProcessLogEntry(string logMessage)
            => _outputChannel.OutputLog($"[{_componentName}] {DateTime.Now:HH:mm:ss} - {logMessage}");
    }

    public class ConsoleOutput : ILogOutput
    {
        public void OutputLog(string text) => Console.WriteLine(text);
    }

    public class FileOutput : ILogOutput
    {
        private string _filePath;

        public FileOutput(string filePath) => _filePath = filePath;

        public void OutputLog(string text)
            => System.IO.File.AppendAllText(_filePath, text + Environment.NewLine);
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var consoleOutput = new ConsoleOutput();
            var fileOutput = new FileOutput("application.log");

            var orderProcessor = new StoreLogProcessor(consoleOutput, "ORDER");
            var userProcessor = new StoreLogProcessor(fileOutput, "USER");

            orderProcessor.ProcessLogEntry("New order #123");
            userProcessor.ProcessLogEntry("New user registration completed");

            orderProcessor = new StoreLogProcessor(fileOutput, "ORDER");
            orderProcessor.ProcessLogEntry("Order #123 processed successfully");
        }
    }
}






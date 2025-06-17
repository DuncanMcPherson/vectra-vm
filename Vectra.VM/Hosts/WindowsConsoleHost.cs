using Vectra.VM.Execution;

namespace Vectra.VM.Hosts;

public class WindowsConsoleHost : IExecutionHost
{
    public void WriteLine(string message) => Console.WriteLine(message);
    public string ReadLine() => Console.ReadLine() ?? string.Empty;
}
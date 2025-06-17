using Vectra.VM.Execution;

namespace Vectra.VM.Hosts;

public class NullHost : IExecutionHost
{
    public void WriteLine(string message)
    {
        // Do Nothing
    }

    public string ReadLine()
    {
        return string.Empty;
    }
}
namespace Vectra.VM.Execution;

/// <summary>
/// Represents an execution host responsible for handling input and output operations.
/// </summary>
public interface IExecutionHost
{
    /// <summary>
    /// Writes a line of text to the output.
    /// </summary>
    /// <param name="message">The message to write to the output.</param>
    void WriteLine(string message);

    /// <summary>
    /// Reads a single line of text from the input source.
    /// </summary>
    /// <returns>
    /// A string containing the text of the line that was read.
    /// </returns>
    string ReadLine();
}
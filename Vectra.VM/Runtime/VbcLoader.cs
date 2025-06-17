using Vectra.Bytecode;
using Vectra.Bytecode.Models;

namespace Vectra.VM.Runtime;

/// <summary>
/// Provides functionality to load and read VbcProgram objects from specific file paths.
/// </summary>
/// <remarks>
/// The VbcLoader class is a static utility designed to handle the loading of Vectra Bytecode program files.
/// This class simplifies the process of loading bytecode programs from external files.
/// </remarks>
public static class VbcLoader
{
    /// <summary>
    /// Loads a VBC program from a specified file path.
    /// </summary>
    /// <param name="path">The file path to the VBC program file.</param>
    /// <returns>Returns a <c>VbcProgram</c> object representing the loaded VBC program.</returns>
    public static VbcProgram Load(string path)
    {
        return BytecodeReader.Read(path);
    }
}
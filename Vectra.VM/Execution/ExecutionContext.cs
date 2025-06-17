using Vectra.Bytecode.Models;

namespace Vectra.VM.Execution;

/// <summary>
/// Represents the context for execution within a virtual machine.
/// Provides a scope for managing local variables and an operand stack for computations.
/// </summary>
public class ExecutionContext
{
    /// <summary>
    /// Gets the collection of local variables and their associated values within the execution context.
    /// </summary>
    /// <remarks>
    /// The Locals property provides a dictionary that maps string keys representing variable names
    /// to their corresponding object values. This acts as a storage for variables that are local
    /// to the current execution environment.
    /// </remarks>
    /// <value>
    /// A dictionary containing the names and values of local variables.
    /// </value>
    public Dictionary<string, object> Locals { get; } = new();

    /// <summary>
    /// Represents the stack used for operands during execution within the execution context.
    /// </summary>
    /// <remarks>
    /// The OperandStack is utilized to store and retrieve operands during the execution
    /// of operations in the virtual machine. It is designed to hold any object type
    /// and supports standard stack operations such as push and pop.
    /// </remarks>
    public Stack<object> OperandStack { get; } = new();
    
    public VbcMethod Method { get; }
    public int InstructionPointer { get; set; }

    public ExecutionContext(VbcMethod method)
    {
        Method = method;
    }
}
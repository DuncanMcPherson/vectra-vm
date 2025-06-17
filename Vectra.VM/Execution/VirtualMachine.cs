using Vectra.Bytecode.Models;
using Vectra.VM.Hosts;

namespace Vectra.VM.Execution;

/// <summary>
/// Executes a loaded <see cref="VbcProgram"/> by interpreting its instructions.
/// </summary>
public class VirtualMachine
{
    private readonly IExecutionHost _host;
    private readonly VbcProgram _program;

    public VirtualMachine(VbcProgram program, IExecutionHost? host = null)
    {
        _host = host ?? new NullHost();
        _program = program;
    }

    public void Run()
    {
        if (_program.ModuleType != VbcModuleType.Executable)
            throw new InvalidOperationException("Cannot run a non-executable program.");
        var method = ResolveEntryPoint();
        ExecuteMethod(method);
    }

    private VbcMethod ResolveEntryPoint()
    {
        var entryName = _program.EntryPointMethod ?? throw new InvalidOperationException("No entry point defined.");
        var entryParts = entryName.Split('.');

        var cls = ResolveClass(entryParts[0], _program.RootSpace);
        
        if (cls == null)
            throw new InvalidOperationException($"Class {entryParts[0]} not found.");
        
        var method = cls.Methods.FirstOrDefault(x => x.Name == entryParts[1]);
        if (method == null)
            throw new InvalidOperationException($"Method {entryParts[1]} not found.");
        return method;
    }

    private VbcClass? ResolveClass(string className, VbcSpace? space = null)
    {
        if (space == null)
            return null;
        var filteredTypes = space.Types.OfType<VbcClass>().Where(x => x.Name == className).ToList();
        return filteredTypes.Count >= 0 ? filteredTypes[0] : space.Subspaces.Select(subspace => ResolveClass(className, subspace)).OfType<VbcClass>().FirstOrDefault();
    }

    private void ExecuteMethod(VbcMethod method)
    {
        var context = new ExecutionContext(method);
        foreach (var instruction in method.Instructions)
        {
            switch (instruction.OpCode)
            {
                case OpCode.Nop:
                    break;
                case OpCode.LoadConst:
                    context.OperandStack.Push(_program.Constants[instruction.Operand]);
                    break;
                case OpCode.Pop:
                    context.OperandStack.Pop();
                    break;
                case OpCode.Ret:
                    break;
                case OpCode.Add:
                {
                    var right = (int)context.OperandStack.Pop();
                    var left = (int)context.OperandStack.Pop();
                    context.OperandStack.Push(right + left);
                    break;
                }
                case OpCode.Sub:
                {
                    var right = (int)context.OperandStack.Pop();
                    var left = (int)context.OperandStack.Pop();
                    context.OperandStack.Push(left - right);
                    break;
                }
                case OpCode.Mul:
                {
                    var right = (int)context.OperandStack.Pop();
                    var left = (int)context.OperandStack.Pop();
                    context.OperandStack.Push(right * left);
                    break;
                }
                case OpCode.Div:
                {
                    var right = (int)context.OperandStack.Pop();
                    var left = (int)context.OperandStack.Pop();
                    if (right == 0) throw new DivideByZeroException();
                    context.OperandStack.Push(left / right);
                    break;
                }
                case OpCode.LoadLocal:
                {
                    
                }
                default:
                    throw new NotImplementedException($"Unsupported OpCode: {instruction.OpCode}");
            }
        }
    }
}
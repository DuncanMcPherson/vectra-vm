using Vectra.Bytecode.Models;
using Vectra.VM.Hosts;
using Vectra.VM.Runtime;
using Vectra.VM.Runtime.Extensions;

namespace Vectra.VM.Execution;

/// <summary>
/// Executes a loaded <see cref="VbcProgram"/> by interpreting its instructions.
/// </summary>
public class VirtualMachine
{
    private readonly IExecutionHost _host; // this will get used eventually, for printing output;
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

    private object? ExecuteMethod(VbcMethod method, object[]? callArgs = null)
    {
        var context = new ExecutionContext(method, callArgs);
        foreach (var instruction in method.Instructions)
        {
            switch (instruction.OpCode)
            {
                case OpCode.Nop:
                    break;
                case OpCode.LoadConst:
                    context.OperandStack.Push(_program.Constants[instruction.Operand]);
                    break;
                case OpCode.LoadLocal:
                {
                    var localName = (string)_program.Constants[instruction.Operand];
                    var localValue = context.Locals[localName];
                    context.OperandStack.Push(localValue);
                    break;
                }
                case OpCode.StoreLocal:
                {
                    var localName = (string)_program.Constants[instruction.Operand];
                    var localValue = context.OperandStack.Pop();
                    context.Locals[localName] = localValue;
                    break;
                }
                case OpCode.Pop:
                    context.OperandStack.Pop();
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
                case OpCode.Call:
                    var packed = instruction.Operand;
                    var methodNameIndex = packed & 0x00ffffff;
                    var arity = (packed >> 24) & 0xff;
                    var methodName = _program.Constants[methodNameIndex] as string ?? throw new InvalidOperationException("Method name not found.");
                    var args = new object[arity + 1];
                    for (var i = arity; i > 0; i--)
                    {
                        args[i] = context.OperandStack.Pop();
                    }
                    var targetInstance = context.OperandStack.Pop();
                    args[0] = targetInstance;
                    if (targetInstance is not VbcInstance instance)
                        throw new InvalidOperationException("Target instance is not an instance.");
                    var classMethod = instance.ClassDef.GetMethod(methodName, arity);
                    if (classMethod == null)
                        throw new InvalidOperationException($"Method {methodName} not found.");
                    var result = ExecuteMethod(classMethod, args);
                    context.OperandStack.Push(result!);
                    break;
                case OpCode.Ret:
                    var value = context.OperandStack.Count > 0 ? context.OperandStack.Pop() : null;
                    return value;
                case OpCode.Eq:
                    PushBoolResult(context, Equals);
                    break;
                case OpCode.Neq:
                    PushBoolResult(context, (x, y) => !Equals(x, y));
                    break;
                case OpCode.Gt:
                    PushBoolResult(context, (x, y) => (dynamic)x > (dynamic)y);
                    break;
                case OpCode.Geq:
                    PushBoolResult(context, (x, y) => (dynamic)x >= (dynamic)y);
                    break;
                case OpCode.Lt:
                    PushBoolResult(context, (x, y) => (dynamic)x < (dynamic)y);
                    break;
                case OpCode.Leq:
                    PushBoolResult(context, (x, y) => (dynamic)x <= (dynamic)y);
                    break;
                default:
                    throw new NotImplementedException($"Unsupported OpCode: {instruction.OpCode}");
            }
        }

        return null;
    }

    private static void PushBoolResult(ExecutionContext context, Func<object, object, bool> comparer)
    {
        var right = context.OperandStack.Pop();
        var left = context.OperandStack.Pop();
        context.OperandStack.Push(comparer(left, right));   
    }
}
using Vectra.Bytecode.Models;

namespace Vectra.VM.Runtime.Extensions;

public static class VbcClassExtensions
{
    public static VbcMethod? GetMethod(this VbcClass cls, string name, int arity)
    {
        var methodsWithName = cls.Methods.Where(x => x.Name == name);
        var methodsWithArity = methodsWithName.Where(x => x.Parameters.Count == arity);
        return methodsWithArity.FirstOrDefault();
    }
}
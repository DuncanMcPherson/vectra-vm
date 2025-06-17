using Vectra.Bytecode.Models;

namespace Vectra.VM.Runtime;

public class VbcInstance
{
    public VbcClass ClassDef { get; }
    public Dictionary<string, object> Fields { get; }
    
    public VbcInstance(VbcClass classDef)
    {
        ClassDef = classDef;
        Fields = new Dictionary<string, object>();
    }
}
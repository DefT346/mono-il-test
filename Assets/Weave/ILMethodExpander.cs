using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ILMethodExpander
{ 
    public static void Weave(AssemblyDefinition asmDef)
    {
        var methodName = "Command";

        var sourceFuncMethod = MethodReslove(asmDef, "Client", methodName);

        var funcMethod = MethodReslove(asmDef, "Server", methodName);

        var processor = sourceFuncMethod.Body.GetILProcessor();
        sourceFuncMethod.Body.Instructions.RemoveAt(sourceFuncMethod.Body.Instructions.Count - 1);
        processor.Emit(OpCodes.Callvirt, funcMethod);
        processor.Emit(OpCodes.Ret);

        PrintInstructions(sourceFuncMethod);
    }

    private static void PrintInstructions(MethodDefinition md)
    {
        var result = $"{md.FullName} Instructions:\n";
        foreach (var instr in md.Body.Instructions)
            result += $"OpCode: {instr.OpCode} -> {instr.Operand}\n";

        Debug.Log(result);
    }

    private static MethodDefinition MethodReslove(AssemblyDefinition asmDef, string className, string methodName)
    {
        TypeDefinition type = asmDef.MainModule.GetType(className);
        MethodDefinition method = type.Methods.Single(m => m.Name == methodName);
        return method;
    }
}

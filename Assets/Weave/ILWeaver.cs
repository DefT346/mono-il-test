using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEngine;
using UnityEditor;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;

public class ILWeaver
{
    [InitializeOnLoadMethod]
    static void init()
    {
        List<string> assemblies = new List<string>();
        UnityEditor.Compilation.CompilationPipeline.compilationStarted += (o) => {
            Debug.Log("[ILWeaver] Compilation started");
            EditorApplication.LockReloadAssemblies();
            assemblies.Clear();
        };

        UnityEditor.Compilation.CompilationPipeline.assemblyCompilationFinished += (asmName, message) => {
            assemblies.Add(asmName);
            Debug.Log("[ILWeaver] Assembly compilation finished");
        };

        UnityEditor.Compilation.CompilationPipeline.compilationFinished += (o) => {
            Debug.Log("[ILWeaver] Weaving...");
            foreach (var asmName in assemblies)
            {
                AssemblyDefinition asmDef = AssemblyDefinition.ReadAssembly(asmName, new ReaderParameters()
                {
                    ReadWrite = true,
                });

                ILMethodExpander.Weave(asmDef);

                asmDef.Write();
                asmDef.Dispose();
            }
            EditorApplication.UnlockReloadAssemblies();
            Debug.Log("[ILWeaver] Compilation finished");
        };

        Debug.Log("[ILWeaver] initilized");
    }
}

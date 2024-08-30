using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEditor.Build.Player;

public class CompileDLLHelper
{
    [MenuItem("HTools/CompileDlls")]
    public static void CompileDll()
    {

        var tempOutputPath = $"{Application.dataPath}/../Dlls";
        Directory.CreateDirectory(tempOutputPath);

        ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
        scriptCompilationSettings.group = BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneWindows64);
        scriptCompilationSettings.target = BuildTarget.StandaloneWindows64;

        PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, tempOutputPath);
    }
}

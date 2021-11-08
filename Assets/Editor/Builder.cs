using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Author: Nicolas Cabrera Lettiere
/// Github: https://github.com/nclettiere
/// </summary>
class Builder
{
    private static string PathCombine(string path1, string path2)
    {
        if (Path.IsPathRooted(path2))
        {
            path2 = path2.TrimStart(Path.DirectorySeparatorChar);
            path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
        }

        return Path.Combine(path1, path2);
    }
    private static string PathDeploy =>
        PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "/PacoBuilds/Win64/PacoGame.exe");

    static void build() {
        Debug.Log($"Build de Paco se generara en: {PathDeploy}");
        
        string[] scenes = {
            "Assets/Scenes/Main/Level1.unity",
            "Assets/Scenes/Main/Level2.unity",
            "Assets/Scenes/Main/Level2.1.unity",
            "Assets/Scenes/Main/Level3.unity",
            "Assets/Scenes/Cutscenes/CutScene0.unity",
            "Assets/Scenes/Creditos/Creditos.unity",
            "Assets/Scenes/Cutscenes/CutScene1.unity"
        };

        //string pathToDeploy = @"C:\Users\Nicolini\Documents\builds\Win64\PacoGame.exe";       

        BuildPipeline.BuildPlayer(scenes, PathDeploy, BuildTarget.StandaloneWindows64, BuildOptions.None);      
    }
}
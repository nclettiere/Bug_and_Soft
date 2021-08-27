using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SaveSystem.Data;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveSystem
    {
        private static readonly string WindowsQuickSavePath = Application.persistentDataPath + "/saves/savegame_quick.bin";
        private static readonly string WindowsSavePath = Application.persistentDataPath + "/saves/savegame_";
        
        public static bool SaveGame(ref PlayerData playerData, int slot)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            EnsureDirs();
            var savePath = GetSaveSlot(slot);
            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(savePath, FileMode.Create);
            binaryFormatter.Serialize(fileStream, playerData);
            fileStream.Close();
            return true;
#endif
        }
        
        public static bool QuickSaveGame(ref PlayerData playerData)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            EnsureDirs();
            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(WindowsQuickSavePath, FileMode.Create);
            binaryFormatter.Serialize(fileStream, playerData);
            fileStream.Close();
            return true;
#endif
        }
        
        public static PlayerData LoadSaveGame(int slot)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            EnsureDirs();
            var savePath = GetSaveSlot(slot);
            if (File.Exists(savePath))
            {
                var binaryFormatter = new BinaryFormatter();
                var fileStream = new FileStream(savePath, FileMode.Open);
                var playerData = binaryFormatter.Deserialize(fileStream) as PlayerData;

                return playerData;
            }
            
            Debug.LogError("Savegame file does not exist.");
            return null;
#endif
        }
        
        public static PlayerData LoadQuickSaveGame()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            EnsureDirs();
            if (File.Exists(WindowsQuickSavePath))
            {
                var binaryFormatter = new BinaryFormatter();
                var fileStream = new FileStream(WindowsQuickSavePath, FileMode.Open);
                var playerData = binaryFormatter.Deserialize(fileStream) as PlayerData;

                return playerData;
            }
            
            Debug.LogError("Savegame file does not exist.");
            return null;
#endif
        }

        private static string GetSaveSlot(int slot)
        {
            return WindowsSavePath + $"{slot:000}" + ".bin"; // example /saves/savegame_005.bin
        }

        private static void EnsureDirs()
        {
            DirectoryInfo di = Directory.CreateDirectory(Application.persistentDataPath + "/saves");
            Debug.Log("The directory was created successfully at " + Directory.GetCreationTime(Application.persistentDataPath + "/saves"));   
        }
    }
}
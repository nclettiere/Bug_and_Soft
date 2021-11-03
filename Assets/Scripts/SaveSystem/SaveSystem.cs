using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SaveSystem.Data;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveSystem
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        private static readonly string QuickSavePath = Application.persistentDataPath + "/saves/savegame_quick.bin";
        private static readonly string SlotSavePath = Application.persistentDataPath + "/saves/savegame_";
#elif  UNITY_EDITOR_LINUX || UNITY_EDITOR_LINUX
        private static readonly string QuickSavePath = Application.persistentDataPath + "\\saves\\savegame_quick.bin";
        private static readonly string SlotSavePath = Application.persistentDataPath + "\\saves\\savegame_";
#endif
        public static bool SaveGame(ref PlayerData playerData, int slot)
        {
            EnsureDirs();
            var savePath = GetSaveSlot(slot);
            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(savePath, FileMode.Create);
            binaryFormatter.Serialize(fileStream, playerData);
            fileStream.Close();
            return true;
        }
        
        public static bool QuickSaveGame(ref GameData gameData)
        {
            try
            {
                EnsureDirs();
                var binaryFormatter = new BinaryFormatter();
                var fileStream = new FileStream(QuickSavePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, gameData);
                fileStream.Close();
            }
            catch(Exception e)
            {
                Debug.LogError($"Could not save the game\n{e.Message}");
            }

            return true;
        }
        
        public static PlayerData LoadSaveGame(int slot)
        {
            EnsureDirs();
            var savePath = GetSaveSlot(slot);
            if (File.Exists(savePath))
            {
                var binaryFormatter = new BinaryFormatter();
                var fileStream = new FileStream(savePath, FileMode.Open);
                var playerData = binaryFormatter.Deserialize(fileStream) as PlayerData;
                fileStream.Close();
                return playerData;
            }
            
            Debug.LogError("Savegame file does not exist.");
            return null;
        }
        
        public static GameData LoadQuickSaveGame()
        {
            EnsureDirs();
            if (File.Exists(QuickSavePath))
            {
                var binaryFormatter = new BinaryFormatter();
                var fileStream = new FileStream(QuickSavePath, FileMode.Open);
                var gameData = binaryFormatter.Deserialize(fileStream) as GameData;
                fileStream.Close();

                Debug.Log("Cargando save desde: " + QuickSavePath);
                return gameData;
            }
            
            Debug.LogError("Savegame no existe;");
            return null;
        }

        public static PlayerData[] LoadSlotsData()
        {
            PlayerData[] slotDatas = new PlayerData[3];
            
            // Checkea la data de los 3 slots
            for (int i = 0; i < 3; i++)
            {
                slotDatas[i] = LoadSaveGame(i);
            }

            return slotDatas;
        }

        private static string GetSaveSlot(int slot)
        {
            return SlotSavePath + $"{slot:000}" + ".bin"; // example: /saves/savegame_003.bin
        }

        private static void EnsureDirs()
        {
            DirectoryInfo di = Directory.CreateDirectory(Application.persistentDataPath + "/saves");
            Debug.Log("The directory was created successfully at " + Directory.GetCreationTime(Application.persistentDataPath + "/saves"));   
        }
    }
}
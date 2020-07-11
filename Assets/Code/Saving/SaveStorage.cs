using UnityEngine;

namespace RedsAndBlues
{
    public class SaveStorage
    {
        private const string SaveKey = "SaveData";

        public bool HaveSavedData { get; private set; }

        public SaveStorage()
        {
            HaveSavedData = PlayerPrefs.HasKey(SaveKey);
        }

        public void Save(string data)
        {
            HaveSavedData = true;
            PlayerPrefs.SetString(SaveKey, data);
            PlayerPrefs.Save();
        }

        public string Load()
        {
            return PlayerPrefs.GetString(SaveKey);
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            HaveSavedData = false;
        }
    }
}
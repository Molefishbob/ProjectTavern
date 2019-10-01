using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class SerializationManager
{
    public static SettingsData LoadedSettings;
    public static SaveData LoadedSave;

    private static bool _initialized;
    private static string _path;

    private static void Init()
    {
        _path = Application.persistentDataPath;
        LoadedSettings = new SettingsData();
        _initialized = true;
    }

    public static void LoadSettings(string filename)
    {
        if (!_initialized)
            Init();

        if (File.Exists(_path + Path.DirectorySeparatorChar + filename))
        {
            string data = File.ReadAllText(_path + Path.DirectorySeparatorChar + filename);
            LoadedSettings = JsonConvert.DeserializeObject<SettingsData>(data);

            Debug.Log(LoadedSettings.Volume.MusicMute);
        }
    }


    public static void SaveSettings(string filename)
    {
        if (!_initialized)
            Init();

        File.WriteAllText(_path + Path.DirectorySeparatorChar + filename, JsonConvert.SerializeObject(LoadedSettings, Formatting.Indented));
    }

    #region DebugStuff

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Load Settings", priority = 2)]
    public static void DEBUGLoadSettings()
    {
        LoadSettings("Settings.json");
    }

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Save Settings", priority = 3)]
    public static void DEBUGSaveSettings()
    {
        SaveSettings("Settings.json");
    }

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Log Path", priority = 1)]
    public static void LogPath()
    {
        if (UnityEditor.EditorApplication.isPlaying && !_initialized)
            Init();
        if (_initialized)
            Debug.Log(_path);
    }

    #endregion

    #region Structures

    public class SettingsData
    {
        [System.Serializable]
        public struct VolumeSettings
        {
            public float SFX;
            public float Music;
            public bool SFXMute;
            public bool MusicMute;

            public VolumeSettings(float SFX = 1, float Music = 1, bool SFXMute = false, bool MusicMute = false)
            {
                this.SFX = SFX;
                this.Music = Music;
                this.SFXMute = SFXMute;
                this.MusicMute = MusicMute;
            }
        }

        public VolumeSettings Volume = new VolumeSettings(1, 1, false, false);
    }

    public class SaveData
    {

    }
    
    #endregion
}

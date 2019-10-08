using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;


public static class SerializationManager
{
    /// <summary>
    /// Current on memory Settings
    /// </summary>
    public static SettingsData LoadedSettings;
    /// <summary>
    /// Current on memory save
    /// </summary>
    public static SaveData LoadedSave;

    private static bool _initialized;
    private static string _path;

    private static void Init()
    {
        _path = Application.persistentDataPath;
        LoadedSettings = MakeDefaultSettings();
        _initialized = true;
    }

    /// <summary>
    /// Load the settings from the saved file.
    /// Loaded settings can be found from this.LoadedSettings
    /// This function might change to just return the settings instead
    /// If file is not found, loads default settings.
    /// </summary>
    /// <param name="filename">in the form of <c>NAME.json</c></param>
    public static void LoadSettings(string filename)
    {
        if (!_initialized)
            Init();

        if (File.Exists(_path + Path.DirectorySeparatorChar + filename))
        {
            string data = File.ReadAllText(_path + Path.DirectorySeparatorChar + filename);
            LoadedSettings = JsonConvert.DeserializeObject<SettingsData>(data);

            // ToDo: Check the data integrity
            Debug.Log("Settings Loaded");
        }
        else
        {
            Debug.LogWarning(filename + " not found!\nUsing currently loaded Settings");
        }
    }

    /// <summary>
    /// Saves the current <c>LoadedSettings</c> to a file
    /// </summary>
    /// <param name="filename">in the form of <c>NAME.json</c></param>
    public static void SaveSettings(string filename)
    {
        if (!_initialized)
            Init();

        File.WriteAllText(_path + Path.DirectorySeparatorChar + filename, JsonConvert.SerializeObject(LoadedSettings, Formatting.Indented));
    }

    /// <summary>
    /// Delete the settings file and create a default one
    /// </summary>
    /// <param name="filename">in the form of <c>NAME.json</c></param>
    public static void DeleteAndMakeDefaultSettings(string filename)
    {
        if (!_initialized)
            Init();

        if (File.Exists(_path + Path.DirectorySeparatorChar + filename))
        {
            LoadedSettings = MakeDefaultSettings();
            SaveSettings(filename);
        }
    }

    private static SettingsData MakeDefaultSettings()
    {
        SettingsData data = new SettingsData();
        data.Resolution = Screen.currentResolution;
        data.Fullscreen = Screen.fullScreen;
        data.FullScreenMode = Screen.fullScreenMode;
        return data;
    }

    #region DebugStuff

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Log Path", priority = 1)]
    public static void LogPath()
    {
        if (UnityEditor.EditorApplication.isPlaying && !_initialized)
            Init();
        if (_initialized)
            Debug.Log(_path);
    }

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Load Settings", priority = 2)]
    public static void DEBUGLoadSettings()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            LoadSettings("Settings.json");
        else
            Debug.LogError("Editor needs to be playing to do this!");
    }

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Save Settings", priority = 3)]
    public static void DEBUGSaveSettings()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            SaveSettings("Settings.json");
        else
            Debug.LogError("Editor needs to be playing to do this!");
    }

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Delete Settings", priority = 4)]
    public static void DEBUGDeleteSettings()
    {
        if (UnityEditor.EditorApplication.isPlaying && !_initialized)
            Init();
        if (_initialized)
            DeleteAndMakeDefaultSettings("Settings.json");
    }

    #endregion

    #region Structures

    /// <summary>
    /// Holds the setting data, which includes resolution, volumes and Keybinds
    /// </summary>
    public class SettingsData
    {
        /// <summary>
        /// Holds the volume controls for SFX, Music and Master.
        /// Includes mutes.
        /// </summary>
        [System.Serializable]
        public struct VolumeSettings
        {
            /// <summary>
            /// The value gets clamped between 0 and 1, to represent percents
            /// </summary>
            public float SFX
            {
                get => _sfx;
                set
                {
                    _sfx = Mathf.Clamp(value, 0, 1);
                }
            }
            private float _sfx;

            /// <summary>
            /// The value gets clamped between 0 and 1, to represent percents
            /// </summary>
            public float Music
            {
                get => _music;
                set
                {
                    _music = Mathf.Clamp(value, 0, 1);
                }
            }
            private float _music;
            
            /// <summary>
            /// The value gets clamped between 0 and 1, to represent percents
            /// </summary>
            public float Master
            {
                get => _master;
                set
                {
                    _master = Mathf.Clamp(value, 0, 1);
                }
            }
            private float _master;

            public bool SFXMute;
            public bool MusicMute;
            public bool MasterMute;

            /// <summary>
            /// All the values will be clamped between 0 and 1
            /// </summary>
            /// <param name="SFX">Sound effect volume in percents</param>
            /// <param name="Music">Music volume in percents</param>
            /// <param name="Master">Master volume in percents</param>
            /// <param name="MasterMute">True = No sound</param>
            /// <param name="SFXMute">True = No sound</param>
            /// <param name="MusicMute">True = No sound</param>
            public VolumeSettings(float SFX = 1, float Music = 1, float Master = 1, bool MasterMute = false, bool SFXMute = false, bool MusicMute = false)
            {
                _sfx = Mathf.Clamp(SFX, 0, 1);
                _music = Mathf.Clamp(Music, 0, 1);
                _master = Mathf.Clamp(Master, 0, 1);
                this.SFXMute = SFXMute;
                this.MusicMute = MusicMute;
                this.MasterMute = MasterMute;
            }
        }

        /// <summary>
        /// Holds the volume controls for SFX, Music and Master.
        /// Includes mutes.
        /// Defaults itself to all full and no mutes
        /// </summary>
        public VolumeSettings Volume = new VolumeSettings(1, 1, 1, false, false, false);

        public Resolution Resolution;
        public bool Fullscreen = true;
        public FullScreenMode FullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public class SaveData
    {
        public string ProfileName = "";
        public int LastLevelCleared = 0;
        public float Money = 0;
    }
    
    #endregion
}

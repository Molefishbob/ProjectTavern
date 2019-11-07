using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;


public static class SerializationManager
{
    /// <summary>
    /// Current on memory Settings
    /// 
    /// This holds everything that has been loaded from the file
    /// Or is going to be saved to the file.
    /// <c>LoadSettings(filename)</c> overrides this
    /// <c>SaveSettings(filename)</c> overrides the saved file with this
    /// </summary>
    public static SettingsData LoadedSettings;

    /// <summary>
    /// Current on memory save
    /// 
    /// This holds everything that has been loaded from the file
    /// Or is going to be saved to the file.
    /// <c>LoadSave(filename)</c> overrides this
    /// <c>SaveSave(filename)</c> overrides the saved file with this
    /// </summary>
    public static SaveData LoadedSave;

    private static bool _initialized;
    private static string _path;
    private const string SETTINGS_EXTENSION = ".json";
    private const string SAVE_EXTENSION = ".sav";

    /// <summary>
    /// Other methods should do this automatically
    /// Initialize the manager, this should be called before anything else happening
    /// </summary>
    private static void Init()
    {
        _path = Application.persistentDataPath;
        LoadedSettings = MakeDefaultSettings();
        LoadedSave = MakeDefaultSave();
        _initialized = true;
    }

    /// <summary>
    /// Delete that file!
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="extension"></param>
    public static void DeleteFile(string filename, string extension)
    {
        if (!_initialized)
            Init();

        if (File.Exists(_path + Path.DirectorySeparatorChar + filename + extension))
            File.Delete(_path + Path.DirectorySeparatorChar + filename + extension);
    }

    #region SettingsMethods

    /// <summary>
    /// Load the settings from the saved file.
    /// Loaded settings can be found from this.LoadedSettings
    /// This function might change to just return the settings instead
    /// If file is not found, loads default settings.
    /// </summary>
    /// <param name="filename"></param>
    public static void LoadSettings(string filename)
    {
        if (!_initialized)
            Init();

        filename += SETTINGS_EXTENSION;

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
    /// <param name="filename"></param>
    public static void SaveSettings(string filename)
    {
        if (!_initialized)
            Init();

        filename += SETTINGS_EXTENSION;

        File.WriteAllText(_path + Path.DirectorySeparatorChar + filename, JsonConvert.SerializeObject(LoadedSettings, Formatting.Indented));
    }

    /// <summary>
    /// Delete the settings file and create a default one
    /// </summary>
    /// <param name="filename"></param>
    public static void DeleteAndMakeDefaultSettings(string filename)
    {
        if (!_initialized)
            Init();

        filename += SETTINGS_EXTENSION;

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


    /// <summary>
    /// Filenames DOES NOT contain extension or path!
    /// </summary>
    /// <returns>All filenames in default path</returns>
    public static string[] GetAllSettingsFilenames()
    {
        string[] filenames = Directory.GetFiles(_path, "*" + SETTINGS_EXTENSION);
        for (int i = 0; i < filenames.Length; i++)
        {
            filenames[i] = filenames[i].Remove(0, _path.Length + 1);
            filenames[i] = filenames[i].Remove(filenames[i].IndexOf('.'), SETTINGS_EXTENSION.Length);
        }
        return filenames;
    }

    #endregion

    #region SaveMethods
    /// <summary>
    /// Load the Save from the saved file.
    /// Loaded save stuff can be found from this.LoadedSave
    /// If file is not found, loads default save.
    /// </summary>
    /// <param name="filename"></param>
    public static void LoadSave(string filename)
    {
        if (!_initialized)
            Init();

        filename += SAVE_EXTENSION;

        if (File.Exists(_path + Path.DirectorySeparatorChar + filename))
        {
            string data = File.ReadAllText(_path + Path.DirectorySeparatorChar + filename);
            LoadedSave = JsonConvert.DeserializeObject<SaveData>(data);

            // ToDo: Check the data integrity
            Debug.Log("Save Loaded");
        }
        else
        {
            Debug.LogWarning(filename + " not found!\nUsing currently loaded Save");
        }
    }

    /// <summary>
    /// Saves the current <c>LoadedSave</c> to a file
    /// </summary>
    /// <param name="filename"></param>
    public static void SaveSave(string filename)
    {
        if (!_initialized)
            Init();

        filename += SAVE_EXTENSION;

        File.WriteAllText(_path + Path.DirectorySeparatorChar + filename, JsonConvert.SerializeObject(LoadedSave, Formatting.Indented));
    }

    /// <summary>
    /// Delete the settings file and create a default one
    /// </summary>
    /// <param name="filename"></param>
    public static void DeleteAndMakeDefaultSave(string filename)
    {
        if (!_initialized)
            Init();

        filename += SAVE_EXTENSION;

        if (File.Exists(_path + Path.DirectorySeparatorChar + filename))
        {
            LoadedSave = MakeDefaultSave();
            SaveSave(filename);
        }
    }

    /// <summary>
    /// Just makes a "dummy"
    /// </summary>
    /// <returns>"empty" save</returns>
    private static SaveData MakeDefaultSave()
    {
        SaveData data = new SaveData();
        return data;
    }

    /// <summary>
    /// Filenames DOES NOT contain extension or path!
    /// </summary>
    /// <returns>All filenames in default path</returns>
    public static string[] GetAllSaveFilenames()
    {
        string[] filenames = Directory.GetFiles(_path, "*" + SAVE_EXTENSION);
        for (int i = 0; i < filenames.Length; i++)
        {
            filenames[i] = filenames[i].Remove(0, _path.Length + 1);
            filenames[i] = filenames[i].Remove(filenames[i].IndexOf('.'), SAVE_EXTENSION.Length);
        }
        return filenames;
    }

    #endregion

    #region DebugStuff

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Open Path", priority = 0)]
    public static void DEBUGOpenFolder()
    {
        if (UnityEditor.EditorApplication.isPlaying && !_initialized)
            Init();

        if (_initialized)
            System.Diagnostics.Process.Start(@_path);
    }

    #region Settings

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Settings/Log Path", priority = 1)]
    public static void LogPath()
    {
        if (UnityEditor.EditorApplication.isPlaying && !_initialized)
            Init();
        if (_initialized)
            Debug.Log(_path);
    }

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Settings/Load Settings", priority = 2)]
    public static void DEBUGLoadSettings()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            LoadSettings("Settings");
        else
            Debug.LogError("Editor needs to be playing to do this!");
    }

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Settings/Save Settings", priority = 3)]
    public static void DEBUGSaveSettings()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            SaveSettings("Settings");
        else
            Debug.LogError("Editor needs to be playing to do this!");
    }

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Settings/Delete Settings", priority = 4)]
    public static void DEBUGDeleteSettings()
    {
        if (UnityEditor.EditorApplication.isPlaying && !_initialized)
            Init();
        if (_initialized)
            DeleteAndMakeDefaultSettings("Settings");
    }

    [UnityEditor.MenuItem("Debug", menuItem = "Tools/Serialization/Settings/Log all settings filenames", priority = 5)]
    public static void DEBUGLogAllFiles()
    {
        if (UnityEditor.EditorApplication.isPlaying && !_initialized)
            Init();

        if (_initialized)
        {
            foreach (string filename in GetAllSettingsFilenames())
            {
                Debug.Log(filename);
            }
        }
    }
    #endregion

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

    /// <summary>
    /// Holds the data for individual saves.
    /// </summary>
    public class SaveData
    {
        public int SaveVersion = 1;
        public string ProfileName = "";
        public int LastLevelCleared = 0;
        public float Money = 0;

        public List<LevelData> LevelDatas = new List<LevelData>();
    }

    /// <summary>
    /// Level data, Work In Progres
    /// ToDo: add relevant stuff
    /// </summary>
    public struct LevelData
    {
        public int CollectedTips;
        public int PlayerCount;
        public float TimeUsed;
    }
    #endregion
}

namespace ATEMkey.Configs
{
    using ATEMkey.CommandStructs;
    using RtMidi.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public class AppConfig
    {
        public const string cATEMip = "ATEMip";
        public const string cHyperDeckIp = "HyperDeckIp";

        public Dictionary<int, CommandOptions> Commands = new Dictionary<int, CommandOptions>();

        public string ATEMip
        {
            get
            {
                return _ATEMip;
            }
            set
            {
                _ATEMip = value;
                AddUpdateAppSettings(cATEMip, value);
            }
        }
        public string HyperDeckIp
        {
            get
            {
                return _HyperDeckIp;
            }
            set
            {
                _HyperDeckIp = value;
                AddUpdateAppSettings(cHyperDeckIp, value);
            }
        }

        public MapNoteList ATEMNoteMap { get; } = new MapNoteList();
        public MapControlList ATEMControlMap { get; } = new MapControlList();

        private string _ATEMip = "";
        private string _HyperDeckIp = "";

        public AppConfig()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _ATEMip = appSettings[cATEMip];
            _HyperDeckIp = appSettings[cHyperDeckIp];
            var midiInput = ConfigurationManager.GetSection(MidiInputSectionNote.SectionName) as MidiInputSectionNote;
            foreach(MapATEMMidi mapATEM in midiInput.MidiInputEndpoints)
            {
                ATEMNoteMap.Add(mapATEM.KeyValue, mapATEM);
            }
            var midiInputControl = ConfigurationManager.GetSection(MidiInputSectionControl.SectionName) as MidiInputSectionControl;
            foreach (MapATEMMidi mapATEM in midiInputControl.MidiInputEndpoints)
            {
                ATEMControlMap.Add(mapATEM.Value, mapATEM);
            }
        }

        private void AddUpdateAppSettings(string key, string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}

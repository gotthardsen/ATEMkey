namespace ATEMkey.Configs
{
    using ATEMkey.CommandStructs;
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

        private string _ATEMip = "";
        private string _HyperDeckIp = "";

        public AppConfig()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _ATEMip = appSettings[cATEMip];
            _HyperDeckIp = appSettings[cHyperDeckIp];
            var midiInput = ConfigurationManager.GetSection("MidiInput.ProgramInput") as MidiInputLoader;
            //todo            
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

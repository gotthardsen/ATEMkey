namespace ATEMkey.Configs
{
    using System.Configuration;

    public class MidiInputLoader : ConfigurationSection
    {
        [ConfigurationProperty("MidiInput")]
        public MidiInputCollection MidiInput
        {
            get { return ((MidiInputCollection)(base["MidiInput"])); }
        }
    }
}

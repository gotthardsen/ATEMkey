namespace ATEMkey.Configs
{
    using System.Configuration;
    using ATEMkey.CommandStructs;
    using Sanford.Multimedia.Midi;

    public class MapATEMMidi : ConfigurationElement
    {
        [ConfigurationProperty("Command", DefaultValue = CommandOptions.Bank.ProgramInput, IsRequired = false)]
        public CommandOptions.Bank Command
        {
            get
            {
                return (CommandOptions.Bank)this["Command"];
            }
            set
            {
                this["Command"] = value;
            }
        }

        [ConfigurationProperty("Port", DefaultValue = CommandOptions.ATEMPort.Cam1, IsRequired = false)]
        public CommandOptions.ATEMPort Port
        {
            get
            {
                return (CommandOptions.ATEMPort)this["Port"];
            }
            set
            {
                this["Port"] = value;
            }
        }

        [ConfigurationProperty("MidiCommand", DefaultValue = ChannelCommand.NoteOn, IsRequired = false)]
        public ChannelCommand MidiCommand
        {
            get
            {
                return (ChannelCommand)this["MidiCommand"];
            }
            set
            {
                this["MidiCommand"] = value;
            }
        }

        [ConfigurationProperty("KeyValue", DefaultValue = -1, IsRequired = false)]
        public int KeyValue
        {
            get
            {
                return (int)this["KeyValue"];
            }
            set
            {
                this["KeyValue"] = value;
            }
        }
    }
}

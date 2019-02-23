namespace ATEMkey.Configs
{
    using System.Configuration;
    using ATEMkey.CommandStructs;
    using RtMidi.Core.Enums;

    public class MapATEMMidi : ConfigurationElement
    {
        [ConfigurationProperty("command", DefaultValue = CommandOptions.Bank.ProgramInput, IsRequired = false)]
        public CommandOptions.Bank Command
        {
            get
            {
                return (CommandOptions.Bank)this["command"];
            }
            set
            {
                this["command"] = value;
            }
        }

        [ConfigurationProperty("port", DefaultValue = CommandOptions.ATEMPort.Cam1, IsRequired = false)]
        public CommandOptions.ATEMPort Port
        {
            get
            {
                return (CommandOptions.ATEMPort)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }

        [ConfigurationProperty("midiCommand", DefaultValue = CommandOptions.ChannelCommand.NoteOn, IsRequired = false)]
        public CommandOptions.ChannelCommand MidiCommand
        {
            get
            {
                return (CommandOptions.ChannelCommand)this["midiCommand"];
            }
            set
            {
                this["midiCommand"] = value;
            }
        }

        [ConfigurationProperty("keyValue", DefaultValue = Key.Key39, IsRequired = false)]
        public Key KeyValue
        {
            get
            {
                return (Key)this["keyValue"];
            }
            set
            {
                this["keyValue"] = value;
            }
        }

        [ConfigurationProperty("value", DefaultValue = -1, IsRequired = false)]
        public int Value
        {
            get
            {
                return (int)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }
    }
}

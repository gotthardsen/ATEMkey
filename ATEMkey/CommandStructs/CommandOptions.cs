namespace ATEMkey.CommandStructs
{
    public class CommandOptions
    {
        public enum Bank { ProgramInput, PreviewInput, Cut, AutoTrans, Record, Stop, LiveAuto };
        public enum ATEMPort { Cam1 = 1, Cam2 = 2, Cam3 = 3, Cam4 = 4, Cam5 = 5, Cam6 = 6, Cam7 = 7, Cam8 = 8, Mp1 = 3010, Mp2 = 3020 };
        public enum ChannelCommand { Control, NoteOn, SysEx };

        public Bank Command { get; set; }
        public ATEMPort Port { get; set; }
        public ChannelCommand MidiCommand { get; set; }
        public int KeyValue { get; set; }

        public CommandOptions(Bank bank, ATEMPort port, ChannelCommand midiCommand, int keyValue)
        {
            this.Command = bank;
            this.Port = port;
            this.MidiCommand = midiCommand;
            this.KeyValue = keyValue;
        }
    }
}

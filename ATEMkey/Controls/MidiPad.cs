namespace ATEMkey.Controls
{
    using System;
    using Sanford.Multimedia.Midi;
    using System.Threading;
    using ATEMkey.Exceptions;
    using ATEMkey.CommandStructs;
    using ATEMkey.Configs;

    public class MidiPad
    {
        private InputDevice inDevice = null;
        private bool goingDown = false;
        private int liveAuto = 0;

        private SynchronizationContext context;

        private ICommand<int[]> programInput;
        private ICommand<int> previewInput;
        private ICommand<bool> autoTrans;
        private ICommand<bool> record;
        private ICommand<double> fader;

        public MidiPad(AppConfig config, ICommand<int[]> programInput, ICommand<int>previewInput, ICommand<bool> autoTrans, ICommand<bool> record, ICommand<double> fader)
        {
            this.programInput = programInput;
            this.previewInput = previewInput;
            this.autoTrans = autoTrans;
            this.record = record;
            this.fader = fader;
            SetupKeys(config);
        }

        private void SetupKeys(AppConfig config)
        {
            throw new NotImplementedException();
        }

        public bool ConnectMidi()
        {
            if (InputDevice.DeviceCount == 0)
            {
                throw new MidiDeviceNotFound("No MIDI input devices available.");
            }
            else
            {
                try
                {
                    context = SynchronizationContext.Current;

                    inDevice = new InputDevice(0);
                    inDevice.ChannelMessageReceived += HandleChannelMessageReceived;
                    //inDevice.SysCommonMessageReceived += HandleSysCommonMessageReceived;
                    inDevice.SysExMessageReceived += HandleSysExMessageReceived;
                    //inDevice.SysRealtimeMessageReceived += HandleSysRealtimeMessageReceived;
                    //inDevice.Error += new EventHandler<ErrorEventArgs>(inDevice_Error);
                    inDevice.StartRecording();
                }
                catch (Exception ex)
                {
                    throw new MidiDeviceSetupException("Problem setting up Midi Device: " +ex.Message);
                    //Close();
                }
            }
            return true;
        }

        public void Close()
        {
            if (inDevice != null)
            {
                inDevice.Close();
            }
        }

        private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            context.Post(delegate (object dummy)
            {
                if (e.Message.Command == ChannelCommand.NoteOn && e.Message.Data2 > 0)
                {
                    //Program
                    if (e.Message.Data1 == 39)
                       programInput.Execute(new int[]{ 4, liveAuto});
                    else if (e.Message.Data1 == 48)
                        programInput.Execute(new int[] { 5, liveAuto });
                    else if (e.Message.Data1 == 45)
                        programInput.Execute(new int[] { 6, liveAuto });
                    else if (e.Message.Data1 == 43)
                        programInput.Execute(new int[] { 7, liveAuto });
                    else if (e.Message.Data1 == 51)
                        programInput.Execute(new int[] { 3010, liveAuto });
                    else if (e.Message.Data1 == 49)
                        programInput.Execute(new int[] { 3020, liveAuto });
                    //Preview
                    else if (e.Message.Data1 == 36)
                        previewInput.Execute(4);
                    else if (e.Message.Data1 == 38)
                        previewInput.Execute(5);
                    else if (e.Message.Data1 == 40)
                        previewInput.Execute(6);
                    else if (e.Message.Data1 == 42)
                        previewInput.Execute(7);
                    else if (e.Message.Data1 == 44)
                        previewInput.Execute(3010);
                    else if (e.Message.Data1 == 46)
                        previewInput.Execute(3020);
                    //Cut - Auto
                    else if (e.Message.Data1 == 1)
                        autoTrans.Execute(false);
                    else if (e.Message.Data1 == 2)
                        autoTrans.Execute(true);
                }
                else if (e.Message.Command == ChannelCommand.Controller && e.Message.Data2 > 0)
                {
                    if (e.Message.Data1 == 44)
                        record.Execute(true);
                    else if (e.Message.Data1 == 46)
                        record.Execute(false);
                    else if (e.Message.Data1 == 49)
                    {
                        if (liveAuto == 0)
                            liveAuto = 1;
                        else
                            liveAuto = 0;
                    }
                }
            }, null);
        }

        private void HandleSysExMessageReceived(object sender, SysExMessageEventArgs e)
        {
            context.Post(delegate (object dummy)
            {
                double pos = e.Message[6] / 127.0;
                if (goingDown)
                    pos = (127 - e.Message[6]) / 127.0;
                fader.Execute(pos);
                if (e.Message[6] == 127 || e.Message[6] == 0)
                    goingDown = !goingDown;
            }, null);
        }
    }
}

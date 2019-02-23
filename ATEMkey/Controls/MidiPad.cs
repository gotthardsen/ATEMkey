namespace ATEMkey.Controls
{
    using System;
    using ATEMkey.Exceptions;
    using ATEMkey.CommandStructs;
    using ATEMkey.Configs;
    using RtMidi.Core;
    using System.Linq;
    using RtMidi.Core.Devices.Infos;
    using RtMidi.Core.Devices;

    public class MidiPad
    {
        private IMidiInputDevice inDevice = null;
        private bool goingDown = false;

        private ICommand<MapATEMMidi> programInput;
        private ICommand<MapATEMMidi> previewInput;
        private ICommand<bool> autoTrans;
        private ICommand<bool> record;
        private ICommand<double> fader;
        private MapNoteList mapNote;
        private MapControlList mapControl;

        public MidiPad(MapNoteList mapNote, MapControlList mapControl, ICommand<MapATEMMidi> programInput, ICommand<MapATEMMidi> previewInput, ICommand<bool> autoTrans, ICommand<bool> record, ICommand<double> fader)
        {
            this.programInput = programInput;
            this.previewInput = previewInput;
            this.autoTrans = autoTrans;
            this.record = record;
            this.fader = fader;
            this.mapNote = mapNote;
            this.mapControl = mapControl;
        }

        public bool ConnectMidi()
        {
            if (MidiDeviceManager.Default.InputDevices.Count() == 0)
            {
                throw new MidiDeviceNotFound("No MIDI input devices available.");
            }
            else
            {
                try
                {
                    IMidiInputDeviceInfo firstDevInfo = MidiDeviceManager.Default.InputDevices.First();

                    inDevice = firstDevInfo.CreateDevice();
                    inDevice.ControlChange += HandleControlChange;
                    inDevice.NoteOn += HandleNoteOn;
                    inDevice.SysEx += HandleSysEx;
                    inDevice.Open();
                }
                catch (Exception ex)
                {
                    throw new MidiDeviceSetupException("Problem setting up Midi Device: " +ex.Message);
                    //Close();
                }
            }
            return true;
        }

        private void HandleSysEx(IMidiInputDevice sender, in RtMidi.Core.Messages.SysExMessage msg)
        {
            //fader
            double pos = msg.Data[5] / 127.0;
            if (goingDown)
                pos = (127 - msg.Data[5]) / 127.0;
            fader.Execute(pos);
            if (msg.Data[5] == 127 || msg.Data[5] == 0)
                goingDown = !goingDown;
        }

        private void HandleNoteOn(IMidiInputDevice sender, in RtMidi.Core.Messages.NoteOnMessage msg)
        {
            if(msg.Velocity > 0)
            {
                MapATEMMidi map;
                if (mapNote.TryGetValue(msg.Key, out map))
                {
                    if(map.Command == CommandOptions.Bank.ProgramInput)
                        programInput.Execute(map);
                    else if (map.Command == CommandOptions.Bank.PreviewInput)
                        previewInput.Execute(map);
                    else if (map.Command == CommandOptions.Bank.Cut)
                        autoTrans.Execute(false);
                    else if (map.Command == CommandOptions.Bank.AutoTrans)
                        autoTrans.Execute(true);
                }
            }
        }

        private void HandleControlChange(IMidiInputDevice sender, in RtMidi.Core.Messages.ControlChangeMessage msg)
        {
            if (msg.Value > 0)
            {
                MapATEMMidi map;
                if (mapControl.TryGetValue(msg.Control, out map))
                {
                    if (map.Command == CommandOptions.Bank.Record)
                        record.Execute(true);
                    else if (map.Command == CommandOptions.Bank.Stop)
                        record.Execute(false);
                    else if (map.Command == CommandOptions.Bank.LiveAuto)
                    {
                        if (programInput.Toggle == 0)
                            programInput.Toggle = 1;
                            else
                            programInput.Toggle = 0;
                    }
                }
            }
        }

        public void Close()
        {
            if (inDevice != null)
            {
                inDevice.ControlChange -= HandleControlChange;
                inDevice.NoteOn -= HandleNoteOn;
                inDevice.SysEx -= HandleSysEx;
                inDevice.Close();
                inDevice.Dispose();
            }
        }
    }
}

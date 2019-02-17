namespace ATEMkey.Exceptions
{
    using System;

    public class MidiDeviceNotFound : Exception
    {
        public MidiDeviceNotFound(string message) : base(message)
        {

        }
    }
}

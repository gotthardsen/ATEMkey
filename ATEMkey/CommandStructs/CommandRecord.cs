using ATEMkey.Controls;

namespace ATEMkey.CommandStructs
{

    public class CommandRecord : ICommand<bool>
    {
        protected IATEMControl control;

        public CommandRecord(IATEMControl control)
        {
            this.control = control;
        }

        public int Toggle { get; set; }

        public void Execute(bool args)
        {
            if (args)
                control.Record();
            else
                control.Stop();
        }
    }
}

namespace ATEMkey.CommandStructs
{
    using ATEMkey.Configs;
    using ATEMkey.Controls;

    public class CommandProgramInput : ICommand<MapATEMMidi>
    {
        protected IATEMControl control;
        private int _LiveAuto = 0;

        public CommandProgramInput(IATEMControl control)
        {
            this.control = control;
        }

        public int Toggle { get { return _LiveAuto; } set { _LiveAuto = value; } }

        public void Execute(MapATEMMidi args)
        {
            if(_LiveAuto == 1)
            {
                control.SetPreviewInput((int)args.Port);
                control.PerformAutoTransition();
            }
            else
                control.SetProgramInput((int)args.Port);
        }
    }
}

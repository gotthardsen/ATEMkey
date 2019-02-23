namespace ATEMkey.CommandStructs
{
    using ATEMkey.Configs;
    using ATEMkey.Controls;

    public class CommandPreviewInput : ICommand<MapATEMMidi>
    {
        protected IATEMControl control;

        public CommandPreviewInput(IATEMControl control)
        {
            this.control = control;
        }

        public int Toggle { get; set; }

        public void Execute(MapATEMMidi args)
        {
            control.SetPreviewInput((int)args.Port);
        }
    }
}

namespace ATEMkey.CommandStructs
{
    using ATEMkey.Controls;

    public class CommandFader : ICommand<double>
    {
        protected IATEMControl control;

        public CommandFader(IATEMControl control)
        {
            this.control = control;
        }

        public int Toggle { get; set; }

        public void Execute(double args)
        {
            control.SetTransitionPosition(args);
        }
    }
}

namespace ATEMkey.CommandStructs
{
    using ATEMkey.Controls;

    public class CommandAutoTransition : ICommand<bool>
    {
        protected IATEMControl control;

        public CommandAutoTransition(IATEMControl control)
        {
            this.control = control;
        }
        public int Toggle { get; set; }

        public void Execute(bool args)
        {
            if (args)
                control.PerformAutoTransition();
            else
                control.PerformCut();
        }
    }
}

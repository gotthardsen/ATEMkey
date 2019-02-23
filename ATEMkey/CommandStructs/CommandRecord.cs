namespace ATEMkey.CommandStructs
{
    using ATEMkey.CommandStructs;
    using BMDSwitcherAPI;

    public class CommandRecord : ICommand<bool>
    {
        protected IBMDSwitcherHyperDeck switcherHyperdeck;

        public CommandRecord(IBMDSwitcherHyperDeck switcherHyperdeck)
        {
            this.switcherHyperdeck = switcherHyperdeck;
        }

        public int Toggle { get; set; }

        public void Execute(bool args)
        {
            if (args)
                switcherHyperdeck.Record();
            else
                switcherHyperdeck.Stop();
        }
    }
}

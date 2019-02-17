namespace ATEMkey.CommandStructs
{
    using ATEMkey.CommandStructs;
    using BMDSwitcherAPI;

    public class CommandAutoTransition : ICommand<bool>
    {
        protected IBMDSwitcherMixEffectBlock mixEffectBlock;

        public CommandAutoTransition(IBMDSwitcherMixEffectBlock mixEffectBlock)
        {
            this.mixEffectBlock = mixEffectBlock;
        }

        public void Execute(bool args)
        {
            if (args)
                mixEffectBlock.PerformAutoTransition();
            else
                mixEffectBlock.PerformCut();
        }
    }
}

namespace ATEMkey.CommandStructs
{
    using ATEMkey.CommandStructs;
    using BMDSwitcherAPI;

    public class CommandFader : ICommand<double>
    {
        protected IBMDSwitcherMixEffectBlock mixEffectBlock;

        public CommandFader(IBMDSwitcherMixEffectBlock mixEffectBlock)
        {
            this.mixEffectBlock = mixEffectBlock;
        }

        public void Execute(double args)
        {
            mixEffectBlock.SetTransitionPosition(args);
        }
    }
}

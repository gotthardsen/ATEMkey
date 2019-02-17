namespace ATEMkey.CommandStructs
{
    using ATEMkey.CommandStructs;
    using BMDSwitcherAPI;

    public class CommandPreviewInput : ICommand<int>
    {
        protected IBMDSwitcherMixEffectBlock mixEffectBlock;

        public CommandPreviewInput(IBMDSwitcherMixEffectBlock mixEffectBlock)
        {
            this.mixEffectBlock = mixEffectBlock;
        }

        public void Execute(int args)
        {
            mixEffectBlock.SetPreviewInput(args);
        }
    }
}

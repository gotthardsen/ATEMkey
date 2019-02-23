namespace ATEMkey.CommandStructs
{
    using ATEMkey.CommandStructs;
    using ATEMkey.Configs;
    using BMDSwitcherAPI;

    public class CommandPreviewInput : ICommand<MapATEMMidi>
    {
        protected IBMDSwitcherMixEffectBlock mixEffectBlock;

        public CommandPreviewInput(IBMDSwitcherMixEffectBlock mixEffectBlock)
        {
            this.mixEffectBlock = mixEffectBlock;
        }

        public int Toggle { get; set; }

        public void Execute(MapATEMMidi args)
        {
            mixEffectBlock.SetPreviewInput((int)args.Port);
        }
    }
}

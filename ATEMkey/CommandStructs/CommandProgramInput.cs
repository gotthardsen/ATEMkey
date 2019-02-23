namespace ATEMkey.CommandStructs
{
    using ATEMkey.CommandStructs;
    using ATEMkey.Configs;
    using BMDSwitcherAPI;
    using System;

    public class CommandProgramInput : ICommand<MapATEMMidi>
    {
        protected IBMDSwitcherMixEffectBlock mixEffectBlock;
        private int _LiveAuto = 0;

        public CommandProgramInput(IBMDSwitcherMixEffectBlock mixEffectBlock)
        {
            this.mixEffectBlock = mixEffectBlock;
        }

        public int Toggle { get { return _LiveAuto; } set { _LiveAuto = value; } }

        public void Execute(MapATEMMidi args)
        {
            if(_LiveAuto == 1)
            {
                mixEffectBlock.SetPreviewInput((int)args.Port);
                mixEffectBlock.PerformAutoTransition();
            }
            else
                mixEffectBlock.SetProgramInput((int)args.Port);
        }
    }
}

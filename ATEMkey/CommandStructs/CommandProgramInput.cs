namespace ATEMkey.CommandStructs
{
    using ATEMkey.CommandStructs;
    using BMDSwitcherAPI;
    using System;

    public class CommandProgramInput : ICommand<int[]>
    {
        protected IBMDSwitcherMixEffectBlock mixEffectBlock;

        public CommandProgramInput(IBMDSwitcherMixEffectBlock mixEffectBlock)
        {
            this.mixEffectBlock = mixEffectBlock;
        }

        public void Execute(int[] args)
        {
            if(args[1] == 1)
            {
                mixEffectBlock.SetPreviewInput(args[0]);
                mixEffectBlock.PerformAutoTransition();
            }
            else
                mixEffectBlock.SetProgramInput(args[0]);
        }
    }
}

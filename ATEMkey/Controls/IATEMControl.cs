namespace ATEMkey.Controls
{
    using ATEMkey.CommandStructs;
    using BMDSwitcherAPI;

    public interface IATEMControl
    {
        void SetProgramInput(int port);
        void SetPreviewInput(int port);
        void PerformAutoTransition();
        void PerformCut();
        void Record();
        void Stop();
        void SetTransitionPosition(double pos);
        IBMDSwitcherMediaPool MediaPool();
        uint VideoHeight();
        uint VideoWidth();
        void ChangeMediaPlayer(CommandOptions.ATEMPort port, uint slot);
    }
}

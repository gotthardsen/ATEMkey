namespace ATEMkey.Callbacks
{
    using BMDSwitcherAPI;

    public class StillsCallback : IBMDSwitcherStillsCallback
    {
        private LockCallbackBase lockObject;

        public StillsCallback(LockCallbackBase lockObject)
        {
            this.lockObject = lockObject;
        }

        public void Notify(_BMDSwitcherMediaPoolEventType eventType, IBMDSwitcherFrame frame, int index)
        {
            _BMDSwitcherMediaPoolEventType mediaPoolEventType = eventType;

            if (mediaPoolEventType != _BMDSwitcherMediaPoolEventType.bmdSwitcherMediaPoolEventTypeTransferCompleted)
            {
                return;
            }
            this.lockObject.TransferCompleted();
        }
    }
}

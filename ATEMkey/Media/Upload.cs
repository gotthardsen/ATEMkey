namespace ATEMkey.Media
{
    using ATEMkey.Callbacks;
    using BMDSwitcherAPI;

    public class Upload : LockCallbackBase
    {
        private IBMDSwitcherStills stills;
        private MediaPool mediaPool;
        private IBMDSwitcherLockCallback lockCallback;

        public Upload(MediaPool mediaPool)
        {
            this.mediaPool = mediaPool;
            stills = mediaPool.GetStills();
        }

        public bool Execute(string fileName, uint index)
        {
            return true;
        }

        public override void LockCallback()
        {
            IBMDSwitcherStillsCallback callback = (IBMDSwitcherStillsCallback)new StillsCallback(this);
            this.stills.AddCallback(callback);
            //this.stills.Upload((uint)this.uploadSlot, this.GetName(), this.frame);
        }

        public override void TransferCompleted()
        {
            this.stills.Unlock(this.lockCallback);
            //this.currentStatus = Upload.Status.Completed;
        }
    }
}

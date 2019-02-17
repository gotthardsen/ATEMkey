namespace ATEMkey.Callbacks
{
    using BMDSwitcherAPI;

    class LockCallback : IBMDSwitcherLockCallback
    {
        private LockCallbackBase lockObject;

        public LockCallback(LockCallbackBase lockObject)
        {
            this.lockObject = lockObject;
        }

        public void Obtained()
        {
            lockObject.LockCallback();
        }
    }
}

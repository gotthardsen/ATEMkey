namespace ATEMkey.Media
{
    using ATEMkey.Controls;
    using BMDSwitcherAPI;
    using System.Collections.Generic;

    public class MediaPool
    {
        private readonly IATEMControl control;
        private IBMDSwitcherMediaPool switcherMediaPool;
        private IBMDSwitcherLockCallback lockCallback;

        public IList<Still> list = new List<Still>();

        public MediaPool(IATEMControl control)
        {
            this.control = control;
            switcherMediaPool = control.MediaPool();

            UpdateStills();
        }

        public void UpdateStills()
        {
            IBMDSwitcherStills stills;
            switcherMediaPool.GetStills(out stills);

            uint count;
            stills.GetCount(out count);
            for (uint index = 0; index < count; index++)
            {
                Still still = new Still(stills, index);
                list.Add(still);
            }
        }

        public void Download(uint index)
        {


            
        }

        public IBMDSwitcherStills GetStills()
        {
            IBMDSwitcherStills stills;
            switcherMediaPool.GetStills(out stills);
            return stills;
        }
    }
}

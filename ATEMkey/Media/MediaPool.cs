namespace ATEMkey.Media
{
    using ATEMkey.Controls;
    using BMDSwitcherAPI;
    using System.Collections.Generic;
    using System.Threading;

    public class MediaPool
    {
        private readonly IATEMControl control;
        public IBMDSwitcherMediaPool switcherMediaPool;

        public IList<Still> list = new List<Still>();
        private Upload upl;

        public MediaPool(IATEMControl control)
        {
            this.control = control;
            switcherMediaPool = control.MediaPool();
            upl = new Upload(this);
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

        public void Upload(string filename, uint index)
        {
            upl.Execute(filename, index);
        }

        public IBMDSwitcherStills GetStills()
        {
            IBMDSwitcherStills stills;
            switcherMediaPool.GetStills(out stills);
            return stills;
        }

        public uint VideoHeight()
        {
            return control.VideoHeight();
        }
        public uint VideoWidth()
        {
            return control.VideoWidth();
        }
    }
}

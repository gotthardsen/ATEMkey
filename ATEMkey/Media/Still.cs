namespace ATEMkey.Media
{
    using BMDSwitcherAPI;
    using System;

    public class Still
    {
        public readonly string hash;
        public readonly string name;
        public readonly int index;

        public Still(IBMDSwitcherStills still, uint index)
        {
            BMDSwitcherHash hash;
            still.GetHash(index, out hash);
            this.hash = String.Join("", BitConverter.ToString(hash.data).Split('-'));
            still.GetName(index, out this.name);
            this.index = (int)index + 1;
        }
    }
}

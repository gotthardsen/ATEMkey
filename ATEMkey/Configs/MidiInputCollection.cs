namespace ATEMkey.Configs
{
    using System.Configuration;

    [ConfigurationCollection(typeof(MapATEMMidi))]
    public class MidiInputCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MapATEMMidi();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MapATEMMidi)(element)).KeyValue;
        }

        public MapATEMMidi this[int idx]
        {
            get { return (MapATEMMidi)BaseGet(idx); }
        }
    }
}

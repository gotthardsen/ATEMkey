namespace ATEMkey.Configs
{
    using System.Configuration;

    public class NoteInputEndpointsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MapATEMMidi();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MapATEMMidi)element).KeyValue;
        }
    }
}
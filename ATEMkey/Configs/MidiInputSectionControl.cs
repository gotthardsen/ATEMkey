namespace ATEMkey.Configs
{
    using System.Configuration;

    public class MidiInputSectionControl : ConfigurationSection
    {
        /// <summary>
        /// The name of this section in the app.config.
        /// </summary>
        public const string SectionName = "MidiInputSectionControl";

        private const string EndpointCollectionName = "ControlInputMap";

        [ConfigurationProperty(EndpointCollectionName)]
        [ConfigurationCollection(typeof(ControlInputEndpointsCollection), AddItemName = "add")]
        public ControlInputEndpointsCollection MidiInputEndpoints
        {
            get
            {
                return (ControlInputEndpointsCollection)base[EndpointCollectionName];
            }
        }
    }
}

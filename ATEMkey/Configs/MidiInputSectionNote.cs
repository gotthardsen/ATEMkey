namespace ATEMkey.Configs
{
    using System.Configuration;

    public class MidiInputSectionNote : ConfigurationSection
    {
        /// <summary>
        /// The name of this section in the app.config.
        /// </summary>
        public const string SectionName = "MidiInputSectionNote";

        private const string EndpointCollectionName = "NoteInputMap";

        [ConfigurationProperty(EndpointCollectionName)]
        [ConfigurationCollection(typeof(NoteInputEndpointsCollection), AddItemName = "add")]
        public NoteInputEndpointsCollection MidiInputEndpoints
        {
            get
            {
                return (NoteInputEndpointsCollection)base[EndpointCollectionName];
            }
        }
    }
}

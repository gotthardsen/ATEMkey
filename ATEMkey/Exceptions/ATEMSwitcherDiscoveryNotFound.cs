namespace ATEMkey.Exceptions
{
    using System;

    /// <summary>
    /// Raised if CBMDSwitcherDiscovery cannot be created
    /// </summary>
    [Serializable]
    public class ATEMSwitcherDiscoveryNotFound : Exception
    {
        /// <summary>
        /// Raised if CBMDSwitcherDiscovery cannot be created
        /// </summary>
        /// <param name="message">Display message</param>
        public ATEMSwitcherDiscoveryNotFound(string message) : base(message)
        {
        }
    }
}

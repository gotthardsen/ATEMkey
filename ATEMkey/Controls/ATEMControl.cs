namespace ATEMkey.Controls
{
    using ATEMkey.CommandStructs;
    using ATEMkey.Exceptions;
    using ATEMkey.Monitors;
    using BMDSwitcherAPI;
    using System;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class ATEMControl : IATEMControl
    {
        private const int SysExBufferSize = 128;

        private IBMDSwitcherMixEffectBlock m_mixEffectBlock1 = null;
        private IBMDSwitcherHyperDeck switcherHyperdeck = null;
        public IBMDSwitcher m_switcher;

        private IBMDSwitcherDiscovery m_switcherDiscovery;
        private SwitcherMonitor m_switcherMonitor;
        private SynchronizationContext context;
        private Form mainForm;

        public ATEMControl(Form mainForm)
        {
            context = SynchronizationContext.Current;

            this.mainForm = mainForm;
        }

        public bool ConnectATEM(string atemIP)
        {
            m_switcherDiscovery = new CBMDSwitcherDiscovery();
            if (m_switcherDiscovery == null)
                throw new ATEMSwitcherDiscoveryNotFound("Could not create Switcher Discovery Instance.\nATEM Switcher Software may not be installed.");

            _BMDSwitcherConnectToFailure failReason = 0;

            try
            {
                // Note that ConnectTo() can take several seconds to return, both for success or failure,
                // depending upon hostname resolution and network response times, so it may be best to
                // do this in a separate thread to prevent the main GUI thread blocking.
                m_switcherDiscovery.ConnectTo(atemIP, out m_switcher, out failReason);
            }
            catch (COMException)
            {
                // An exception will be thrown if ConnectTo fails. For more information, see failReason.
                switch (failReason)
                {
                    case _BMDSwitcherConnectToFailure.bmdSwitcherConnectToFailureNoResponse:
                        throw new ATEMSwitcherConnectToFailure("No response from Switcher");
                    case _BMDSwitcherConnectToFailure.bmdSwitcherConnectToFailureIncompatibleFirmware:
                        throw new ATEMSwitcherConnectToFailure("Switcher has incompatible firmware");
                    default:
                        throw new ATEMSwitcherConnectToFailure("Connection failed for unknown reason");
                }
            }

            m_switcherMonitor = new SwitcherMonitor();
            // note: this invoke pattern ensures our callback is called in the main thread. We are making double
            // use of lambda expressions here to achieve this.
            // Essentially, the events will arrive at the callback class (implemented by our monitor classes)
            // on a separate thread. We must marshal these to the main thread, and we're doing this by calling
            // invoke on the Windows Forms object. The lambda expression is just a simplification.
            m_switcherMonitor.SwitcherDisconnected += new SwitcherEventHandler((s, a) => mainForm.Invoke((Action)(() => SwitcherDisconnected())));

            return SetMixBlock();
        }

        public bool ConnectHyperDeck(string hyperDeckIP)
        {
            IntPtr hyperdeckIteratorPtr;
            Guid hyperdeckIteratorIID = typeof(IBMDSwitcherHyperDeckIterator).GUID;
            this.m_switcher.CreateIterator(ref hyperdeckIteratorIID, out hyperdeckIteratorPtr);
            IBMDSwitcherHyperDeckIterator hyperdeckIterator = null;
            if (hyperdeckIteratorPtr == null)
                return false;
            hyperdeckIterator = (IBMDSwitcherHyperDeckIterator)Marshal.GetObjectForIUnknown(hyperdeckIteratorPtr);

            hyperdeckIterator.Next(out switcherHyperdeck);
            if (switcherHyperdeck == null)
                throw new HyperDeckNotFound("Unexpected: Could not get HyperDeck");
            else
            {
                string hdIp = hyperDeckIP;
                var ipAddress = IPAddress.Parse(hdIp);
                var ipBytes = ipAddress.GetAddressBytes();
                var ip = (uint)ipBytes[0] << 24;
                ip += (uint)ipBytes[1] << 16;
                ip += (uint)ipBytes[2] << 8;
                ip += (uint)ipBytes[3];
                switcherHyperdeck.SetNetworkAddress(ip);
            }

            return true;
        }

        private void SwitcherDisconnected()
        {
            if (m_mixEffectBlock1 != null)
            {
                // Release reference
                m_mixEffectBlock1 = null;
            }

            if (m_switcher != null)
            {
                // Remove callback:
                //m_switcher.RemoveCallback(m_switcherMonitor);

                // Release reference:
                m_switcher = null;
            }
        }

        public void Close()
        {
            SwitcherDisconnected();
        }

        private bool SetMixBlock()
        {
            // We want to get the first Mix Effect block (ME 1). We create a ME iterator,
            // and then get the first one:
            m_mixEffectBlock1 = null;

            IBMDSwitcherMixEffectBlockIterator meIterator = null;
            IntPtr meIteratorPtr;
            Guid meIteratorIID = typeof(IBMDSwitcherMixEffectBlockIterator).GUID;
            m_switcher.CreateIterator(ref meIteratorIID, out meIteratorPtr);
            if (meIteratorPtr != null)
                meIterator = (IBMDSwitcherMixEffectBlockIterator)Marshal.GetObjectForIUnknown(meIteratorPtr);

            if (meIterator == null)
                return false;

            if (meIterator == null)
                return false;
            meIterator.Next(out m_mixEffectBlock1);

            if (m_mixEffectBlock1 == null)
                throw new ATEMSwitcherMissingMixerBlock("Unexpected: Could not get first mix effect block");
            return true;
        }

        public IBMDSwitcherMediaPool MediaPool()
        {
            return (IBMDSwitcherMediaPool)this.m_switcher;
        }

        public void SetProgramInput(int port)
        {
            context.Post(delegate (object dummy)
            {
                m_mixEffectBlock1.SetProgramInput((long)port);
            }, null);        
        }

        public void SetPreviewInput(int port)
        {
            context.Post(delegate (object dummy)
            {
                m_mixEffectBlock1.SetPreviewInput(port);
            }, null);
        }

        public void PerformAutoTransition()
        {
            context.Post(delegate (object dummy)
            {
                m_mixEffectBlock1.PerformAutoTransition();
            }, null);
        }

        public void PerformCut()
        {
            context.Post(delegate (object dummy)
            {
                m_mixEffectBlock1.PerformCut();
            }, null);
        }

        public void Record()
        {
            context.Post(delegate (object dummy)
            {
                switcherHyperdeck.Record();
            }, null);
        }

        public void Stop()
        {
            context.Post(delegate (object dummy)
            {
                switcherHyperdeck.Stop();
            }, null);
        }

        public void SetTransitionPosition(double pos)
        {
            context.Post(delegate (object dummy)
            {
                m_mixEffectBlock1.SetTransitionPosition(pos);
            }, null);
        }
    }
}

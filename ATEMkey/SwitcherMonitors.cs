namespace ATEMkey.Monitors
{
    using BMDSwitcherAPI;

    public delegate void SwitcherEventHandler(object sender, object args);

    class SwitcherMonitor : IBMDSwitcherCallback
    {
        // Events:
        public event SwitcherEventHandler SwitcherDisconnected;

        public SwitcherMonitor()
        {
        }

        void IBMDSwitcherCallback.Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            if (eventType == _BMDSwitcherEventType.bmdSwitcherEventTypeDisconnected)
            {
                if (SwitcherDisconnected != null)
                    SwitcherDisconnected(this, null);
            }
        }
    }

    class MixEffectBlockMonitor : IBMDSwitcherMixEffectBlockCallback
    {
        // Events:
        public event SwitcherEventHandler ProgramInputChanged;
        public event SwitcherEventHandler PreviewInputChanged;
        public event SwitcherEventHandler TransitionFramesRemainingChanged;
        public event SwitcherEventHandler TransitionPositionChanged;
        public event SwitcherEventHandler InTransitionChanged;

        public MixEffectBlockMonitor()
        {
        }

        void IBMDSwitcherMixEffectBlockCallback.Notify(_BMDSwitcherMixEffectBlockEventType eventType)
        {
            switch (eventType)
            {
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeProgramInputChanged:
                    if (ProgramInputChanged != null)
                        ProgramInputChanged(this, null);
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypePreviewInputChanged:
                    if (PreviewInputChanged != null)
                        PreviewInputChanged(this, null);
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeTransitionFramesRemainingChanged:
                    if (TransitionFramesRemainingChanged != null)
                        TransitionFramesRemainingChanged(this, null);
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeTransitionPositionChanged:
                    if (TransitionPositionChanged != null)
                        TransitionPositionChanged(this, null);
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeInTransitionChanged:
                    if (InTransitionChanged != null)
                        InTransitionChanged(this, null);
                    break;
            }
        }

        public void Notify(_BMDSwitcherMixEffectBlockEventType eventType)
        {
            switch(eventType)
            {
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeProgramInputChanged:
                    if (ProgramInputChanged != null)
                        ProgramInputChanged(this, null);
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypePreviewInputChanged:
                    if (PreviewInputChanged != null)
                        PreviewInputChanged(this, null);
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeTransitionFramesRemainingChanged:
                    if (TransitionFramesRemainingChanged != null)
                        TransitionFramesRemainingChanged(this, null);
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeTransitionPositionChanged:
                    if (TransitionPositionChanged != null)
                        TransitionPositionChanged(this, null);
                    break;
                case _BMDSwitcherMixEffectBlockEventType.bmdSwitcherMixEffectBlockEventTypeInTransitionChanged:
                    if (InTransitionChanged != null)
                        InTransitionChanged(this, null);
                    break;
            }
        }
    }

    class InputMonitor : IBMDSwitcherInputCallback
    {
        // Events:
        public event SwitcherEventHandler LongNameChanged;

        private IBMDSwitcherInput m_input;
        public IBMDSwitcherInput Input { get { return m_input; } }

        public InputMonitor(IBMDSwitcherInput input)
        {
            m_input = input;
        }

        void IBMDSwitcherInputCallback.Notify(_BMDSwitcherInputEventType eventType)
        {
            switch (eventType)
            {
                case _BMDSwitcherInputEventType.bmdSwitcherInputEventTypeLongNameChanged:
                    if (LongNameChanged != null)
                        LongNameChanged(this, null);
                    break;
            }
        }
    }
}

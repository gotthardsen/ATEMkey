namespace ATEMkey
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using ATEMkey.CommandStructs;
    using ATEMkey.Configs;
    using ATEMkey.Controls;
    using ATEMkey.Media;

    public partial class Form1 : Form
    {
        private ATEMControl atemControl = null;
        private MidiPad midiPad = null;
        private MediaPool mediaPool = null;

        public Form1()
        {
            InitializeComponent();

            try
            {
                var config = new AppConfig();
                atemControl = new ATEMControl(this);

                atemControl.ConnectATEM(config.ATEMip);
                atemControl.ConnectHyperDeck(config.HyperDeckIp);

                midiPad = new MidiPad(config.ATEMNoteMap, config.ATEMControlMap,
                    new CommandProgramInput(atemControl.m_mixEffectBlock1),
                    new CommandPreviewInput(atemControl.m_mixEffectBlock1),
                    new CommandAutoTransition(atemControl.m_mixEffectBlock1),
                    new CommandRecord(atemControl.switcherHyperdeck),
                    new CommandFader(atemControl.m_mixEffectBlock1));

                midiPad.ConnectMidi();

                mediaPool = new MediaPool(atemControl.m_switcher);

                foreach (Still still in mediaPool.list)
                {
                    listBox1.Items.Add(still.name);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show($"Unable to start: {e.Message}");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (atemControl != null)
                atemControl.Close();
            if(midiPad != null)
                midiPad.Close();
        }
    }
}

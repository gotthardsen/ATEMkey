namespace ATEMkey
{
    using System;
    using System.Windows.Forms;
    using ATEMkey.CommandStructs;
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
                atemControl = new ATEMControl(this);

                atemControl.ConnectATEM("192.168.1.240");
                atemControl.ConnectHyperDeck("192.168.1.241");

                midiPad = new MidiPad(
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
                MessageBox.Show(e.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(atemControl != null)
                atemControl.Close();
            if(midiPad != null)
                midiPad.Close();
        }
    }
}

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
                    new CommandProgramInput(atemControl),
                    new CommandPreviewInput(atemControl),
                    new CommandAutoTransition(atemControl),
                    new CommandRecord(atemControl),
                    new CommandFader(atemControl));

                midiPad.ConnectMidi();

                mediaPool = new MediaPool(atemControl);

                UpdateImages();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Unable to start: {e.Message}");
            }
        }

        private void UpdateImages()
        {
            listBox1.Items.Clear();
            foreach (Still still in mediaPool.list)
            {
                if (!String.IsNullOrEmpty(still.name))
                    listBox1.Items.Add(still.name);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (atemControl != null)
                atemControl.Close();
            if(midiPad != null)
                midiPad.Close();
        }

        private void uploadBut_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mediaPool.Upload(openFileDialog1.FileName, (uint)listBox1.Items.Count);
                UpdateImages();
            }
        }

        private void updateBut_Click(object sender, EventArgs e)
        {
            UpdateImages();
        }
    }
}

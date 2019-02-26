namespace ATEMkey
{
    using System;
    using System.Drawing;
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

        private void butMP1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            var idx = mediaPool.GetSlot(listBox1.Items[listBox1.SelectedIndex].ToString()) - 1;
            if (idx > -1)
                atemControl.ChangeMediaPlayer(CommandOptions.ATEMPort.Mp1, (uint)idx);
        }

        private void butMP2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            var idx = mediaPool.GetSlot(listBox1.Items[listBox1.SelectedIndex].ToString()) - 1;
            if (idx > -1)
                atemControl.ChangeMediaPlayer(CommandOptions.ATEMPort.Mp2, (uint)idx);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (listBox1.SelectedIndex == -1)
                return;
            Bitmap MyImage = new Bitmap("C:\\Private\\GitHub\\Gotthardsen\\ATEMkey\\ATEMkey\\bin\\Debug\\Images\\" + listBox1.SelectedItem.ToString()+".jpg");
            //pictureBox1.ClientSize = new Size(xSize, ySize);
            pictureBox1.Image = (Image)MyImage;
            */
        }
    }
}

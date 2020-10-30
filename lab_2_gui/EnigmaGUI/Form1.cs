using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace EnigmaGUI
{
    public partial class ConfigForm : Form
    {
        Enigma curMachine;
        Stream streamIn;

        Enigma CurMachine
        { get; set; }
        Stream StreamIn
        { get; set; }

        public ConfigForm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void butOpenConfig_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "enigma config files (*.enigma)|*.enigma";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream fs = openFileDialog1.OpenFile();

                CurMachine = new Enigma(fs);

                textBox1.Text = openFileDialog1.FileName;
                butNextForm1.Enabled = true;
            }
        }

        private void butNextForm1_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            this.Text = "Энигма";
        }

        private void butCreateCongig_Click(object sender, EventArgs e)
        {
            int rotorAmount = (int)numericUpDown1.Value;
            Enigma machine = new Enigma(rotorAmount, 256);
            Stream myStream;

            saveFileDialog1.Filter = "enigma config files (*.enigma)|*.enigma";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    machine.SaveConfiguration(myStream);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            this.Text = "Выбор файла конфигурации";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "all files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamIn = openFileDialog1.OpenFile();

                textBox2.Text = openFileDialog1.FileName;
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream streamOut;

            saveFileDialog1.Filter = "all files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((streamOut = saveFileDialog1.OpenFile()) != null)
                {
                    CurMachine.StreamEncrypt(StreamIn, streamOut);
                }
            }
        }
    }
}

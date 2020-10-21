using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Rotor[] rotors;
        private Deflector curDeflector;

        private Rotor[] Rotors
        { get; set; }

        private Deflector CurDeflector
        { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Encoding ascii = Encoding.ASCII;

            string alphSizeInput = textBox1.Text;
            string rotorAmountInput = numericUpDown1.Text;
            String messageInput = textBox2.Text;
            

            Byte[] encodedBytes = ascii.GetBytes(messageInput);
            int[] message = new int[encodedBytes.Length];

            for (int i = 0; i < encodedBytes.Length; i++)
            {
                message[i] = encodedBytes[i];
            }

            int alphSize = Int32.Parse(alphSizeInput);
            int rotorAmount = Int32.Parse(rotorAmountInput);

            int[] alphabet = new int[alphSize];
            for (int i = 0; i < alphSize; i++)
            {
                alphabet[i] = i;
            }

            if (Rotors == null || Rotors.Length != rotorAmount)
            {
                Console.WriteLine("[NEW ROTORS]");
                Rotors = new Rotor[rotorAmount];
                for (int i = 0; i < rotorAmount; i++)
                {
                    Rotors[i] = new Rotor(alphabet);
                }
            }

            CurDeflector = new Deflector(alphabet);

            int[] encodedMessage = encode(message);
            Byte[] newEncodedBytes = new Byte[message.Length];

            for (int i = 0; i < encodedMessage.Length; i++)
            {
                newEncodedBytes[i] = (byte)encodedMessage[i];
                Console.Write($"[{encodedMessage[i]}]");
            }

            Console.WriteLine();
            String decodedString = ascii.GetString(newEncodedBytes);
            Console.WriteLine(decodedString);
            textBox3.Text = decodedString;

            
            


        }

        private int[] encode(int[] message)
        {
            int[] encryptedMessage = new int[message.Length];
            int encI = 0;
            int s = Rotors[0].Alphabet.Length;

            foreach (int sym in message)
            {
                int newSym = sym;
                foreach (Rotor rotor in Rotors)
                {
                    newSym = rotor.forward(newSym);
                }

                newSym = CurDeflector.process(newSym);

                for (int i = Rotors.Length - 1; i > -1; i--)
                {
                    newSym = Rotors[i].backward(newSym);
                }

                for (int i = 0; i < Rotors.Length; i++)
                {
                    if (Rotors[i].Pos == s - 1)
                    {
                        Rotors[i].Pos = 0;
                    }
                    else
                    {
                        Rotors[i].Pos += 1;
                        break;
                    }
                }

                encryptedMessage[encI] = newSym;
                encI += 1;
            }

            return encryptedMessage;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}

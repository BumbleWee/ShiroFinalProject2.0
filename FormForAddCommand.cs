using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ShiroFinalProject
{
    public partial class Form2 : Form
    {
        private readonly Form1 form;

        public Form2(Form1 form)
        {
            this.form = form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter streamWriter = new StreamWriter("Dictionary.dc", true);
            //streamWriter.BaseStream.Seek(file.Length, SeekOrigin.End);
            streamWriter.WriteLine('{' + textBox1.Text + "} &" + listBox1.Text + "& %" + textBox2.Text + '%');
            streamWriter.Close();
            form.AddListBox = new object[]{ textBox1.Text + " " + listBox1.Text + " " + textBox2.Text };
            textBox1.Text = "";
            textBox2.Text = "";
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox2.Text = openFileDialog1.FileName;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            listBox1.SetSelected(0, true);
        }

        private int x = 0; private int y = 0;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X; y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Location = new System.Drawing.Point(this.Location.X + (e.X - x), this.Location.Y + (e.Y - y));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            this.Hide();
        }
    }
}

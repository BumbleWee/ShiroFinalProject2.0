using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Speech.Recognition;

namespace ShiroFinalProject
{
    public partial class Form1 : Form
    {
        private readonly Form2 f2;

        private double LV = 0.5;

        public Form1()
        {
            f2 = new Form2(this);
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f2.Show();
        } 

        public object[] AddListBox
        {
            set
            {
                listBox1.Items.Add(value[0]);
            }            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("Dictionary.dc");
            int i = -1, j = 0, u = listBox1.Items.Count - 1;
            string[] str = new string[u];
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                i++;
                if (i != listBox1.SelectedIndex)
                {
                    str[j] = s;
                    j++;
                }
            }
            sr.Close();
            File.Delete("Dictionary.dc");
            StreamWriter streamWriter = new StreamWriter("Dictionary.dc", true);
            i = 0;
            foreach (string s in str)
            {
                streamWriter.WriteLine(s);
                i++;
            }
            streamWriter.Close();
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            if (listBox1.Items.Count == 0) File.Delete("Dictionary.dc");
            listBox2.Items.Add("Функция удалена...");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("Dictionary.dc"))
            {
                int it = 0;
                StreamReader sr = new StreamReader("Dictionary.dc");
                while (!sr.EndOfStream)
                {
                    it++;
                    string str = sr.ReadLine();
                    string s = "";
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i] == '{' || str[i] == '}' || str[i] == '&' || str[i] == '%') { }
                        else s += str[i];
                    }
                    listBox1.Items.Add(s);
                }
                sr.Close();

                if (File.Exists("LV.set"))
                {
                    trackBar1.Value = Convert.ToInt32(File.ReadAllText("LV.set"));
                    LV = Convert.ToInt32(File.ReadAllText("LV.set")) / 10;
                }

                //Thread th = new Thread(Speech);
                //th.Start();
                Speech();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (File.Exists("LV.set")) File.Delete("LV.set");
            File.WriteAllText("LV.set", trackBar1.Value.ToString());
            Application.Restart();
        }

        private void Speech()
        {            
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();

            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ci;
            Choices ch = new Choices(); 
            StreamReader sr = new StreamReader("Dictionary.dc");
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                string s = "";
                for (int i = 1;i<str.Length;i++)
                {
                    if (str[i] == '}') { ch.Add(s); break; }
                    else s += str[i];
                }
            }
            sr.Close();
            gb.Append(ch);
            Grammar g = new Grammar(gb);
            sre.LoadGrammar(g);

            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > 0.5) SVR(e.Result.Text);
        }

        private void SVR(string s)
        {            
            StreamReader sr = new StreamReader("Dictionary.dc");
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                string st = "";
                for (int i = 1;i<str.Length;i++)
                {                    
                    if (str[i] == '}') { break; }
                    else st += str[i];
                }
                if (st == s)
                {                    
                    bool a=false, b=false;
                    string eve = "";
                    string p = "";
                    for (int i = st.Length + 3;i<str.Length;i++)
                    {
                        if (str[i] == '&') { a = !a; i++; }
                        if (str[i] == '%') {b = !b; i++; }
                        if (a) eve += str[i];
                        if (b) p += str[i];
                    }
                    if (eve == "Открыть") Open(p);
                    else if (eve == "Закрыть") Close(p);
                    else if (eve == "Выключить компьютер") PowerOff();
                    else if (eve == "Перезагрузить компьютер") Reset();
                        break;
                }
            }
            sr.Close();
        }

        private void Open(string p)
        {
            Process.Start(p);
            try { }
            catch { }
        }

        private void Close(string p)
        {
            string t = "",s="";
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i] == '\\' || p[i] == '/') t = "";
                else t += p[i];
            }
            for (int i = 0; i < p.Length; i++)
            {
                if (t[i] == '.') break;
                else s += t[i];
            }
            Process[] proc = Process.GetProcessesByName(s);
            try { }
            catch { }
            foreach (Process pr in proc)
            {               
                pr.Kill();
            }
        }

        private void PowerOff()
        {
            Process.Start("shutdown.exe", " -t 0 -s");
        }

        private void Reset()
        {
            Process.Start("shutdown.exe", " -t 0 -r");
        }
        private int x = 0; private int y = 0;
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X; y = e.Y;
        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Location = new System.Drawing.Point(this.Location.X + (e.X - x), this.Location.Y + (e.Y - y));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
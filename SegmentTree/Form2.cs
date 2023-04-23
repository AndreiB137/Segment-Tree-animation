using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SegmentTree
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        int count = 0;

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f = new Form1();
            f.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void VisitLink()
        {
            var ps1 = new ProcessStartInfo("https://www.pbinfo.ro/probleme/2090/actualizare-element-minim-interval")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps1);
            var ps2 = new ProcessStartInfo("https://www.pbinfo.ro/probleme/2094/actualizare-element-cmmdc-interval")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps2);
            var ps3 = new ProcessStartInfo("https://www.pbinfo.ro/probleme/2091/actualizare-interval-minim-interval")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Link-urile nu se pot deschide");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Query-urilor din fisier vor incepe cu cifra 1, iar update-urile cu cifra 2.\n\nCuloarea galbena va marca nodurile din arbore " +
                "care devin modificate in urma unui update sau care trebuie combinate pentru a oferi raspunsul la o intrebare. De asemenea, cu aceeasi culoare se vor indica patratelele din vector " +
                "cu intervalul unde este un query sau un update.\n\nRecomand folosirea unui N <= 8 pentru numarul de elemente din vector si valori mai mici decat 100.");
        }
    }
}

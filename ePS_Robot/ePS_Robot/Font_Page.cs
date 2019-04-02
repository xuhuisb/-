using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Windows.Forms;
using ePS_Robot.人工咨询;
using ePS_Robot.处方咨询;
using ePS_Robot.药物咨询;
using System.Speech.Synthesis;

namespace ePS_Robot
{
    public partial class Font_Page : Form
    {
        SpeechSynthesizer synth = new SpeechSynthesizer();
        public Font_Page()
        {
            InitializeComponent();
        }

        private void Font_Page_Load(object sender, EventArgs e)
        {
            synth.SpeakAsync(label1.Text);
            
        }

        private void btn_prescription_Click(object sender, EventArgs e)
        {
            Inquire_Prescription Inq_Pre = new Inquire_Prescription();
            Inq_Pre.ShowDialog();
            //this.Hide();
        }

        private void btn_medicine_Click(object sender, EventArgs e)
        {
            Inquire_Medicine Inq_Med = new Inquire_Medicine();
            Inq_Med.ShowDialog();
            // this.Hide();

        }

        private void btn_manual_Click(object sender, EventArgs e)
        {
            Inquire_Manual Inq_Man = new Inquire_Manual();
            Inq_Man.ShowDialog();
            //this.Hide();
        }


    }
}

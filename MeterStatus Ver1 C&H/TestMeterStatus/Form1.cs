using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestMeterStatus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MeterStatus._303 _303 = new MeterStatus._303();
            _303.statusRegister_0000600404FF(textBox1.Text, "Farsi");
        }
    }
   
}

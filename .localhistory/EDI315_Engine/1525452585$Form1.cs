using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDI315_Engine
{
    public partial class form_main : Form
    {
        private EngineService engineService = new EngineService();

        public form_main()
        {
            InitializeComponent();
        }
        private void form_main_Load(object sender, EventArgs e)
        {
            btn_Start.PerformClick();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (btn_Start.Text == "START")
            {
                btn_Start.Text = "STOP";
                addListBox("TEST");

                timer_mainTimer.Start();
            }
            else
            {
                btn_Start.Text = "START";
                addListBox("TEST");
            }
        }



        private void timer_mainTimer_Tick(object sender, EventArgs e)
        {

        }

        private void addListBox(string strItem)
        {
            // Total 10 item can be shown.
            if (listbox_result.Items.Count > 9)
                listbox_result.Items.Clear();

            listbox_result.Items.Add(strItem);
        }

        
    }
}

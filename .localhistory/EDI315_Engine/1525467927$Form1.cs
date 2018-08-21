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
    public partial class mainForm : Form
    {
        private EngineService engineService;

        public mainForm()
        {
            InitializeComponent();
            engineService = new EngineService();

            // to control Listview from EngineService class
            engineService.addListItemMainForm += new EngineService.addListBoxItem(addListBoxItem);
        }
        private void form_main_Load(object sender, EventArgs e)
        {
            btn_Start.PerformClick();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            engineService.test();
            if (btn_Start.Text == "START")
            {
                btn_Start.Text = "STOP";
                timer_mainTimer.Start();
            }
            else
            {
                btn_Start.Text = "START";
                timer_mainTimer.Stop();
            }
        }
        private void timer_mainTimer_Tick(object sender, EventArgs e)
        {
            timer_mainTimer.Stop();
            //
            engineService.RunEngine();
            //
            timer_mainTimer.Start();
        }

        private void addListBoxItem(string strItem)
        {
            // Total 10 item can be shown.
            if (listbox_result.Items.Count > 9)
                listbox_result.Items.Clear();

            listbox_result.Items.Add(strItem);
        }
    }
}

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
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
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
                listbox_result_addItem("TEST");
            }
            else
            {
                btn_Start.Text = "START";
                listbox_result.Items.Add("TEST");
            }
        }






        private void listbox_result_addItem(string strItem)
        {
            if (listbox_result.Items.Count > 10)
                listbox_result.Items.Clear();

            listbox_result.Items.Add(strItem);
        }
    }
}

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
        private EngineService engineService;

        public form_main()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            engineService = new EngineService();
        }
        private void form_main_Load(object sender, EventArgs e)
        {

            btn_Start.PerformClick();
        }
        private void btn_Start_Click(object sender, EventArgs e)
        {
            if(this.Text == "START")
            {
                this.Text = "STOP";
            }
            else
            {
                this.Text = "START";
            }
        }
    }
}

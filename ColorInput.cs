using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DHHTA
{
    public partial class ColorInput : Form
    {
        public ColorInput()
        {
            InitializeComponent();
            Ok.DialogResult = DialogResult.OK;
            Cancel.DialogResult = DialogResult.Cancel;
        }

        public int red
        {
            get { return Int32.Parse(txtRed.Text); }
            set { txtRed.Text = value.ToString(); }
        }

        public int green
        {
            get { return Int32.Parse(txtGreen.Text); }
            set { txtGreen.Text = value.ToString(); }
        }

        public int blue
        {
            get { return Int32.Parse(txtBlue.Text); }
            set { txtBlue.Text = value.ToString(); }
        }
    }
}

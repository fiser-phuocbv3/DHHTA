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
    public partial class PopupForm : Form
    {
        public PopupForm()
        {
            InitializeComponent();
            Ok.DialogResult = DialogResult.OK;
            Cancel.DialogResult = DialogResult.Cancel;
        }
    }
}

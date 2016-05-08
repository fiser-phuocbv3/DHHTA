using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;

namespace DHHTA
{
    public partial class ContrastForm : Form
    {
        // private DHHTA.FilterPreview filterPrevie;
        private ContrastCorrection filter = new ContrastCorrection();
        public ContrastForm()
        {
            InitializeComponent();
           
            tbContrast.Value = (int)(2000);
            filterPreview.Filter = filter;

            ok.DialogResult = DialogResult.OK;
            cancel.DialogResult = DialogResult.Cancel;
        }

        public Bitmap Image
        {
            set { filterPreview.Image = value; }
            get { return filterPreview.Image; }
        }

        public IFilter Filter
        {
            get { return filter; }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double d = (double)tbContrast.Value / 1000;
            label2.Text = d.ToString();
            filter.Factor = d;
            filterPreview.RefreshFilter();
        }    
    }
}

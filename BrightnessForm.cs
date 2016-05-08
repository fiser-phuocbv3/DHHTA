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
    public partial class BrightnessForm : Form
    {
        // private DHHTA.FilterPreview filterPrevie;
        private BrightnessCorrection filter = new BrightnessCorrection();
        public BrightnessForm()
        {
            InitializeComponent();
           
            trackBar1.Value = (int)(filter.AdjustValue);
            filterPreview1.Filter = filter;
            ok.DialogResult = DialogResult.OK;
            cancel.DialogResult = DialogResult.Cancel;
        }

        public Bitmap Image
        {
            set { filterPreview1.Image = value; }
            get { return filterPreview1.Image; }
        }

        public IFilter Filter
        {
            get { return filter; }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double d = (double)trackBar1.Value / 1000;
            label2.Text = d.ToString();
            filter.AdjustValue = d;
            filterPreview1.RefreshFilter();
        }


    }
}

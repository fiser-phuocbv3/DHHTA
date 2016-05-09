using System;
using System.Drawing;
using System.Windows.Forms;
using DHHTA.Core;
using DHHTA;
using System.Collections;
using AForge.Imaging.Filters;
using AForge.Imaging;
using System.Drawing.Drawing2D;

namespace DHHTA
{
    public partial class Form1 : Form
    {
        private Bitmap bitmap;
        private Bitmap undo;
        private Bitmap first;
        private Bitmap image;
        private Bitmap backup;
        private IDocumentsHost host = null;
        private Stack stackUndo;
        private Stack stackRedo;
        private double zoom = 1.0;

        public Form1()
        {
            InitializeComponent();
            //bitmap = new Bitmap(2,2);
            stackUndo = new Stack();
            stackRedo = new Stack();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if(bitmap != null)
            {
                Graphics g = e.Graphics;
                Rectangle rc = ClientRectangle;
                Pen pen = new Pen(Color.FromArgb(0, 0, 0));

                int width = (int)(bitmap.Width * zoom);
                int height = (int)(bitmap.Height * zoom);
                int x = (rc.Width < width) ? this.AutoScrollPosition.X : (rc.Width - width) / 2;
                int y = (rc.Width < width) ? this.AutoScrollPosition.Y : (rc.Height - height) / 2;
                g.DrawRectangle(pen, x - 1, y - 1, width + 1, height + 1);
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(bitmap, new Rectangle(x, y, width, height));
                pen.Dispose();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }



        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp|Jpeg files (*.jpg)|*.jpg|All files (*.bmp/*.jpg)|*.bmp/*.jpg";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                bitmap = (Bitmap)Bitmap.FromFile(openFileDialog.FileName, false);
                first = bitmap;
                //stackUndo.Push(bitmap);
                this.AutoScroll = true;
                this.AutoScrollMinSize = new Size((int)(bitmap.Width), (int)(bitmap.Height));
                this.Invalidate();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp|Jpeg files (*.jpg)|*.jpg|All files (*.bmp/*.jpg)|*.bmp/*.jpg";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                bitmap.Save(saveFileDialog.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undo  = (Bitmap)bitmap.Clone();
            stackUndo.Push(bitmap);
            if (Process.Invert(bitmap))
                this.Invalidate();       
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
      
            stackUndo.Push(bitmap);
            if (Process.GrayScale(bitmap))        
                this.Invalidate();                   
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopupForm popup = new PopupForm();
            popup.Value = "0";
            if(popup.ShowDialog() == DialogResult.OK)
            {
              
                stackUndo.Push(bitmap);
                if (Process.Brightness(bitmap, Int32.Parse(popup.Value)))
               
                    this.Invalidate();         
            }
        }

        private void contrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopupForm popup = new PopupForm();
            popup.Value = "0";
            if(popup.ShowDialog() == DialogResult.OK)
            {
              //  undo = (Bitmap)bitmap.Clone();
                stackUndo.Push(bitmap);
                if (Process.Contrast(bitmap, (sbyte)Int32.Parse(popup.Value)))
              
                    
                    this.Invalidate();
                                 
            }
        }

        //color
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorInput colorInput = new ColorInput();
            colorInput.red = 0;
            colorInput.green = 0;
            colorInput.blue = 0;
            if(colorInput.ShowDialog() == DialogResult.OK)
            {
                //undo = (Bitmap)bitmap.Clone();
                stackUndo.Push(bitmap);
                if (Process.Color(bitmap, colorInput.red, colorInput.green, colorInput.blue))
               
                    this.Invalidate();
                           
            }
        }

        //lam min
        private void smoothToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // undo = (Bitmap)bitmap.Clone();
            stackUndo.Push(bitmap);
            if (Process.Smooth(bitmap, 1))
            
                
                this.Invalidate();
                        
        }

        private void kirshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopupForm popup = new PopupForm();
            popup.Value = "0";
            if (popup.ShowDialog() == DialogResult.OK)
            {
                undo = (Bitmap)bitmap.Clone();
                stackUndo.Push(bitmap);
                if (Process.EdgeDetectConvolution(bitmap, Process.EDGE_DETECT_KIRSH, (byte)Int32.Parse(popup.Value)))
                {
                   
                    this.Invalidate();
                }                 
            }
        }

        private void prewittToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopupForm popup = new PopupForm();
            popup.Value = "0";
            if (popup.ShowDialog() == DialogResult.OK)
            {
                undo = (Bitmap)bitmap.Clone();
                stackUndo.Push(bitmap);
                if (Process.EdgeDetectConvolution(bitmap, Process.EDGE_DETECT_PREWITT, (byte)Int32.Parse(popup.Value)))
                {
                  
                    this.Invalidate();
                }                  
            }
        }

        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PopupForm popup = new PopupForm();
            popup.Value = "0";
            if (popup.ShowDialog() == DialogResult.OK)
            {
                undo = (Bitmap)bitmap.Clone();
                stackUndo.Push(bitmap);
                if (Process.EdgeDetectConvolution(bitmap, Process.EDGE_DETECT_SOBEL, (byte)Int32.Parse(popup.Value)))
                {
                   
                    this.Invalidate();
                }                 
            }
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undo = (Bitmap)bitmap.Clone();
            stackUndo.Push(bitmap);
            if (Process.EdgeDetectHorizontal(bitmap))
            {
               
                this.Invalidate();
            }               
        }

        private void verticalStripMenuItem2_Click(object sender, EventArgs e)
        {
           // undo = (Bitmap)bitmap.Clone();
            stackUndo.Push(bitmap);
            if (Process.EdgeDetectVertical(bitmap))
            {
               
                this.Invalidate();
            }              
        }

        private void zoomToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            undo = (Bitmap)bitmap.Clone();
            var a = sender as ToolStripMenuItem;
            switch (a.Name)
            {
                case "zoom10":
                    zoom = 0.1;
                    break;
                case "zoom25":
                    zoom = 0.25;
                    break;
                case "zoom50":
                    zoom = 0.5;
                    break;
                case "zoom100":
                    zoom = 1.0;
                    break;
                case "zoom150":
                    zoom = 1.5;
                    break;
                case "zoom200":
                    zoom = 2.0;
                    break;
                case "zoom300":
                    zoom = 3.0;
                    break;
                case "zoom500":
                    zoom = 5.0;
                    break;
            }
            
            this.AutoScrollMinSize = new Size((int)(bitmap.Width * zoom), (int)(bitmap.Height * zoom));
            this.Invalidate();        
        }

        private void horzOrVertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undo = (Bitmap)bitmap.Clone();
            stackUndo.Push(bitmap);
            var obj = sender as ToolStripMenuItem;
            bool b = false;
            switch (obj.Name)
            {
                case "horz":
                    b = Process.FlipHorzOrVert(bitmap, true, false);
                    break;
                case "vert":
                    b = Process.FlipHorzOrVert(bitmap, false, true);
                    break;
                case "both":
                    b = Process.FlipHorzOrVert(bitmap, true, true);
                    break;
            }
            
            this.Invalidate();
        }

        private void swirlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undo = (Bitmap)bitmap.Clone();
            stackUndo.Push(bitmap);
            var obj = sender as ToolStripMenuItem;
            switch (obj.Name)
            {
                case "normal":
                    Process.Swirl(bitmap, 0.04, false);
                    break;
                case "antiAlias":
                    Process.Swirl(bitmap, 0.04, true);
                    break;
            }
            
            this.Invalidate();
        }

        private void sphereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undo = (Bitmap)bitmap.Clone();
            stackUndo.Push(bitmap);
            var obj = sender as ToolStripMenuItem;
            switch (obj.Name)
            {
                case "normalSphere":
                    Process.Sphere(bitmap, false);
                    break;
                case "antiAliasSphere":
                    Process.Sphere(bitmap, true);
                    break;
            }
           
            this.Invalidate();
        }

        private void Water_Click(object sender, EventArgs e)
        {
            undo = (Bitmap)bitmap.Clone();
            stackUndo.Push(bitmap);
            var obj = sender as ToolStripMenuItem;
            switch (obj.Name)
            {
                case "normalWater":
                    Process.Water(bitmap, 15, false);
                    break;
                case "antiAliasWater":
                    Process.Water(bitmap, 15, true);
                    break;
            }
            
            this.Invalidate();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = stackUndo.Count;
           
            if (stackUndo.Count > 0)
            {
                stackRedo.Push(bitmap);
                bitmap = (Bitmap)stackUndo.Pop();
               // stackRedo.Push(bitmap);
                this.Invalidate();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = stackRedo.Count;
            if (stackRedo.Count > 0)
            {
                stackUndo.Push(bitmap);
                bitmap = (Bitmap)stackRedo.Pop();
                
                this.Invalidate();
            }
        }

        //create sepia color
        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stackUndo.Push(bitmap);
            image = (Bitmap)bitmap.Clone();
            filter(new Sepia());
            
        }

        //create rotate color
        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stackUndo.Push(bitmap);
            image = (Bitmap)bitmap.Clone();
            filter(new RotateChannels());
             
        }

//extract
        // extract channel red
        private void extractChannelRedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stackUndo.Push(bitmap);
            image = (Bitmap)bitmap.Clone();
            filter(new ExtractChannel(RGB.R));
           
        }

        //extract channel green
        private void extractChannelGreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stackUndo.Push(bitmap);
            image = (Bitmap)bitmap.Clone();
            filter(new ExtractChannel(RGB.G));
           
        }

        //extract channel blue
        private void extractChannelBlueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stackUndo.Push(bitmap);
            image = (Bitmap)bitmap.Clone();
            filter(new ExtractChannel(RGB.B));
            
        }

        private void filter(IFilter filter)
        {
            try
            {
                // set wait cursor
                this.Cursor = Cursors.WaitCursor;
                // apply filter to the image
                bitmap = filter.Apply(image);//goi de ap dung filter
                // if (host.CreateNewDocumentOnChange)
                //{
                // open new image in new document
                // host.NewDocument(newImage);
                /*}
                else
                {
                    if (host.RememberOnChange)
                    {
                        // backup current image
                        if (backup != null)
                            backup.Dispose();
                        backup = image;
                    }
                    else
                    {
                        // release current image
                        image.Dispose();
                    }
                    bitmap = newImage;
                    // update
                    //UpdateNewImage();
                    this.Invalidate();
                }*/
                this.Invalidate();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Selected filter can not be applied to the image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public void saveStatus()
        {
            stackUndo.Push(bitmap);
            stackRedo.Clear();
        }

        private void brightnessToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveStatus();
            BrightnessForm form = new BrightnessForm();
            form.Image = bitmap;
            if(form.ShowDialog() == DialogResult.OK)
            {
                image = (Bitmap)bitmap.Clone();
                filter(form.Filter);
            }
        }

        private void contrastToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveStatus();
            ContrastForm form = new ContrastForm();
            form.Image = bitmap;
            if(form.ShowDialog() == DialogResult.OK)
            {
                image = (Bitmap)bitmap.Clone();
                filter(form.Filter);
            }
        }

        private void saturetionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveStatus();
            SaturationForm form = new SaturationForm();
            form.Image = bitmap;
            if(form.ShowDialog() == DialogResult.OK)
            {
                image = (Bitmap)bitmap.Clone();
                filter(form.Filter);
            }
        }

//menu - morphology

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveStatus();
            image = (Bitmap)bitmap.Clone();
            filter(new Erosion());
        }

        private void dilatationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveStatus();
            image = (Bitmap)bitmap.Clone();
            filter(new Dilatation());
        }

        private void opentingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveStatus();
            image = (Bitmap)bitmap.Clone();
            filter(new Opening());
        }

        private void colosingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveStatus();
            image = (Bitmap)bitmap.Clone();
            filter(new Closing());
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = Controller.getOpenFileDiaLog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                first = (Bitmap)Bitmap.FromFile(openFileDialog.FileName, false);
                if(first != null)
                {
                    saveStatus();
                    image = (Bitmap)bitmap.Clone();
                    filter(new Add(first));
                }
            }

        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = Controller.getOpenFileDiaLog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                first = (Bitmap)Bitmap.FromFile(openFileDialog.FileName, false);
                if(first != null)
                {
                    saveStatus();
                    image = (Bitmap)bitmap.Clone();
                    filter(new Subtract(first));
                }
            }
        }

        private void edgeDetetionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = Controller.getOpenFileDiaLog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                first = (Bitmap)Bitmap.FromFile(openFileDialog.FileName, false);
                if(first != null)
                {
                    saveStatus();
                    image = (Bitmap)bitmap.Clone();
                    filter(new Merge(first));
                }
            }
        }

        private void intersectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDiaLog = Controller.getOpenFileDiaLog();
            if(openFileDiaLog.ShowDialog() == DialogResult.OK)
            {
                first = (Bitmap)Bitmap.FromFile(openFileDiaLog.FileName, false);
                if(first != null)
                {
                    saveStatus();
                    image = (Bitmap)bitmap.Clone();
                    filter(new Intersect(first));
                }
            }
        }

        private void homogenityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveStatus();
            image = (Bitmap)bitmap.Clone();
            filter(new HomogenityEdgeDetector());
        }

        private void differenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveStatus();
            image = (Bitmap)bitmap.Clone();
            filter(new DifferenceEdgeDetector());
        }

        private void sobelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveStatus();
            image = (Bitmap)bitmap.Clone();
            filter(new SobelEdgeDetector());
        }

        private void cannyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveStatus();
            image = (Bitmap)bitmap.Clone();
            filter(new CannyEdgeDetector());
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            saveStatus();
            image = (Bitmap)bitmap.Clone();
            filter(new Median());
        }
    }
}

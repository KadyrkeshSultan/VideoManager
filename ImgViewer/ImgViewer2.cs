
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AppGlobal;
using ImgViewer.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Unity;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
using VMModels.Enums;
using VMModels.Model;

namespace ImgViewer
{
    public class ImgViewer2 : Form
    {
        private Bitmap sourceImage;
        private Bitmap filteredImage;
        private string ImageFileName;
        private PrintDocument pd;
        private IContainer components;
        private Panel HeaderPanel;
        private PictureBox picImage;
        private vComboBox cboSize;
        private PictureBox pictureBox;
        private vButton btnPrint;
        private vComboBox cboFilters;
        private Panel RecPanel;
        private Label label1;
        private Chart chart;

        
        public ImgViewer2()
        {
            ImageFileName = string.Empty;
            InitializeComponent();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        
        private void ImgViewer2_Load(object sender, EventArgs e)
        {
            Text = LangCtrl.GetString("dlg_ImgViewer", "Image Viewer");
            LangCtrl.reText(this);
            SecurityProfile();
        }

        
        private void SecurityProfile()
        {
            if (Global.IsRights(Global.RightsProfile, UserRights.PRINT))
                return;
            btnPrint.Visible = false;
        }

        
        public void OpenImage(string FileName)
        {
            RecPanel.Visible = false;
            try
            {
                Global.Log("IMAGE-VIEW", string.Format("Still Image: {0}", FileName));
                ImageFileName = FileName;
                FileStream fileStream = new FileInfo(FileName).OpenRead();
                Bitmap bitmap1 = new Bitmap(fileStream);
                sourceImage = (Bitmap)System.Drawing.Image.FromFile(FileName);
                if (sourceImage.PixelFormat != PixelFormat.Format24bppRgb)
                {
                    Bitmap bitmap2 = AForge.Imaging.Image.Clone(sourceImage, PixelFormat.Format24bppRgb);
                    sourceImage.Dispose();
                    sourceImage = bitmap2;
                }
                ClearCurrentImage();
                pictureBox.Image = sourceImage;
                picImage.Image = Utilities.resizeImage(125, 70, (System.Drawing.Image)bitmap1.Clone());
                fileStream.Close();
                Histogram();
            }
            catch
            {
            }
        }

        
        public void OpenImage(string FileName, Snapshot sRec)
        {
            RecPanel.Visible = true;
            label1.Text = string.Format(LangCtrl.GetString("iv_FrameNum", "Frame No.: {0}"), sRec.FrameNumber);
            Global.Log("IMAGE-VIEW", string.Format("Snapshot: {0}", FileName));
            ImageFileName = FileName;
            FileStream fileStream = new FileInfo(FileName).OpenRead();
            Bitmap bitmap1 = new Bitmap(fileStream);
            sourceImage = (Bitmap)System.Drawing.Image.FromFile(FileName);
            if (sourceImage.PixelFormat != PixelFormat.Format24bppRgb)
            {
                Bitmap bitmap2 = AForge.Imaging.Image.Clone(sourceImage, PixelFormat.Format24bppRgb);
                sourceImage.Dispose();
                sourceImage = bitmap2;
            }
            ClearCurrentImage();
            pictureBox.Image = sourceImage;
            picImage.Image = Utilities.resizeImage(110, 70, (System.Drawing.Image)bitmap1.Clone());
            fileStream.Close();
            Histogram();
        }

        
        private void Histogram()
        {
            try
            {
                Bitmap image = new Bitmap(this.pictureBox.Image);
                int[] values = new ImageStatisticsHSL(image).Luminance.Values;
                ImageStatistics imageStatistics = new ImageStatistics(image);
                int[] numArray1 = SmoothHistogram(imageStatistics.Red.Values);
                int[] numArray2 = SmoothHistogram(imageStatistics.Green.Values);
                int[] numArray3 = SmoothHistogram(imageStatistics.Blue.Values);
                chart.Series[0].Points.Clear();
                chart.Series[1].Points.Clear();
                chart.Series[2].Points.Clear();
                chart.Legends.Clear();
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    switch (index1)
                    {
                        case 0:
                            for (int index2 = 0; index2 < numArray1.Length; ++index2)
                                chart.Series[index1].Points.Add((double)numArray1[index2]);
                            break;
                        case 1:
                            for (int index2 = 0; index2 < numArray2.Length; ++index2)
                                chart.Series[index1].Points.Add((double)numArray2[index2]);
                            break;
                        case 2:
                            for (int index2 = 0; index2 < numArray3.Length; ++index2)
                                chart.Series[index1].Points.Add((double)numArray3[index2]);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        
        private int[] SmoothHistogram(int[] originalValues)
        {
            int[] numArray1 = new int[originalValues.Length];
            double[] numArray2 = new double[3] { 0.25, 0.5, 0.25 };
            for (int index1 = 1; index1 < originalValues.Length - 1; ++index1)
            {
                double num = 0.0;
                for (int index2 = 0; index2 < numArray2.Length; ++index2)
                    num += (double)originalValues[index1 - 1 + index2] * numArray2[index2];
                numArray1[index1] = (int)num;
            }
            return numArray1;
        }

        
        private void cboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboSize.SelectedIndex)
            {
                case 0:
                    pictureBox.SizeMode = PictureBoxSizeMode.Normal;
                    break;
                case 1:
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                case 2:
                    pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;
            }
        }

        
        private void cboFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            cboFilters.CloseDropDown();
            switch (cboFilters.SelectedIndex)
            {
                case 0:
                    ClearCurrentImage();
                    pictureBox.Image = this.sourceImage;
                    break;
                case 1:
                    ApplyFilter(Grayscale.CommonAlgorithms.BT709);
                    break;
                case 2:
                    ApplyFilter(new Sepia());
                    break;
                case 3:
                    ApplyFilter(new Invert());
                    break;
                case 4:
                    ApplyFilter(new RotateChannels());
                    break;
                case 5:
                    ApplyFilter(new ColorFiltering(new IntRange(25, 230), new IntRange(25, 230), new IntRange(25, 230)));
                    break;
                case 6:
                    ApplyFilter(new HueModifier(50));
                    break;
                case 7:
                    ApplyFilter(new SaturationCorrection(0.15f));
                    break;
                case 8:
                    ApplyFilter(new BrightnessCorrection());
                    break;
                case 9:
                    ApplyFilter(new ContrastCorrection());
                    break;
                case 10:
                    ApplyFilter(new HSLFiltering(new IntRange(330, 30), new Range(0.0f, 1f), new Range(0.0f, 1f)));
                    break;
                case 11:
                    ApplyFilter(new YCbCrFiltering(new Range(0.2f, 0.9f), new Range(-0.3f, 0.3f), new Range(-0.3f, 0.3f)));
                    break;
                case 12:
                    Bitmap sourceImage1 = this.sourceImage;
                    sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(this.sourceImage);
                    ApplyFilter(new Threshold());
                    sourceImage.Dispose();
                    sourceImage = sourceImage1;
                    break;
                case 14:
                    ApplyFilter(new Sharpen());
                    break;
                case 15:
                    Bitmap sourceImage2 = this.sourceImage;
                    sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
                    ApplyFilter(new DifferenceEdgeDetector());
                    sourceImage.Dispose();
                    sourceImage = sourceImage2;
                    break;
                case 16:
                    Bitmap sourceImage3 = this.sourceImage;
                    sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(this.sourceImage);
                    ApplyFilter(new HomogenityEdgeDetector());
                    sourceImage.Dispose();
                    sourceImage = sourceImage3;
                    break;
                case 17:
                    Bitmap sourceImage4 = this.sourceImage;
                    sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(this.sourceImage);
                    ApplyFilter(new SobelEdgeDetector());
                    sourceImage.Dispose();
                    sourceImage = sourceImage4;
                    break;
                case 18:
                    ApplyFilter(new LevelsLinear()
                    {
                        InRed = new IntRange(30, 230),
                        InGreen = new IntRange(50, 240),
                        InBlue = new IntRange(10, 210)
                    });
                    break;
            }
            Histogram();
            Cursor = Cursors.Default;
        }

        
        private void ClearCurrentImage()
        {
            pictureBox.Image = null;
            if (filteredImage == null)
                return;
            filteredImage.Dispose();
            filteredImage = null;
        }

        
        private void ApplyFilter(IFilter filter)
        {
            ClearCurrentImage();
            filteredImage = filter.Apply(this.sourceImage);
            pictureBox.Image = this.filteredImage;
        }

        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                pd = new PrintDocument();
                pd.PrintPage -= new PrintPageEventHandler(pd_PrintPage);
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                if (new PrintDialog() { Document = pd }.ShowDialog() == DialogResult.OK)
                {
                    pd.Print();
                    Global.Log("PRINT", string.Format("Print Image: {0}", ImageFileName));
                }
                pd.PrintPage -= new PrintPageEventHandler(pd_PrintPage);
            }
            catch
            {
            }
        }

        
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (!File.Exists(this.ImageFileName))
                    return;
                System.Drawing.Image image = this.pictureBox.Image;
                Rectangle marginBounds = e.MarginBounds;
                if ((double)image.Width / (double)image.Height > (double)marginBounds.Width / (double)marginBounds.Height)
                    marginBounds.Height = (int)((double)image.Height / (double)image.Width * (double)marginBounds.Width);
                else
                    marginBounds.Width = (int)((double)image.Width / (double)image.Height * (double)marginBounds.Height);
                e.Graphics.DrawImage(image, marginBounds);
            }
            catch (Exception ex)
            {
            }
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            ListItem listItem = new ListItem();
            ListItem listItem1 = new ListItem();
            ListItem listItem2 = new ListItem();
            ListItem listItem3 = new ListItem();
            ListItem listItem4 = new ListItem();
            ListItem listItem5 = new ListItem();
            ListItem listItem6 = new ListItem();
            ListItem listItem7 = new ListItem();
            ListItem listItem8 = new ListItem();
            ListItem listItem9 = new ListItem();
            ListItem listItem10 = new ListItem();
            ListItem listItem11 = new ListItem();
            ListItem listItem12 = new ListItem();
            ListItem listItem13 = new ListItem();
            ListItem listItem14 = new ListItem();
            ListItem listItem15 = new ListItem();
            ListItem listItem16 = new ListItem();
            ListItem listItem17 = new ListItem();
            ListItem listItem18 = new ListItem();
            ListItem listItem19 = new ListItem();
            ListItem listItem20 = new ListItem();
            ChartArea chartArea = new ChartArea();
            Series series = new Series();
            Series green = new Series();
            Series blue = new Series();
            this.HeaderPanel = new Panel();
            this.cboSize = new vComboBox();
            this.pictureBox = new PictureBox();
            this.picImage = new PictureBox();
            this.cboFilters = new vComboBox();
            this.btnPrint = new vButton();
            this.RecPanel = new Panel();
            this.label1 = new Label();
            this.chart = new Chart();
            this.HeaderPanel.SuspendLayout();
            ((ISupportInitialize)this.pictureBox).BeginInit();
            ((ISupportInitialize)this.picImage).BeginInit();
            this.RecPanel.SuspendLayout();
            ((ISupportInitialize)this.chart).BeginInit();
            base.SuspendLayout();
            this.HeaderPanel.BackColor = Color.FromArgb(64, 64, 64);
            this.HeaderPanel.Controls.Add(this.chart);
            this.HeaderPanel.Controls.Add(this.RecPanel);
            this.HeaderPanel.Controls.Add(this.btnPrint);
            this.HeaderPanel.Controls.Add(this.cboFilters);
            this.HeaderPanel.Controls.Add(this.cboSize);
            this.HeaderPanel.Controls.Add(this.picImage);
            this.HeaderPanel.Dock = DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new Size(884, 80);
            this.HeaderPanel.TabIndex = 0;
            this.cboSize.BackColor = Color.White;
            this.cboSize.DefaultText = "Size Mode...";
            this.cboSize.DisplayMember = "";
            this.cboSize.DropDownList = true;
            this.cboSize.DropDownMaximumSize = new Size(1000, 1000);
            this.cboSize.DropDownMinimumSize = new Size(10, 10);
            this.cboSize.DropDownResizeDirection = SizingDirection.Both;
            this.cboSize.DropDownWidth = 205;
            listItem.RoundedCornersMask = 15;
            listItem.Text = "Normal";
            listItem1.RoundedCornersMask = 15;
            listItem1.Text = "Stretched";
            listItem2.RoundedCornersMask = 15;
            listItem2.Text = "Center";
            this.cboSize.Items.Add(listItem);
            this.cboSize.Items.Add(listItem1);
            this.cboSize.Items.Add(listItem2);
            this.cboSize.Location = new System.Drawing.Point(10, 9);
            this.cboSize.Name = "cboSize";
            this.cboSize.RoundedCornersMaskListItem = 15;
            this.cboSize.SelectedIndex = 1;
            this.cboSize.Size = new Size(205, 23);
            this.cboSize.TabIndex = 1;
            this.cboSize.Text = "Stretched";
            this.cboSize.UseThemeBackColor = false;
            this.cboSize.UseThemeDropDownArrowColor = true;
            this.cboSize.ValueMember = "";
            this.cboSize.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboSize.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboSize.SelectedIndexChanged += new EventHandler(this.cboSize_SelectedIndexChanged);
            this.pictureBox.Dock = DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 80);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new Size(884, 582);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            this.picImage.BackColor = Color.Black;
            this.picImage.Dock = DockStyle.Right;
            this.picImage.Location = new System.Drawing.Point(759, 0);
            this.picImage.Name = "picImage";
            this.picImage.Size = new Size(125, 80);
            this.picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 0;
            this.picImage.TabStop = false;
            this.cboFilters.BackColor = Color.White;
            this.cboFilters.DefaultText = "Filters...";
            this.cboFilters.DisplayMember = "";
            this.cboFilters.DropDownMaximumSize = new Size(1000, 1000);
            this.cboFilters.DropDownMinimumSize = new Size(10, 10);
            this.cboFilters.DropDownResizeDirection = SizingDirection.Both;
            this.cboFilters.DropDownWidth = 205;
            listItem3.RoundedCornersMask = 15;
            listItem3.Text = "None";
            listItem4.RoundedCornersMask = 15;
            listItem4.Text = "Gray Scale";
            listItem5.RoundedCornersMask = 15;
            listItem5.Text = "Sepia";
            listItem6.RoundedCornersMask = 15;
            listItem6.Text = "Invert";
            listItem7.RoundedCornersMask = 15;
            listItem7.Text = "Rotate Channels";
            listItem8.RoundedCornersMask = 15;
            listItem8.Text = "Color Filtering";
            listItem9.RoundedCornersMask = 15;
            listItem9.Text = "Hue Modifier";
            listItem10.RoundedCornersMask = 15;
            listItem10.Text = "Saturation Adjusting";
            listItem11.RoundedCornersMask = 15;
            listItem11.Text = "Brightness Adjusting";
            listItem12.RoundedCornersMask = 15;
            listItem12.Text = "Contrast Adjusting";
            listItem13.RoundedCornersMask = 15;
            listItem13.Text = "HSL Filtering";
            listItem14.RoundedCornersMask = 15;
            listItem14.Text = "YCbCr Filtering";
            listItem15.RoundedCornersMask = 15;
            listItem15.Text = "Threshold Binarization";
            listItem16.RoundedCornersMask = 15;
            listItem16.Text = "Sharpen Image";
            listItem17.RoundedCornersMask = 15;
            listItem17.Text = "Difference Edge Detector";
            listItem18.RoundedCornersMask = 15;
            listItem18.Text = "Homogenity Edge Detector";
            listItem19.RoundedCornersMask = 15;
            listItem19.Text = "Sobel Edge Detector";
            listItem20.RoundedCornersMask = 15;
            listItem20.Text = "Levels Linear Correction";
            this.cboFilters.Items.Add(listItem3);
            this.cboFilters.Items.Add(listItem4);
            this.cboFilters.Items.Add(listItem5);
            this.cboFilters.Items.Add(listItem6);
            this.cboFilters.Items.Add(listItem7);
            this.cboFilters.Items.Add(listItem8);
            this.cboFilters.Items.Add(listItem9);
            this.cboFilters.Items.Add(listItem10);
            this.cboFilters.Items.Add(listItem11);
            this.cboFilters.Items.Add(listItem12);
            this.cboFilters.Items.Add(listItem13);
            this.cboFilters.Items.Add(listItem14);
            this.cboFilters.Items.Add(listItem15);
            this.cboFilters.Items.Add(listItem16);
            this.cboFilters.Items.Add(listItem17);
            this.cboFilters.Items.Add(listItem18);
            this.cboFilters.Items.Add(listItem19);
            this.cboFilters.Items.Add(listItem20);
            this.cboFilters.Location = new System.Drawing.Point(10, 49);
            this.cboFilters.Name = "cboFilters";
            this.cboFilters.RoundedCornersMaskListItem = 15;
            this.cboFilters.Size = new Size(205, 23);
            this.cboFilters.TabIndex = 2;
            this.cboFilters.UseThemeBackColor = false;
            this.cboFilters.UseThemeDropDownArrowColor = true;
            this.cboFilters.ValueMember = "";
            this.cboFilters.VIBlendScrollBarsTheme = VIBLEND_THEME.VISTABLUE;
            this.cboFilters.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.cboFilters.SelectedIndexChanged += new EventHandler(this.cboFilters_SelectedIndexChanged);
            this.btnPrint.AllowAnimations = true;
            this.btnPrint.Anchor = AnchorStyles.Right;
            this.btnPrint.BackColor = Color.Transparent;
            this.btnPrint.Image = Resources.print;
            this.btnPrint.Location = new System.Drawing.Point(502, 15);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.PaintBorder = false;
            this.btnPrint.PaintDefaultBorder = false;
            this.btnPrint.PaintDefaultFill = false;
            this.btnPrint.RoundedCornersMask = 15;
            this.btnPrint.RoundedCornersRadius = 0;
            this.btnPrint.Size = new Size(63, 53);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.VIBlendTheme = VIBLEND_THEME.VISTABLUE;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.RecPanel.Anchor = AnchorStyles.Left;
            this.RecPanel.Controls.Add(this.label1);
            this.RecPanel.Location = new System.Drawing.Point(221, 9);
            this.RecPanel.Name = "RecPanel";
            this.RecPanel.Size = new Size(175, 63);
            this.RecPanel.TabIndex = 4;
            this.RecPanel.Visible = false;
            this.label1.AutoSize = true;
            this.label1.ForeColor = Color.White;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisX.MinorTickMark.TickMarkStyle = TickMarkStyle.None;
            chartArea.AxisX2.MajorTickMark.Enabled = false;
            chartArea.AxisX2.MinorTickMark.TickMarkStyle = TickMarkStyle.None;
            chartArea.AxisY.IsMarginVisible = false;
            chartArea.AxisY.LabelAutoFitMaxFontSize = 6;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisY.MinorTickMark.TickMarkStyle = TickMarkStyle.None;
            chartArea.AxisY.ScaleBreakStyle.StartFromZero = StartFromZero.Yes;
            chartArea.AxisY2.MajorGrid.Enabled = false;
            chartArea.AxisY2.MajorTickMark.Enabled = false;
            chartArea.AxisY2.MajorTickMark.TickMarkStyle = TickMarkStyle.None;
            chartArea.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea);
            this.chart.Dock = DockStyle.Right;
            this.chart.Location = new System.Drawing.Point(581, 0);
            this.chart.Margin = new Padding(1);
            this.chart.Name = "chart";
            series.BorderColor = Color.Red;
            series.ChartArea = "ChartArea1";
            series.ChartType = SeriesChartType.Area;
            series.Color = Color.Red;
            series.IsVisibleInLegend = false;
            series.IsXValueIndexed = true;
            series.Name = "Red";
            series.XValueType = ChartValueType.Int32;
            series.YValueType = ChartValueType.Int32;
            green.ChartArea = "ChartArea1";
            green.ChartType = SeriesChartType.Area;
            green.Color = Color.Green;
            green.IsVisibleInLegend = false;
            green.IsXValueIndexed = true;
            green.Name = "Green";
            green.XValueType = ChartValueType.Int32;
            green.YValueType = ChartValueType.Int32;
            blue.ChartArea = "ChartArea1";
            blue.ChartType = SeriesChartType.Area;
            blue.Color = Color.Blue;
            blue.IsVisibleInLegend = false;
            blue.IsXValueIndexed = true;
            blue.Name = "Blue";
            blue.XValueType = ChartValueType.Int32;
            blue.YValueType = ChartValueType.Int32;
            this.chart.Series.Add(series);
            this.chart.Series.Add(green);
            this.chart.Series.Add(blue);
            this.chart.Size = new Size(178, 80);
            this.chart.TabIndex = 5;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(884, 662);
            base.Controls.Add(this.pictureBox);
            base.Controls.Add(this.HeaderPanel);
            this.MinimumSize = new Size(800, 600);
            base.Name = "ImgViewer2";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Image Viewer";
            base.Load += new EventHandler(this.ImgViewer2_Load);
            this.HeaderPanel.ResumeLayout(false);
            ((ISupportInitialize)this.pictureBox).EndInit();
            ((ISupportInitialize)this.picImage).EndInit();
            this.RecPanel.ResumeLayout(false);
            this.RecPanel.PerformLayout();
            ((ISupportInitialize)this.chart).EndInit();
            base.ResumeLayout(false);
        }
    }
}

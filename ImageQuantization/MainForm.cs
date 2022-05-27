using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
           
            watch.Start();
            
            int x=DistinctColours.get_distinct_colour(ImageMatrix);
           // int x=ImageOperations.dist_colors(ImageMatrix);

            // Console.WriteLine(x);
            double sum = ImageOperations.Min_span_tree();
            MessageBox.Show($"Number of colours:{x}\nMST sum:{sum}");
           // Console.WriteLine("his sum: " + sum);
          //  HashSet<edge>[] adj = ImageOperations.generate_adj_list();
            int k = int.Parse(Cluster_txt.Text);
           // ImageOperations.Generate_clusters(k, adj);
            Clusters clu = new Clusters(x,k);
            HashSet<edge>[] adj = clu.generate_adj_list();
            clu.Generate_clusters(adj);


        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
           

            ImageOperations.get_result_image(ImageMatrix);
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            watch.Stop();
            MessageBox.Show($"Total Execution Time in Millie seconds: {watch.ElapsedMilliseconds} ms\nTotal Execution Time in seconds: {watch.ElapsedMilliseconds/1000} s");

            pictureBox2.Visible = true;
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
///Algorithms Project
///Intelligent Scissors
///

namespace ImageQuantization
{
    /// <summary>
    /// Holds the pixel color in 3 byte values: red, green and blue
    /// </summary>
    public struct RGBPixel
    {
        public byte red, green, blue;
        public RGBPixel(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }
    }
    public struct edge
    {
        public int position;
        public RGBPixel child_node;
    }
    public struct RGBPixelD
    {
        public double red, green, blue;
    }
    /// <summary>
    /// Library of static functions that deal with images
    /// </summary>
    public class ImageOperations
    {
        /// <summary>
        /// Open an image and load it into 2D array of colors (size: Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of colors</returns>
        public static RGBPixel[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;
            RGBPixel[,] Buffer = new RGBPixel[Height, Width];
            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x].red = Buffer[y, x].green = Buffer[y, x].blue = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x].red = p[2];
                            Buffer[y, x].green = p[1];
                            Buffer[y, x].blue = p[0];
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }
        
        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(RGBPixel[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[2] = ImageMatrix[i, j].red;
                        p[1] = ImageMatrix[i, j].green;
                        p[0] = ImageMatrix[i, j].blue;
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }


       /// <summary>
       /// Apply Gaussian smoothing filter to enhance the edge detection 
       /// </summary>
       /// <param name="ImageMatrix">Colored image matrix</param>
       /// <param name="filterSize">Gaussian mask size</param>
       /// <param name="sigma">Gaussian sigma</param>
       /// <returns>smoothed color image</returns>
        public static RGBPixel[,] GaussianFilter1D(RGBPixel[,] ImageMatrix, int filterSize, double sigma)
        {
            int Height = GetHeight(ImageMatrix);
            int Width = GetWidth(ImageMatrix);

            RGBPixelD[,] VerFiltered = new RGBPixelD[Height, Width];
            RGBPixel[,] Filtered = new RGBPixel[Height, Width];

           
            // Create Filter in Spatial Domain:
            //=================================
            //make the filter ODD size
            if (filterSize % 2 == 0) filterSize++;

            double[] Filter = new double[filterSize];

            //Compute Filter in Spatial Domain :
            //==================================
            double Sum1 = 0;
            int HalfSize = filterSize / 2;
            for (int y = -HalfSize; y <= HalfSize; y++)
            {
                //Filter[y+HalfSize] = (1.0 / (Math.Sqrt(2 * 22.0/7.0) * Segma)) * Math.Exp(-(double)(y*y) / (double)(2 * Segma * Segma)) ;
                Filter[y + HalfSize] = Math.Exp(-(double)(y * y) / (double)(2 * sigma * sigma));
                Sum1 += Filter[y + HalfSize];
            }
            for (int y = -HalfSize; y <= HalfSize; y++)
            {
                Filter[y + HalfSize] /= Sum1;
            }

            //Filter Original Image Vertically:
            //=================================
            int ii, jj;
            RGBPixelD Sum;
            RGBPixel Item1;
            RGBPixelD Item2;

            for (int j = 0; j < Width; j++)
                for (int i = 0; i < Height; i++)
                {
                    Sum.red = 0;
                    Sum.green = 0;
                    Sum.blue = 0;
                    for (int y = -HalfSize; y <= HalfSize; y++)
                    {
                        ii = i + y;
                        if (ii >= 0 && ii < Height)
                        {
                            Item1 = ImageMatrix[ii, j];
                            Sum.red += Filter[y + HalfSize] * Item1.red;
                            Sum.green += Filter[y + HalfSize] * Item1.green;
                            Sum.blue += Filter[y + HalfSize] * Item1.blue;
                        }
                    }
                    VerFiltered[i, j] = Sum;
                }

            //Filter Resulting Image Horizontally:
            //===================================
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    Sum.red = 0;
                    Sum.green = 0;
                    Sum.blue = 0;
                    for (int x = -HalfSize; x <= HalfSize; x++)
                    {
                        jj = j + x;
                        if (jj >= 0 && jj < Width)
                        {
                            Item2 = VerFiltered[i, jj];
                            Sum.red += Filter[x + HalfSize] * Item2.red;
                            Sum.green += Filter[x + HalfSize] * Item2.green;
                            Sum.blue += Filter[x + HalfSize] * Item2.blue;
                        }
                    }
                    Filtered[i, j].red = (byte)Sum.red;
                    Filtered[i, j].green = (byte)Sum.green;
                    Filtered[i, j].blue = (byte)Sum.blue;
                }

            return Filtered;
        }
        public  static List<RGBPixel> dist_colours = new List<RGBPixel>();
        public static int number_of_coulurs;
        public static bool[,,] arrColour = new bool[256,256,256];



        public static HeapNode[] all_vertices;
        static List<HeapNode>  mstList = new List<HeapNode>();
        public static double Min_span_tree()
        {
          
            MST mst = new MST(number_of_coulurs);
            
            mst.Building_theHeap();
            double sum= mst.Calculate_MST_sum();
            sum = MST.sum;
            all_vertices = mst.getAllVertices();
          //  Console.WriteLine(sum);
            return sum;
        


        }

        
       

        public static RGBPixel[,,] arr = new RGBPixel[256, 256, 256];
        public static RGBPixel average(int r ,int g, int b, int count)
        {
            //int r = 0;
            //int g = 0;
            //int b = 0;
            //foreach (RGBPixel s in dist_colours)
            //{
            //    r = s.red;
            //    g = s.green;
            //    b = s.blue;
            //}
            //RGBPixel stor = new RGBPixel(0, 0, 0);
            //r = r / all_colours.Length;
            //g = g / all_colours.Length;
            //b = b / all_colours.Length;
            //stor.red = (byte)r;
            //stor.green = (byte)g;
            //stor.blue = (byte)b;
            //for (int j = 0; j < z.Count; j++)
            //{
            //    arr[z[j].red, z[j].blue, z[j].green] = stor;
            //}
            RGBPixel result = new RGBPixel((byte)(r/count), (byte)(g / count), (byte)(b / count));
            //Console.WriteLine(result.red);
            //Console.WriteLine(result.green);
            //Console.WriteLine(result.blue);
            //Console.WriteLine("*******");
            return result;
        
        }

        public static RGBPixel Breadth(int bb,HeapNode edge , bool bfsFirst , HashSet<edge>[] edges, bool[] checkedd,RGBPixel p)//checked //distcolors //adj
        {
            List<RGBPixel> clu = new List<RGBPixel>();//O(1) 
            Queue<HeapNode> QCluster = new Queue<HeapNode>();//O(1) 
            clu.Add(dist_colours[bb]);//O(1) 
            QCluster.Enqueue(edge);//O(1) 
            HeapNode n;//O(1) 
            int temp = 0;//O(1) 
            int temp1 = 0;//O(1) 
            int count = 0;//O(1) 
            int r=0, g=0, b=0;//O(1) 
            while (QCluster.Count > 0)//O(E) where E is the number of edges 
            {
                n = QCluster.Dequeue();//O(1) 
                int postion = n.position;//O(1) 
                if (bfsFirst)
                {
                    count++;//O(1) 
                    r += n.node.red;//O(1) 
                    g += n.node.green;//O(1) 
                    b += n.node.blue;//O(1) 
                }
                else
                {
                    arr[n.node.red, n.node.green, n.node.blue] = p;//O(1) 
                }
                checkedd[n.position] = true;//O(1) 
                if (edges[postion].Count >0)
                {
                    foreach(edge edgee in edges[postion])//O(V) 
                    {
                        if (checkedd[edgee.position] == false)//O(1) 
                        {
                            clu.Add(dist_colours[temp1]);//O(1) 
                            QCluster.Enqueue(all_vertices[edgee.position]);//O(1) 
                        }
                    }
                }
            }
            if (bfsFirst)//O(1) 
            {
                RGBPixel result = average(r, g, b, count);//O(1) 
                return result;//O(1) 
            }
            else
            {
                return new RGBPixel();//O(1) 
            }
        }
        public static void new_bfs(HeapNode element, int id, RGBPixel new_pallet, HashSet<edge>[] edges, bool[] checkedd)
        {

            Queue<HeapNode> queue = new Queue<HeapNode>();//O(1) 
            queue.Enqueue(element);//O(1) 


            while (queue.Count != 0)//O(E) 
            {
                HeapNode node = queue.Dequeue();//O(1) 
                arr[node.node.red, node.node.blue, node.node.green] = new_pallet;//O(1) 
                checkedd[node.position] = true;//O(1) 
                if (edges[node.position].Count != 0)//O(1) 
                {
                    foreach (edge v in edges[node.position])//O(V) 
                    {
                        if (!(checkedd[v.position]))//O(1) 
                        {
                            queue.Enqueue(MST.all_vertices[v.position]);//O(1) 
                        }
                    }
                }

            }

        }

        public static void get_result_image(RGBPixel[,] image)
        {
        
            for (int i = 0; i < GetHeight(image); i++)//O(n)
            {
                for (int j = 0; j < GetWidth(image); j++)//O(n)
                {

                    image[i, j] = arr[image[i, j].red, image[i, j].blue, image[i, j].green];//O(1)

                }
            }

        }

    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace ImageQuantization
//{
//    class BFS
//    {
//        List<RGBPixel> clu ;
//       static  Queue<HeapNode> QCluster ;
//        static HeapNode n;
//        public static RGBPixel[,,] arr;


//        public BFS()
//        {
//            clu = new List<RGBPixel>();
//            QCluster = new Queue<HeapNode>();
//            arr = new RGBPixel[256, 256, 256];

//        }

//        public static RGBPixel Breadth(int bb, HeapNode edge, bool bfsFirst, HashSet<edge>[] edges, bool[] checkedd, RGBPixel p)//checked //distcolors //adj
//        {
//            List<RGBPixel> clu = new List<RGBPixel>();
//            Queue<HeapNode> QCluster = new Queue<HeapNode>();
//            clu.Add(ImageOperations.dist_colours[bb]);
//            QCluster.Enqueue(edge);
        
//            int temp = 0;
//            int temp1 = 0;
//            int count = 0;
//            int r = 0, g = 0, b = 0;
//            while (QCluster.Count > 0)
//            {

//                n = QCluster.Dequeue();
//                int postion = n.position;
//                if (bfsFirst)
//                {
//                    count++;
//                    r += n.node.red;
//                    g += n.node.green;
//                    b += n.node.blue;



//                }
//                else
//                {
//                    arr[n.node.red, n.node.green, n.node.blue] = p;
//                    //    Console.WriteLine(arr[n.node.red, n.node.green, n.node.blue]);

//                    //Console.WriteLine(p.red);
//                    //Console.WriteLine(p.green);
//                    //Console.WriteLine(p.blue);
//                }
//                checkedd[n.position] = true;
//                if (edges[postion].Count > 0)
//                {
//                    foreach (edge edgee in edges[postion])
//                    {

//                        if (checkedd[edgee.position] == false)
//                        {
//                            clu.Add(ImageOperations.dist_colours[temp1]);
//                            QCluster.Enqueue(ImageOperations.all_vertices[edgee.position]);
//                        }
//                    }
//                }


//            }
//            if (bfsFirst)
//            {
//                RGBPixel result = average(r, g, b, count);
//                return result;
//            }
//            else
//            {
//                return new RGBPixel();
//            }
//        }
//        public static  RGBPixel average(int r, int g, int b, int count)
//        {
//            //int r = 0;
//            //int g = 0;
//            //int b = 0;
//            //foreach (RGBPixel s in dist_colours)
//            //{
//            //    r = s.red;
//            //    g = s.green;
//            //    b = s.blue;
//            //}
//            //RGBPixel stor = new RGBPixel(0, 0, 0);
//            //r = r / all_colours.Length;
//            //g = g / all_colours.Length;
//            //b = b / all_colours.Length;
//            //stor.red = (byte)r;
//            //stor.green = (byte)g;
//            //stor.blue = (byte)b;
//            //for (int j = 0; j < z.Count; j++)
//            //{
//            //    arr[z[j].red, z[j].blue, z[j].green] = stor;
//            //}
//            RGBPixel result = new RGBPixel((byte)(r / count), (byte)(g / count), (byte)(b / count));
//            //Console.WriteLine(result.red);
//            //Console.WriteLine(result.green);
//            //Console.WriteLine(result.blue);
//            //Console.WriteLine("*******");
//            return result;

//        }
//        public static void new_bfs(HeapNode element, int id, RGBPixel new_pallet, HashSet<edge>[] edges, bool[] checkedd)
//        {

//            QCluster = new Queue<HeapNode>();
//            QCluster.Enqueue(element);


//            while (QCluster.Count != 0)
//            {
//                HeapNode node = QCluster.Dequeue();
//                arr[node.node.red, node.node.blue, node.node.green] = new_pallet;
//                //   Console.WriteLine(arr[node.node.red, node.node.blue, node.node.green]);
//                //Console.WriteLine(new_pallet.red);
//                //Console.WriteLine(new_pallet.green);
//                //Console.WriteLine(new_pallet.blue);
//                checkedd[node.position] = true;
//                if (edges[node.position].Count != 0)
//                {
//                    foreach (edge v in edges[node.position])
//                    {

//                        if (!(checkedd[v.position]))
//                        {
//                            QCluster.Enqueue(MST.all_vertices[v.position]);
//                        }
//                    }
//                }

//            }

//        }

//    }
//}

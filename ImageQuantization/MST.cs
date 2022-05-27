using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public class MST
    {
        int number_of_coulurs;
        public static HeapNode[] all_vertices; //mst set
        bool[] checkedd;
        public static double  sum = 0;
        MinimumHeap heap;
        public MST(int number_of_coulurs)
        {
            this.number_of_coulurs = number_of_coulurs;
            all_vertices = new HeapNode[number_of_coulurs];
            checkedd = new bool[number_of_coulurs];// visitted or not
            heap = new MinimumHeap(number_of_coulurs);
        }
     
        public void Building_theHeap() // create the heap(no graph yet)
        {
          
          
            for (int i = 0; i < number_of_coulurs; i++)//O(N)
            {
                //  Console.WriteLine("*****");
                HeapNode newnode = new HeapNode();//O(1)
                if (i == 0)
                {
                    newnode.weight = 0;//O(1)
                }
                else
                {
                    newnode.weight = double.MaxValue;//O(1)

                }

                newnode.node = ImageOperations.dist_colours[i];//O(1)
                // Console.WriteLine(newnode.node.blue + "*" + newnode.node.green + "*" + newnode.node.red);
                newnode.position = i;//O(1)
                checkedd[i] = false;//O(1)
                all_vertices[i] = newnode;//O(1)
                heap.insert(newnode);//O(N) the N is the size of the heap
            }
        }
        public double Calculate_MST_sum()//O(E log(V)) the E is the Edges, the V is the number of colours 
        {
            while (!heap.is_empty()) 
            {
                HeapNode node = heap.pop_the_root();//Olog(N) N is the number of the levels in the heap
                checkedd[node.position] = true;//O(1)
              //  sum += node.weight;//O(1)

                for (int i = 0; i < number_of_coulurs; i++)////O(E log(V))
                {
                    if (checkedd[i])//O(1)
                    {
                        continue;
                    }
                    else
                    {
                        double weight;
                        weight = Math.Sqrt((node.node.red - ImageOperations.dist_colours[i].red) * (node.node.red - ImageOperations.dist_colours[i].red) +
                               (node.node.green - ImageOperations.dist_colours[i].green) * (node.node.green - ImageOperations.dist_colours[i].green) +
                               (node.node.blue - ImageOperations.dist_colours[i].blue) * (node.node.blue - ImageOperations.dist_colours[i].blue));//O(1)
                        double nodeWeight = all_vertices[i].weight;//O(1)
                        if (nodeWeight > weight)
                        {
                            HeapNode heapNode = new HeapNode();//O(1)
                            heapNode.weight = weight;//O(1)
                            heapNode.parent_node = node.node;//O(1)
                            heapNode.position = i;//O(1)

                            heapNode.parent_position = node.position;//O(1)
                            heapNode.node = all_vertices[i].node;//O(1)
                            heap.decrease(heapNode, all_vertices[i].position);//O(E) the E is the size of the heap
                            all_vertices[i] = heapNode;//O(1) // upload in mst
                        }
                    }
                }
            }
            foreach(var x in all_vertices)//O(N) where N is the number of Distinct colours
            {
                sum += x.weight;//O(1)
            }
            return sum;//O(1)
        }
        public HeapNode[] getAllVertices()
        {
            return all_vertices;
        }
    }
}

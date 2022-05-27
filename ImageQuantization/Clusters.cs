using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public class Clusters
    {
        int number_of_coulurs;
        int numberOfClusters;
        HashSet<edge>[] adj;
        edge edgeFor1node;
        edge edgefor2node;
        edge edgeForParentnode ;
        edge edgeforChildnode ;
        static  bool[] checkedd ;
        static   bool[] checkedd2 ;
        RGBPixel[] plate ;
        RGBPixel[] rGB ;
        int edgee = 0;
        int platePos = 0;
        double MaxWeight = -10;
        int deleted_postion = -10;
        int firstEdgePostion = -10;
        int secondEdgePostion = -10;


        public Clusters(int number_of_coulurs,int numberOfClusters)
        {
            this.number_of_coulurs = number_of_coulurs;
            this.numberOfClusters = numberOfClusters;
            adj = new HashSet<edge>[number_of_coulurs];
            edgeForParentnode = new edge();
            edgeforChildnode = new edge();
            checkedd = new bool[number_of_coulurs];
            checkedd2 = new bool[number_of_coulurs];
            plate = new RGBPixel[numberOfClusters + 20];
            rGB = new RGBPixel[numberOfClusters + 20];
           


        }
        public  HashSet<edge>[] generate_adj_list()//O(N)
        {
   
            //intializing the edges
            for (int i = 0; i < number_of_coulurs; i++)//O(N) where N is the number of colours
            {
                adj[i] = new HashSet<edge>();
            }
            for (int colour = 0; colour < number_of_coulurs; colour++)//O(N)
            {

                for (int adjColour = 0; adjColour < number_of_coulurs; adjColour++)//O(1) each node has only one child
                {
                    if (ImageOperations.all_vertices[colour].node.red == ImageOperations.all_vertices[adjColour].parent_node.red &&
                       ImageOperations.all_vertices[colour].node.green == ImageOperations.all_vertices[adjColour].parent_node.green &&
                       ImageOperations.all_vertices[colour].node.blue == ImageOperations.all_vertices[adjColour].parent_node.blue)
                    {
                        //adding an edge for the child node 
                        edgeFor1node.position = ImageOperations.all_vertices[adjColour].position;//O(1)
                        edgeFor1node.child_node = ImageOperations.all_vertices[adjColour].node;//O(1)
                        adj[ImageOperations.all_vertices[colour].position].Add(edgeFor1node);//O(1)

                        //adding an edge for the parent node 
                        edgefor2node.position = ImageOperations.all_vertices[colour].position;//O(1)
                        edgefor2node.child_node = ImageOperations.all_vertices[colour].node;//O(1)
                        adj[ImageOperations.all_vertices[adjColour].position].Add(edgefor2node);//O(1)


                    }
                }
            }
    
            return adj;//O(1)
        }
        public  void Generate_clusters( HashSet<edge>[] edges)//O(C*N) where N is the number of edges and C is the number of clusters
        {
            //loop to delete the maximum k - 1 edges
            for (int edge = 0; edge < numberOfClusters - 1; edge++)//O(C) where C is the number of clusters
            {
                 MaxWeight = -10;//O(1)
                deleted_postion = -10;//O(1)
                firstEdgePostion = -10;//O(1)
                secondEdgePostion = -10;//O(1)
                for (int colour = 0; colour < number_of_coulurs; colour++)//O(N)
                {

                    if (MaxWeight < ImageOperations.all_vertices[colour].weight)//O(1)
                    {
                        firstEdgePostion = ImageOperations.all_vertices[colour].parent_position;//O(1)
                        edgeforChildnode.position = ImageOperations.all_vertices[colour].position;//O(1)
                        edgeforChildnode.child_node = ImageOperations.all_vertices[colour].node;//O(1)

                        secondEdgePostion = ImageOperations.all_vertices[colour].position;//O(1)
                        edgeForParentnode.position = ImageOperations.all_vertices[colour].parent_position;//O(1)
                        edgeForParentnode.child_node = ImageOperations.all_vertices[colour].parent_node;//O(1)

                        //updating the maximum weight
                        MaxWeight = ImageOperations.all_vertices[colour].weight;//O(1)

                        //the index to delete in the edges
                        deleted_postion = colour;//O(1)

                    }
                }
                // removing from the cluster
                ImageOperations.all_vertices[deleted_postion].weight = -10;//O(1)

                //making a disconnected graph
                edges[firstEdgePostion].Remove(edgeforChildnode);//O(1)
                edges[secondEdgePostion].Remove(edgeForParentnode);//O(1)

            }

            //BFS
            for (int i = 0; i < edges.Length; i++)//O(E) where E is the number of edges
            {

                if (!checkedd[i])
                {
                    plate[platePos] = ImageOperations.Breadth(platePos, ImageOperations.all_vertices[i], true, edges, checkedd, plate[platePos]);
                    platePos++;

                }

            }
            edgee = 0;
            platePos = 0;
            for (int i = 0; i < edges.Length; i++)//O(E) where E is the number of edges
            {

                if (!checkedd2[i])
                {
                    ImageOperations.new_bfs(ImageOperations.all_vertices[i], i, plate[platePos], edges, checkedd2);
                    platePos++;
                }

            }
        }
    }
}

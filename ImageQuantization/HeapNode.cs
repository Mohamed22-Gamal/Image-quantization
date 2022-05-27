using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    public class HeapNode
    {
        public RGBPixel node;
        public double weight;
        public RGBPixel parent_node;
        public int position;
        public int parent_position;

        //public HeapNode(RGBPixel node, double weight, RGBPixel parent_node, int position, int parent_position)
        //{
        //    this.parent_position = parent_position;
        //    this.node = node;
        //    this.weight = weight;
        //    this.parent_node = parent_node;
        //    this.position = position;
        //}
    }
}

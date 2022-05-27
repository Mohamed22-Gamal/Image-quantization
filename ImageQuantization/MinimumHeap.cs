using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class MinimumHeap
    {

        public HeapNode[] heap;
        int[] indexer;
        int capacity;
        int sizeOFheap;
        public MinimumHeap(int capacity)//O(1)
        {
            this.capacity = capacity;//O(1)
            sizeOFheap = 0;//O(1)
            indexer = new int[capacity];//O(1)
            heap = new HeapNode[capacity + 1];//O(1)
            heap[0] = new HeapNode();//O(1)
            heap[0].position = -1;//O(1)
            heap[0].weight = int.MinValue;//O(1)

        }



        public bool is_empty()//O(1)
        {
            return sizeOFheap == 0;//O(1)
        }

        public void insert(HeapNode node)
        {
            sizeOFheap++;//O(1)
            heap[sizeOFheap] = node;//O(1)
            heapify_down_to_top(sizeOFheap);//O(L) where L is the number of levels
            indexer[node.position] = sizeOFheap;//O(1)

        }
        public void heapify_down_to_top(int sizeOFheap)//O(log(L)) where L is the number of levels
        {
            int parent_index = sizeOFheap / 2;
            int cur_idx = sizeOFheap;

            while (cur_idx > 0 && heap[parent_index].weight > heap[cur_idx].weight)//O(log(L)) 
            {
                HeapNode curNode = heap[cur_idx];//O(1)
                HeapNode parentNode = heap[parent_index];//O(1)
                indexer[curNode.position] = parent_index;//O(1)
                indexer[parentNode.position] = cur_idx;//O(1)
                //indexer[heap[cur_idx].position] = parent_index;
                //indexer[heap[parent_index].position] = cur_idx;
                HeapNode tmp = heap[cur_idx];
                heap[cur_idx] = heap[parent_index];
                heap[parent_index] = tmp;
               // swap(cur_idx, parent_index);//O(1)
                cur_idx = parent_index;//O(1)
                parent_index = parent_index / 2;//O(1)
            }
        }
        public void swap(int first, int second)       //O(1)        
        {
            HeapNode temp = heap[first];        //O(1)                    
            heap[first] = heap[second];        //O(1)                      
            heap[second] = temp;                //O(1)                      
        }
        public HeapNode pop_the_root()
        {
            HeapNode root = heap[1];//O(1)
            HeapNode last = heap[sizeOFheap];//O(1)
            indexer[last.position] = 1;//O(1)
            heap[1] = last;//O(1)
            heap[sizeOFheap] = new HeapNode();//O(1)

            heapify_top_to_down(1);
            sizeOFheap--;//O(1)
            return root;
        }
        public void heapify_top_to_down(int n)//O(log(L)) where L is the number of levels
        {
            int the_smallest = n;//O(1)
            int left = n * 2;//O(1)
            int right = n * 2 + 1;//O(1)

            if (left < HeapSize() && heap[the_smallest].weight > heap[left].weight)//O(1)
            {
                the_smallest = left;//O(1)
            }
            if (right < HeapSize() && heap[the_smallest].weight > heap[right].weight)//O(1)
            {
                the_smallest = right;//O(1)
            }
            if (the_smallest != n)//O(1)
            {
                HeapNode the_smallest_node = heap[the_smallest];//O(1)
                HeapNode Nnode = heap[n];//O(1)
                indexer[the_smallest_node.position] = n;//O(1)
                indexer[Nnode.position] = the_smallest;//O(1)
                HeapNode tmp = heap[n];
                heap[n] = heap[the_smallest];
                heap[the_smallest] = tmp;
               // swap(n, the_smallest);//O(1)
                heapify_top_to_down(the_smallest);//O(log(L)) where L is the number of levels
            }
        }
        public void decrease(HeapNode node, int pos)
        {
            int indx = indexer[pos];

            heap[indx] = node;
            heapify_down_to_top(indx);
        }
        public int HeapSize()
        {
            return sizeOFheap;
        }
    }
}

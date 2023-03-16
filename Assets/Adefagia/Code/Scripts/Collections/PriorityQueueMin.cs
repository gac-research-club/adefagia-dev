using System.Collections.Generic;
using System.Text;

using UnityEngine;

using Grid = Adefagia.GridSystem.Grid;

namespace Adefagia.Collections
{
    public class PriorityQueueMin
    {

        public int size;
        protected List<Grid> heap;

        public PriorityQueueMin()
        {
            size = 0;
            heap = new List<Grid>();
        }

        void SwapValue(int indexA, int indexB)
        {
            (heap[indexA], heap[indexB]) = (heap[indexB], heap[indexA]);
        }

        int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        bool IsValidIndex(int index)
        {
            return index >= 0 && index < size;
        }

        bool IsEmpty()
        {
            return size == 0;
        }

        int GetLeftChild(int parentIndex)
        {
            return (2 * parentIndex) + 1;
        }

        int GetRightChild(int parentIndex)
        {
            return (2 * parentIndex) + 2;
        }

        protected virtual bool Equation(int indexA, int indexB)
        {
            return heap[indexA].Priority < heap[indexB].Priority;
        }

        void ShiftUp(int childIndex)
        {
            var parentIndex = GetParentIndex(childIndex);

            while (IsValidIndex(parentIndex) && Equation(childIndex, parentIndex))
            {
                SwapValue(parentIndex, childIndex);
                childIndex = parentIndex;
                parentIndex = GetParentIndex(childIndex);
            }
        }

        void ShiftDown(int parentIndex)
        {
            var leftChildIndex = GetLeftChild(parentIndex);
            var isLeftValid = IsValidIndex(leftChildIndex);

            while (isLeftValid)
            {
                var smallerChildIndex = leftChildIndex;
                var rightChildIndex = GetRightChild(parentIndex);
                var isRightValid = IsValidIndex(rightChildIndex);

                if (isRightValid && Equation(rightChildIndex, leftChildIndex))
                {
                    smallerChildIndex = rightChildIndex;
                }

                if (Equation(smallerChildIndex, parentIndex))
                {
                    SwapValue(smallerChildIndex, parentIndex);
                    parentIndex = smallerChildIndex;
                    leftChildIndex = GetLeftChild(parentIndex);
                    isLeftValid = IsValidIndex(leftChildIndex);
                }
                else
                {
                    return;
                }
            }
        }

        public void Heapify()
        {
            var startIndex = GetParentIndex(size - 1);
            if (!IsValidIndex(startIndex)) return;
        
            for (var i = startIndex; i >= 0; i--)
            {
                ShiftDown(i);
            }
        }

        public void Insert(Grid value)
        {
            heap.Add(value);
            size++;
            ShiftUp(size-1);
        }

        public Grid DeleteMin()
        {
            if (!IsEmpty())
            {
                var min = heap[0];
                SwapValue(0, size-1);
                heap.RemoveAt(size-1);
                size--;
                ShiftDown(0);

                return min;
            }

            return null;
        }

        public bool Contains(Grid grid)
        {
            return heap.Contains(grid);
        }

        public void Clear()
        {
            heap.Clear();
            size = 0;
        }
    
        public void DebugListIndex()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (var grid in heap)
            {
                // sb.Append(grid.id + ", ");
            }
            sb.Append("]");
        
            Debug.Log(sb);
        }
    }
}

namespace Collections
{
    public class PriorityQueueMax : PriorityQueueMin
    {
        protected override bool Equation(int indexA, int indexB)
        {
            return heap[indexA].priority > heap[indexB].priority;
        }
    }
}
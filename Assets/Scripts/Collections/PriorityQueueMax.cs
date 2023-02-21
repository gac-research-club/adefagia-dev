namespace adefagia.Collections
{
    public class PriorityQueueMax : PriorityQueueMin
    {
        protected override bool Equation(int indexA, int indexB)
        {
            return heap[indexA].Priority > heap[indexB].Priority;
        }
    }
}
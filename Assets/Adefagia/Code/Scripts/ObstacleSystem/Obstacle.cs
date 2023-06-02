namespace Adefagia.ObstacleSystem
{
    public class Obstacle
    {
        #region Properties
        public int X { get; }
        public int Y { get; }
        
        #endregion

        #region Constructor

        /*------------------------------------------------------------------------------------------------------------
         * Constructor
         *------------------------------------------------------------------------------------------------------------*/
        public Obstacle(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion

        

        public override string ToString()
        {
            return $"Obstacle ({X},{Y})";
        }
    }

}
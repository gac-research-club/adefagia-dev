using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class AttackHighlight : MonoBehaviour
    {
        [SerializeField] private HighlightPattern highlighPattern;
        [SerializeField] private MoveAction moveAction;
        public Vector2 playerLocation;
        private Grid[] grid;
        private Vector2[] pattern;
        private bool isHighlighted;
        
        void Start()
        {
            isHighlighted = false;
            pattern = highlighPattern.attackPatternDiamond;
            // grid = highlighPattern.GetHighlightGrid(pattern, playerLocation);
        }

        /*--------------------------------------------------------------------------
         * Saat attack button di click dan movement highlight sudah tampil, maka disable movement button
         * * function untuk menampilkan attack pattern 
         *--------------------------------------------------------------------------*/
        public void AttackButtonOnClicked()
        {
            moveAction.MoveButtonOnDisable();
            // highlighPattern.SetActiveHighlightMovement(grid);
            isHighlighted = true;
        }

        /*--------------------------------------------------------------------------
         * function ntuk mendisable attack button
         *--------------------------------------------------------------------------*/
        public void AttackButtonOnDisable()
        {
            if(isHighlighted)
            {
                // highlighPattern.SetDisableHighlightMovement(grid);
            }
            isHighlighted = false;
        }
    }
}

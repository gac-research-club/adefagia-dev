using UnityEngine;

namespace Adefagia.PlayerAction
{
    public class MoveAction : MonoBehaviour
    {
        [SerializeField] private HighlightPattern highlighPattern;
        [SerializeField] private AttackHighlight attackHighlight;
        public Vector2 playerLocation;
        private Grid[] grid;
        private Vector2[] pattern;
        private bool isHighlighted;



        void Start()
        {
            isHighlighted = false;
            pattern = highlighPattern.movementPattern;
            // grid = highlighPattern.GetHighlightGrid(pattern, playerLocation);
        }

        public void MoveButtonOnClicked()
        {
            attackHighlight.AttackButtonOnDisable();
            // highlighPattern.SetActiveHighlightMovement(grid);
            isHighlighted = true;
        }

        public void MoveButtonOnDisable()
        {
            if(isHighlighted)
            {
            // highlighPattern.SetDisableHighlightMovement(grid);
            }
            isHighlighted = false;
        }
    }
}

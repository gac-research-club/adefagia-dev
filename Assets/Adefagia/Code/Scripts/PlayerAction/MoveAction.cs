using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

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
            
            // Get Robot Selected
            // Robot robot = RobotManager.GetRobot();
            // if (robot == null) return;
            //
            // Grid gridSelected = GridManager.GetGrid();
            // if (gridSelected == null) return;
            //
            // robot.RobotGameObject.GetComponent<RobotMovement>().Move(gridSelected);
            //
            // Debug.Log("Move Clicked");
            
            isHighlighted = true;
        }

        public void MoveButtonOnDisable()
        {
            if(isHighlighted)
            {
                highlighPattern.SetDisableHighlightMovement(grid);
            }
            isHighlighted = false;
        }
    }
}

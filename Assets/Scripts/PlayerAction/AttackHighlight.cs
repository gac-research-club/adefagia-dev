using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = adefagia.Graph.Grid;
using HighlighPattern = adefagia.PlayerAction.HighlightPattern;

namespace adefagia.PlayerAction
{
    public class AttackHighlight : MonoBehaviour
    {
        [SerializeField] private HighlighPattern highlighPattern;
        [SerializeField] private MoveAction moveAction;
        public Vector2 playerLocation;
        private Grid[] grid;
        private Vector2[] pattern;
        private bool isHighlighted;
        
        void Start()
        {
            isHighlighted = false;
            pattern = highlighPattern.attackPatternDiamond;
            grid = highlighPattern.GetHighlightGrid(pattern, playerLocation);
        }

        public void AttackButtonOnClicked()
        {
            moveAction.MoveButtonOnDisable();
            highlighPattern.SetActiveHighlightMovement(grid);
            isHighlighted = true;
        }

        public void AttackButtonOnDisable()
        {
            if(isHighlighted)
            {
                highlighPattern.SetDisableHighlightMovement(grid);
            }
            isHighlighted = false;
        }
    }
}

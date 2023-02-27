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
            pattern = highlighPattern.attackPattern;
            grid = highlighPattern.GetHighlightGrid(pattern, playerLocation);
        }

        public void AttackButtonOnClicked()
        {
            highlighPattern.SetActiveHighlightMovement(grid);
            isHighlighted = true;
            // moveAction.MoveButtonOnDisable();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = adefagia.Graph.Grid;
using HighlighPattern = adefagia.PlayerAction.HighlightPattern;

namespace adefagia.PlayerAction
{
    public class MoveAction : MonoBehaviour
    {
        [SerializeField] private HighlighPattern highlighPattern;
        [SerializeField] private AttackHighlight attackHighlight;
        public Vector2 playerLocation;
        private Grid[] grid;
        private Vector2[] pattern;



        void Start()
        {
            pattern = highlighPattern.movementPattern;
            grid = highlighPattern.GetHighlightGrid(pattern, playerLocation);
        }

        public void MoveButtonOnClicked()
        {
            attackHighlight.AttackButtonOnDisable();
            highlighPattern.SetActiveHighlightMovement(grid);
        }

        public void MoveButtonOnDisable()
        {
            highlighPattern.SetDisableHighlightMovement(grid);
        }
    }
}

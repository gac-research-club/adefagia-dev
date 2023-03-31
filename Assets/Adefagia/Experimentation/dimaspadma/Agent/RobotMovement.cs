using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.Collections;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;
using UnityEngine;
using Grid = Adefagia.GridSystem.Grid;

public class RobotMovement : MonoBehaviour
{
    private AStar _aStar;

    private void Awake()
    {
        _aStar = new AStar();
    }

    public void Move(RobotController robotController, GridController gridController, float delayMove)
    {
        if (gridController == null)
        {
            Debug.LogWarning("Pathfinding failed grid null");
            return;
        }

        var grid = gridController.Grid;

        if (grid.Status != GridStatus.Free)
        {
            Debug.LogWarning("Pathfinding failed Grid not Free");
            return;
        }

        // Move
        var start = BattleManager.TeamActive.RobotControllerSelected.Robot.Location;

        var directions = _aStar.Move(start, grid);

        if (directions == null)
        {
            Debug.LogWarning("Pathfinding Failed Direction null");
            return;
        }

        // Change grid reference to
        gridController.RobotController = robotController;
        gridController.Grid.SetFree();

        StartCoroutine(MovePosition(robotController, directions, delayMove));

        robotController.Robot.ChangeLocation(grid);
        grid.SetOccupied();

        // means the robot is considered to move
        robotController.Robot.HasMove = true;

        Debug.Log($"Move to {grid}");
    }

    private IEnumerator MovePosition(RobotController robotController, List<Grid> grids, float delayMove)
    {
        foreach (var grid in grids)
        {
            robotController.MovePosition(grid);

            yield return new WaitForSeconds(delayMove);
        }
    }
}

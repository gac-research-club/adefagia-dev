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

        var directions = new AStar().Move(start, grid);
    
        if (directions == null)
        {
            Debug.LogWarning("Pathfinding Failed Direction null");
            return;
        }

        // Free grid start
        robotController.GridController.Grid.SetFree();
        
        // Change grid reference to robot
        gridController.RobotController = robotController;
        
        // Change robot reference to grid
        gridController.RobotController.GridController = gridController;

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

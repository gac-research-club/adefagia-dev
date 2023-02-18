using System.Collections;
using adefagia.Graph;
using UnityEngine;

namespace adefagia.Robot
{
    public class Spawner : MonoBehaviour
    {
        public static bool doneSpawn;
        
        public GameObject robotPrefab;

        public Vector2 spawnCoord;

        private GameObject _robot;

        private GridManager _gridManager;
        private void Start()
        {
            _gridManager = GameManager.instance.gridManager;
            StartCoroutine(SpawnRobot(spawnCoord));
        }

        IEnumerator SpawnRobot(Vector2 loc)
        {
            while (!GridManager.doneGenerateGrids)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1);
            _robot = Instantiate(robotPrefab, transform);
            _robot.GetComponent<RobotMovement>().ChangeGridBerdiri(_gridManager.GetGridByLocation(loc));

            doneSpawn = true;
        }

        public RobotMovement GetRobot()
        {
            return _robot.GetComponent<RobotMovement>();
        }
    }
}
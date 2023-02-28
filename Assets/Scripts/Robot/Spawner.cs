using System.Collections;
using adefagia.Graph;
using UnityEngine;

namespace adefagia.Robot
{
    public class Spawner : MonoBehaviour
    {
        public static bool doneSpawn;
        
        public GameObject robotPrefab;

        public GameObject enemyPrefab;

        public Vector2 spawnCoord;
        public Vector2 spawnCoordEnemy;

        private GameObject _robot;

        private GameObject _enemy;
     

        private GridManager _gridManager;
        private void Start()
        {
            _gridManager = GameManager.instance.gridManager;
            StartCoroutine(SpawnRobot(spawnCoord,spawnCoordEnemy));
        }

        IEnumerator SpawnRobot(Vector2 loc,Vector2 enemyLoc)
        {
            while (!GridManager.doneGenerateGrids)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1);
            _robot = Instantiate(robotPrefab, transform);
            _robot.GetComponent<RobotMovement>().ChangeGridBerdiri(_gridManager.GetGridByLocation(loc));

            SpawnEnemy(enemyLoc);

            doneSpawn = true;
        }

        public RobotMovement GetRobot()
        {
            return _robot.GetComponent<RobotMovement>();
        }

        public void SpawnEnemy(Vector2 enemyLoc)
        {
            _enemy = Instantiate(enemyPrefab, transform);
            _enemy.GetComponent<RobotMovement>().ChangeGridBerdiri(_gridManager.GetGridByLocation(enemyLoc));
        }
    }
}
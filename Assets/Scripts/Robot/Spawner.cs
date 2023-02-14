using System.Collections;
using Grid;
using UnityEngine;

namespace Robot
{
    public class Spawner : MonoBehaviour
    {
        public static bool doneSpawn;
        
        public GameObject robotPrefab;

        public Vector2 spawnCoord;

        private GameObject _robot;
        private void Awake()
        {
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
            _robot.GetComponent<RobotMovement>().gridBerdiri = GridManager.instance.GetGridByLocation(loc);

            doneSpawn = true;
        }
    }
}
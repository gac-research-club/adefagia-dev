using System.Collections;
using Playground.Grid;
using UnityEngine;

namespace Playground
{
    public class Spawner : MonoBehaviour
    {
        public GameObject robotPrefab;

        public static bool doneSpawn;

        private GameObject Robot;
        private void Awake()
        {
            StartCoroutine(SpawnRobot(new Vector2(5,0)));
        }

        IEnumerator SpawnRobot(Vector2 loc)
        {
            while (!GridManager.doneGenerateNodes)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1);
            Robot = Instantiate(robotPrefab, transform);
            Robot.GetComponent<RobotMovement>().gridBerdiri = GridManager.GetNode(loc);

            doneSpawn = true;
        }
    }
}

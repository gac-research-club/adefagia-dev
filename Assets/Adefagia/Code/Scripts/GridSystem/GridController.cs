using System;
using Adefagia.ObstacleSystem;
using UnityEngine;

using Adefagia.RobotSystem;
using Object = UnityEngine.Object;

namespace Adefagia.GridSystem
{
    public class GridController : MonoBehaviour
    {
        public Grid Grid { get; set; }

        public RobotController RobotController { get; set; }
    }
}
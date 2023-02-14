using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace Playground.Grid
{
    public class GridManager : MonoBehaviour
    {
        public int xSize, ySize;
        public int index;
        public Vector2 nodeLoc;

        public GameObject emptyPrefab;
    
        public static bool doneGenerateNodes;

        // Set of nodes
        private static Dictionary<Vector2, Grid> allNodes;

        private void Awake()
        {
            // StartCoroutine(Generate());
            Generate();
        
            // Debug.Log(all_nodes.Length);
        }

        void Generate()
        {
            // WaitForSeconds wait = new WaitForSeconds(0.01f);

            // Set of nodes
            allNodes = new Dictionary<Vector2, Grid>();
        
            for (int i = 0, y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++, i++)
                {
                    var location = new Vector2(x, y);

                    // Add node
                    Grid grid = new Grid();
                    grid.index = i;
                    grid.location = location;
                
                    allNodes.Add(location, grid);

                    // yield return wait;
                }
            }

            Neighboors();
        
            SetGameObject();
        
            ShowGameObject();
        
            doneGenerateNodes = true;
        }

        void Neighboors()
        {
            for (int i = 0, y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++, i++)
                {
                    var location = new Vector2(x, y);

                    // Add neighbor
                    if (allNodes.TryGetValue(location, out Grid node))
                    {
                        node.neighboors = new Grid[4];
                        Vector2[] dirs = { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
                        for (int ni=0; ni<4; ni++)
                        {
                            node.neighboors[ni] = allNodes.TryGetValue(location + dirs[ni], out Grid neighboor)
                                ? neighboor
                                : null;
                        }
                    }
                }
            }
        }

        void SetGameObject()
        {
            StreamReader reader = new StreamReader("Assets/Resources/Map.txt");
            string map = reader.ReadToEnd();

            map = CleanInput(map);
            // Debug.Log(map);
        
            // return;

            for (int i = 0, y = ySize-1; y >= 0; y--)
            {
                for (int x = 0; x < xSize; x++, i++)
                {
                    switch (map[i])
                    {
                        case '-':
                            allNodes[new Vector2(x, y)].state = State.Empty;
                            break;
                    
                        case 'o':
                            allNodes[new Vector2(x, y)].state = State.Ground;
                            break;
                    }
                }
            }
        }
    
        static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try {
                return Regex.Replace(strIn, @"[^\w\.@-]", "",
                    RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException) {
                return String.Empty;
            }
        }

        void ShowGameObject()
        {
            foreach (var node in allNodes)
            {
                switch (node.Value.state)
                {
                    case State.Ground:
                        var cube = Instantiate(emptyPrefab, ToVec3(node.Value.location), Quaternion.identity, transform);
                        cube.name = "Cube " + node.Value.index;
                        break;
                }
            }
        }

        public static Grid GetNode(Vector2 loc)
        {
            return allNodes.TryGetValue(loc, out Grid node) ? node : null;
        }

        Vector3 ToVec3(Vector2 loc)
        {
            return new Vector3(loc.x, 0, loc.y);
        }

        private void OnDrawGizmos()
        {
            if (allNodes.IsUnityNull()) return;
    
            foreach (var node in allNodes)
            {
                if (!node.Value.IsUnityNull())
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(ToVec3(node.Value.location), 0.1f);
                
                    Gizmos.color = Color.blue;
                    foreach (var neighboor in node.Value.neighboors)
                    {
                        if (!neighboor.IsUnityNull())
                        {
                            Gizmos.DrawLine(ToVec3(neighboor.location), ToVec3(node.Value.location));
                        }
                    }
                
                }
            }
        }
    
    }
}


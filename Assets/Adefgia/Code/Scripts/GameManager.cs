using System;
using System.Collections;
using System.Collections.Generic;
using adefagia.Collections;
using adefagia.Graph;
using adefagia.Robot;
using Unity.VisualScripting;
using UnityEngine;

namespace adefagia
{
    public class GameManager : MonoBehaviour
    {
        // Singleton
        public static GameManager instance;

        // All Manager
        public BattleManager battleManager;
        public GridManager gridManager;
        public RobotManager robotManager;
        public UIManager uiManager;

        private void Awake()
        {
            Singleton();
        }

        private void Singleton()
        {
            // Ensure only one Instance is in the hierarchy
            // If there is no instance then do Instantiation
            if (instance.IsUnityNull())
            {
                instance = this;
            }
            // If already instantiation, destroy new duplicate singleton
            // to make sure only one
            else
            {
                Destroy(this);
            }
            
            // Can access from any scene
            DontDestroyOnLoad(instance);
        }
        
        
    }
}

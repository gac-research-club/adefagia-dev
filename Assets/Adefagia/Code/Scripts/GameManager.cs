using Unity.VisualScripting;
using UnityEngine;

using Adefagia.BattleMechanism;
using Adefagia.GridSystem;
using Adefagia.RobotSystem;

namespace Adefagia
{
    public class GameManager : MonoBehaviour
    {
        // Singleton
        public static GameManager instance;

        // All Manager
        public BattleManager battleManager;
        public GridManager gridManager;
        public UIManager uiManager;

        [Header("For testing only")] 
        public BattleManagerTest BattleManagerTest;

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

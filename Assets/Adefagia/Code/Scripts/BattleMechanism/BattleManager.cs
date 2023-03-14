using System;
using System.Collections;
using adefagia.Adefgia.Code.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Adefagia.RobotSystem;
using Adefagia.PlayerAction;

namespace Adefagia.BattleMechanism
{
    public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}
    public class BattleManager : MonoBehaviour
    {
        public BattleState state;
        [SerializeField] RobotStatus playerUnit;
        [SerializeField] RobotStatus enemyUnit;

        private Transform highlight;
        private Transform selection;
        private RaycastHit raycastHit;
        // [SerializeField] private GameInput gameInput;
        [SerializeField] private Color color;
        
        [SerializeField] private MoveAction moveAction;
        [SerializeField] private AttackHighlight attackHighlight;
        [SerializeField] private GameObject actionButton;
        [SerializeField] private Button attackButton;
        [SerializeField] private Button defendButton;
        
        [SerializeField] private GameInput gameInput;

        private void Start()
        {
            
            gameInput.OnInteractAction += GameInput_OnInteractAction;
           
            // foreach (var item in spawner._playerUnit.Values)
            // {
            //     playerUnit = item.GetComponent<RobotStats>();
            // }
            StartCoroutine(SetupBattle());

        }

        void Update()
        {
            Highlight();
        }

        /*--------------------------------------------------------------------------
        * Kondisi di saat highlighted robot telah di click 
        *--------------------------------------------------------------------------*/
        private void GameInput_OnInteractAction(object sender, System.EventArgs e)
        {
            
            if (highlight)
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                selection = raycastHit.transform;
                selection.gameObject.GetComponent<Outline>().enabled = true;
                highlight = null;
                
                // Saat highlighted robot di click maka set active action button hud
                actionButton.SetActive(true);
            } else {
                if (!EventSystem.current.IsPointerOverGameObject() && selection)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                    
                    // Saat highlighted robot on clicked, lalu click apapun kecuali robot maka disable action button hud 
                    actionButton.SetActive(false);

                    // Saat highlighted robot on clicked, lalu click apapun kecuali robot maka disable movement pattern dan attack pattern
                    moveAction.MoveButtonOnDisable();
                    attackHighlight.AttackButtonOnDisable();
                }
            }
        }
    
        IEnumerator SetupBattle()
        {
            yield return new WaitForSeconds(0f);
            
            state = BattleState.PLAYERTURN;
            PlayerTurnMessage();
        }

        void PlayerTurnMessage()
        {
            Debug.Log("Player turn");
        }

        void EnemyTurnMessage()
        {
            Debug.Log("Enemy turn");
        }

        public void AttackButtonOnClicked()
        {
            // bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
            attackButton.interactable = false;
            // if (isDead)
            // {
            //     state = BattleState.WON;
            //     EndBattle();
            // }
        }

        public void DefendButtonOnClicked()
        {
            // playerUnit.Heal(playerUnit.healAmount);
            defendButton.interactable = false;
        }

        void EndBattle()
        {
            if(state == BattleState.WON)
            {
                Debug.Log("You Won the battle!");
            } 
            else if(state == BattleState.LOST)
            {
                Debug.Log("You were defeated.");
            }
        }

        public void EndTurnButtonOnClicked()
        {
            if(state == BattleState.PLAYERTURN)
            {
                state = BattleState.ENEMYTURN;
                EnemyTurnMessage();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurnMessage();
            }
        }

        /*--------------------------------------------------------------------------
        * Kondisi saat pointer berada di robot
         * untuk menghighlight robot
        *--------------------------------------------------------------------------*/
        private void Highlight()
        {
            // Disable outline saat highlight != null
            if (highlight != null)
            {
                highlight.gameObject.GetComponent<Outline>().enabled = false;
                highlight = null;
            }
            
            /*--------------------------------------------------------------------------
            * Saat pointer berada di game object dengan tag tertentu maka aktifkan outline
            *--------------------------------------------------------------------------*/
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
            {
                highlight = raycastHit.transform;
                // 
                if (highlight.CompareTag("Selectable") && highlight != selection && state == BattleState.PLAYERTURN)
                {
                    if (highlight.gameObject.GetComponent<Outline>() != null)
                    {
                        highlight.gameObject.GetComponent<Outline>().enabled = true;
                    }
                    else
                    {
                        Outline outline = highlight.gameObject.AddComponent<Outline>();
                        outline.enabled = true;
                        highlight.gameObject.GetComponent<Outline>().OutlineColor = color;
                        highlight.gameObject.GetComponent<Outline>().OutlineWidth = 8.0f;
                    }
                }
                else if((highlight.CompareTag("EnemySelectable") && highlight != selection && state == BattleState.ENEMYTURN))
                {
                    if (highlight.gameObject.GetComponent<Outline>() != null)
                    {
                        highlight.gameObject.GetComponent<Outline>().enabled = true;
                    }
                    else
                    {
                        Outline outline = highlight.gameObject.AddComponent<Outline>();
                        outline.enabled = true;
                        highlight.gameObject.GetComponent<Outline>().OutlineColor = color;
                        highlight.gameObject.GetComponent<Outline>().OutlineWidth = 8.0f;
                    }
                }
                else
                {
                    highlight = null;
                }
            }
        }
    }
}


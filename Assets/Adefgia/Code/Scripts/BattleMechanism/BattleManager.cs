using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

namespace adefagia
{
    public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}
    public class BattleManager : MonoBehaviour
    {
        public BattleState state;
        [SerializeField] RobotStats playerUnit;
        [SerializeField] RobotStats enemyUnit;

        private Transform highlight;
        private Transform selection;
        private RaycastHit raycastHit;
        [SerializeField] private GameInput gameInput;
        [SerializeField] private Color color;
        
        [SerializeField] private PlayerAction.MoveAction moveAction;
        [SerializeField] private PlayerAction.AttackHighlight attackHighlight;
        [SerializeField] private GameObject actionButton;
        [SerializeField] private Button attackButton;
        [SerializeField] private Button defendButton;

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

        private void GameInput_OnInteractAction(object sender, System.EventArgs e)
        {
            // Selection
            if (highlight)
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                selection = raycastHit.transform;
                selection.gameObject.GetComponent<Outline>().enabled = true;
                highlight = null;
                actionButton.SetActive(true);

            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject() && selection)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                    actionButton.SetActive(false);

                    moveAction.MoveButtonOnDisable();
                    attackHighlight.AttackButtonOnDisable();
                }
            }
        }

        IEnumerator SetupBattle()
        {
            yield return new WaitForSeconds(0f);
            
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

        void PlayerTurn()
        {
            Debug.Log("Player turn");
        }

        void EnemyTurn()
        {
            Debug.Log("Enemy turn");
        }

        public void AttackButtonOnClicked()
        {
            bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
            attackButton.interactable = false;
            if (isDead)
            {
                state = BattleState.WON;
                EndBattle();
            }
        }

        public void DefendButtonOnClicked()
        {
            playerUnit.Heal(playerUnit.healAmount);
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
                EnemyTurn();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }

        private void Highlight()
        {
            // Highlight
            if (highlight != null)
            {
                highlight.gameObject.GetComponent<Outline>().enabled = false;
                highlight = null;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
            {
                highlight = raycastHit.transform;
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


using UnityEngine;
using UnityEngine.UI;

namespace Adefagia.Experimentation.dimaspadma
{
    public class UIBattleController : MonoBehaviour
    {
        [SerializeField] private Button buttonMove;
        [SerializeField] private Button buttonAttack;
        [SerializeField] private Button buttonDefend;
        
        // TODO: disable button Move
        public void DisableButtonMove()
        {
            DisableButton(buttonMove);
        }
        
        /*-------------------------------------------------
         * Disable button
         *-------------------------------------------------*/
        public void DisableButton(Button button)
        {
            button.enabled = false;
        }
        
    }
}
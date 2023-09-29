using UnityEngine;

namespace Michsky.UI.Shift
{
    public class SwitchHandler : MonoBehaviour
    {
        [SerializeField] private SwitchManager switchSource;

        void Awake()
        {
            if (switchSource == null)
            {
                try { switchSource = gameObject.GetComponent<SwitchManager>(); }
                catch { Debug.Log("Switch Source is not assigned.", this); }
            }
        }

        public void SetOn()
        {
            switchSource.isOn = true;
            switchSource.switchAnimator.Play("Switch On");
            switchSource.OnEvents.Invoke();
        }

        public void SetOff()
        {
            switchSource.isOn = false;
            switchSource.switchAnimator.Play("Switch Off");
            switchSource.OffEvents.Invoke();
        }
    }
}
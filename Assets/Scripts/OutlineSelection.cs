using UnityEngine.EventSystems;
using UnityEngine;

namespace adefagia
{
    public class OutlineSelection : MonoBehaviour
    {
        private Transform highlight;
        private Transform selection;
        private RaycastHit raycastHit;

        [SerializeField] private GameInput gameInput;
        [SerializeField] private GameObject actionButton;
        [SerializeField] private Color color;

        private void Start()
        {
            gameInput.OnInteractAction += GameInput_OnInteractAction;
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
                if (selection)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                    actionButton.SetActive(false);
                }
            }
        }

        void Update()
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
                if (highlight.CompareTag("Selectable") && highlight != selection)
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

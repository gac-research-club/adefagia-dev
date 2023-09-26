using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.Shift
{
    public class SettingsButton : MonoBehaviour, IPointerEnterHandler
    {
        [Header("Resources")]
        public Image detailImage;
        public Image detailIcon;
        public Image detailBackground;
        public TextMeshProUGUI detailTitle;
        public TextMeshProUGUI detailDescription;
        public TextMeshProUGUI buttonTitleObj;

        [Header("Content")]
        public bool useCustomContent;
        public string buttonTitle;

        [Header("Preview")]
        public bool enableIconPreview;
        public string title;
        [TextArea] public string description;
        public Sprite imageSprite;
        public Sprite iconSprite;
        public Sprite iconBackground;

        void Start()
        {
            if (useCustomContent == false) { buttonTitleObj.text = buttonTitle; }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableIconPreview == true)
            {
                detailImage.gameObject.SetActive(false);
                detailIcon.gameObject.SetActive(true);
                detailBackground.gameObject.SetActive(true);
                detailIcon.sprite = iconSprite;
                detailBackground.sprite = iconBackground;
            }

            else
            {
                detailImage.gameObject.SetActive(true);
                detailIcon.gameObject.SetActive(false);
                detailBackground.gameObject.SetActive(false);
                detailImage.sprite = imageSprite;
            }

            detailTitle.text = title;
            detailDescription.text = description;
        }
    }
}
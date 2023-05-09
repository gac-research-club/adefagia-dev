using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using adefagia.CharacterStats;

public class StatDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CharacterStat _stat;

    //To update stat value on character stat
    public CharacterStat Stat
    {
        get { return _stat; }
        set
        {
            _stat = value;
            UpdateStatValue();
        }
    }

    private string _name;
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            nameText.text = _name.ToLower();
        }
    }

    [SerializeField] Text nameText;
    [SerializeField] Text valueText;
    [SerializeField] StatTooltip tooltip;

    private bool showingTooltip;


    private void OnValidate()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        nameText = texts[0];
        valueText = texts[1];

        if (tooltip == null)
            tooltip = FindObjectOfType<StatTooltip>();
    }

    //To show tooltip
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(Stat, Name);
        showingTooltip = true;
    }

    //To hide tooltip
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
        showingTooltip = false;
    }

    //To update stat value
    public void UpdateStatValue()
    {
        valueText.text = _stat.Value.ToString();
        if (showingTooltip)
        {
            tooltip.ShowTooltip(Stat, Name);
        }
    }
}



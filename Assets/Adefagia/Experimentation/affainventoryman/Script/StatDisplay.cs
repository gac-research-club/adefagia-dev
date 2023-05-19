using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using adefagia.CharacterStats;

public class StatDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const string DataKey = "GameData";
    private CharacterStat _stat;
    public CharacterStat Stat {
        get { return _stat; }
        set {
            _stat = value;
            UpdateStatValue();
        }
    }

    public string robotSelect = "0";

    private string _name;
    public string Name {
        get { return _name; }
        set {
            _name = value;
            nameText.text = _name.ToLower();
        }
    }

    [SerializeField] Text nameText;
    [SerializeField] Text valueText;
    [SerializeField] StatTooltip tooltip;

    public void setRobotSelect(string idRobot){
        robotSelect = idRobot;
    }

    private void OnValidate()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        nameText = texts[0];
        valueText = texts[1];

        if (tooltip == null)
            tooltip = FindObjectOfType<StatTooltip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(Stat, Name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    public void UpdateStatValue()
    {
        valueText.text = _stat.Value.ToString();
        
        Dictionary<string, string> data = new Dictionary<string, string>();
            
        data.Add("robot", robotSelect);
        data.Add("type", _name);
        data.Add("value", _stat.Value.ToString());
        
        SaveData(data);
    }

    // Menyimpan data dalam PlayerPrefs
    public static void SaveData(Dictionary<string, string> data)
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(DataKey, jsonData);
    }
}

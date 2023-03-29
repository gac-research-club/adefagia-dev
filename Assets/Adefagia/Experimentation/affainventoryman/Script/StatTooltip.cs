using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using adefagia.CharacterStats;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] Text StatNameText;
    [SerializeField] Text StatModifierLabelText;
    [SerializeField] Text StatModifierText;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip(CharacterStat stat, string statName)
    {
        StatNameText.text = GetStatTopText(stat, statName);

        StatModifierText.text = GetStatModifiersText(stat);

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private string GetStatTopText(CharacterStat stat, string statName)
    {
        sb.Length = 0;
        sb.Append(statName);
        sb.Append(" ");
        sb.Append(stat.Value);

        if (stat.Value != stat.BaseValue)
        {
            sb.Append(" (");
            sb.Append(stat.BaseValue);

            if (stat.Value > stat.BaseValue)
                sb.Append("+");

            sb.Append(System.Math.Round(stat.Value - stat.BaseValue, 4));
            sb.Append(")");
        }

        return sb.ToString();
    }

    private string GetStatModifiersText(CharacterStat stat)
    {
        sb.Length = 0;

        foreach (StatModifier mod in stat.StatModifiers)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (mod.Value > 0)
                sb.Append("+");

            if (mod.Type == StatModType.Flat)
            {
                sb.Append(mod.Value);
            }
            else
            {
                sb.Append(mod.Value * 100);
                sb.Append("%");
            }

            EquippableItem item = mod.Source as EquippableItem;

            if (item != null)
            {
                sb.Append(" ");
                sb.Append(item.ItemName);
            }
            else
            {
                Debug.LogError("Modifier is null");
            }
        }

        return sb.ToString();
    }
}

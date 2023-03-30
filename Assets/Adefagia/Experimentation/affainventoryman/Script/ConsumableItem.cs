using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    Healing,
    Mana,

}

[CreateAssetMenu]

public class ConsumableItem : Item
{
    public int AddedHealth;
    public int AddedMana;
}

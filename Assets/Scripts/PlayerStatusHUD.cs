using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusHUD : MonoBehaviour
{
    BattleSystem battleSystem;
    PlayerRobot playerRobot;
    public Text robotName;
    public Text robotLevel;
    public Text robotDamage;
    public Text robotCurrentHP;

    void Update()
    {
        
    }

    public void setHUD(PlayerRobot playerRobot)
    {
        robotName.text = "Name : " + playerRobot.robotName;
        robotLevel.text = "Level : " + playerRobot.robotLevel;
        robotDamage.text = "Damage : " + playerRobot.damage;
        robotCurrentHP.text = "HP : " + playerRobot.currentHP;
    }
}
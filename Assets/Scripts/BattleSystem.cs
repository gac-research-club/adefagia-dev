using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private GameObject robotPrefab1;
    [SerializeField] private GameObject robotPrefab2;
    [SerializeField] private GameObject robotPrefab3;

    [SerializeField] private Transform robotStation1;
    [SerializeField] private Transform robotStation2;
    [SerializeField] private Transform robotStation3;

    [SerializeField] private PlayerStatusHUD playerStatusHUD1;
    [SerializeField] private PlayerStatusHUD playerStatusHUD2;
    [SerializeField] private PlayerStatusHUD playerStatusHUD3;

    PlayerRobot playerRobot1;
    PlayerRobot playerRobot2;
    PlayerRobot playerRobot3;

    public BattleState state;
    

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    public void SetupBattle()
    {
        GameObject robot1 = Instantiate(robotPrefab1,robotStation1);
        playerRobot1 = robot1.GetComponent<PlayerRobot>();

        playerStatusHUD1.setHUD(playerRobot1);
        
        GameObject robot2 = Instantiate(robotPrefab2,robotStation2);
        playerRobot2 = robot2.GetComponent<PlayerRobot>();

        playerStatusHUD2.setHUD(playerRobot2);

        GameObject robot3 = Instantiate(robotPrefab3,robotStation3);
        playerRobot3 = robot3.GetComponent<PlayerRobot>();

        playerStatusHUD3.setHUD(playerRobot3);
    }
}
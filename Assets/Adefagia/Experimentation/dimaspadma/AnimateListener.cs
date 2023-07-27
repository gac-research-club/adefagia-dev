using System.Collections;
using System.Collections.Generic;
using Adefagia.PlayerAction;
using Adefagia.RobotSystem;
using UnityEngine;

public class AnimateListener : MonoBehaviour
{

    private Animator _animator;
    
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Turn = Animator.StringToHash("Turn");

    void Start()
    {
        _animator = GetComponent<Animator>();
        RobotMovement.MoveAnimation += OnMoving;
        RobotController.TurnAnimation += OnTurn;
        RobotController.MoveAnimation += OnMoving;
    }

    private void OnMoving(bool isMove)
    {
        Debug.Log("Move Animation");
        _animator.SetBool(Move, isMove);
    }

    private void OnTurn(bool isTurn)
    {
        _animator.SetBool(Turn, isTurn);
    }
}

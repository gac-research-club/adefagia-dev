    using System.Collections;
using System.Collections.Generic;
using Adefagia;
using Adefagia.BattleMechanism;
using Adefagia.PlayerAction;
using Adefagia.RobotSystem;
using UnityEngine;

public class AnimateListener : MonoBehaviour
{

    private Animator _animator;
    
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Turn = Animator.StringToHash("Turn");

    private int _instanceId;

    void Start()
    {
        _instanceId = gameObject.GetInstanceID();
        _animator = GetComponent<Animator>();
        
        RobotController.TurnAnimation += OnTurn;
        RobotController.MoveAnimation += OnMoving;
    }

    private void OnMoving(int id, bool isMove)
    {
        if(id != _instanceId) return;
        
        _animator.SetBool(Move, isMove);
    }

    private void OnTurn(int id, bool isTurn)
    {
        if(id != _instanceId) return;
        
        _animator.SetBool(Turn, isTurn);
    }

}

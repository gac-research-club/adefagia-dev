using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScene : MonoBehaviour
{
    public static UnityAction<int> ButtonActionLoad;
    public static UnityAction<int> ButtonActionUnLoad;

    public void LoadSceneIndex(int id)
    {
        ButtonActionLoad?.Invoke(id);
    }
    
    public void UnLoadSceneIndex(int id)
    {
        ButtonActionUnLoad?.Invoke(id);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EmotScript : MonoBehaviour
{
    Image _image;
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
    }

    public void ShowEmot(Sprite spr = null)
    {
        if (spr != null) _image.sprite = spr;
        _animator.Play("EmotAnimation", 0, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ExceptionHandling : MonoBehaviour {
    public string text;
    public TMP_Text Text;
    public GameObject errorObj;

    void Start() {
        StartCoroutine(CloseDelay());
    }

    public void Dongo(string text) {
        Text.text = text;
    }

    IEnumerator CloseDelay() {
        yield return new WaitForSeconds(2);
        errorObj.SetActive(false);
    }
}

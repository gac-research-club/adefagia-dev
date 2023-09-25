using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class ExceptionHandling : MonoBehaviour
{
	public string text;
	public TMP_Text Text;
	public float time = 0.1f;
	public float shakeRange = 20f;

	void Awake()
	{
		
	}

	public void Dongo(string text)
	{
		Text.text = text;
	}

	public IEnumerator CloseDelay()
	{
		StartCoroutine(Shake());
		yield return new WaitForSeconds(2);
		this.gameObject.SetActive(false);

	}

	IEnumerator Shake()
	{
		float elapsed = 0.0f;
		Quaternion originalRotation = this.gameObject.transform.rotation;
		while (elapsed < time)
		{
			float z = Random.value * shakeRange - (shakeRange / 2);
			this.gameObject.transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y, originalRotation.z + z);
			yield return null;
		}
		this.gameObject.transform.rotation = originalRotation;
	}
}
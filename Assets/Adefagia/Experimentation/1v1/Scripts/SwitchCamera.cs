using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour
{
	[SerializeField] GameObject cam1;
	[SerializeField] GameObject cam2;
	Button _btn;
	void Awake()
	{
		_btn = GetComponent<Button>();
		_btn.onClick.AddListener(delegate
		{
			if (cam1.activeSelf)
			{
				cam1.SetActive(false);
				cam2.SetActive(true);
			}
			else
			{
				cam1.SetActive(true);
				cam2.SetActive(false);
			}
		});
	}
}

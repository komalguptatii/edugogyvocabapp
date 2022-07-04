using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Popup : MonoBehaviour
{

	[SerializeField] Button _button1;
	[SerializeField] Button _button2;
	[SerializeField] TextMeshProUGUI _button1Text;
	[SerializeField] TextMeshProUGUI _button2Text;
	[SerializeField] TextMeshProUGUI _popupText;


	public void Init(Transform canvas, string popupMessage, string btn1txt, string btn2txt, Action action) {
		_popupText.text = popupMessage;
		_button1Text.text = btn1txt;
		_button2Text.text = btn2txt;

		transform.SetParent(canvas);
		transform.localScale = Vector3.one;
		GetComponent<RectTransform>().offsetMin = Vector2.zero;
		GetComponent<RectTransform>().offsetMax = Vector2.zero;

		_button1.onClick.AddListener(() => {
			GameObject.Destroy(this.gameObject);
		});

		_button2.onClick.AddListener(() => {
			action();
			GameObject.Destroy(this.gameObject);
		});

	}
}
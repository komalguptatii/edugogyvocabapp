using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class InteractivePopUp : MonoBehaviour
{

	[SerializeField] Button _button1;
	
	[SerializeField] TextMeshProUGUI _button1Text;
	[SerializeField] TextMeshProUGUI _popupText;


	public void Init(Transform canvas, string popupMessage, string btn1txt) {
		_popupText.text = popupMessage;
		_button1Text.text = btn1txt;

		transform.SetParent(canvas);
		transform.localScale = Vector3.one;
		GetComponent<RectTransform>().offsetMin = Vector2.zero;
		GetComponent<RectTransform>().offsetMax = Vector2.zero;

		_button1.onClick.AddListener(() => {
			GameObject.Destroy(this.gameObject);
		});

		

	}
}
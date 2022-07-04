using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

	public static UIController Instance;

	public Transform MainCanvas;

    // Start is called before the first frame update
    void Start()
    {
		if (Instance != null) {
			GameObject.Destroy(this.gameObject);
			return;
		}

		Instance = this;
    }

	public Popup CreatePopup() {
		GameObject popUpGo = Instantiate(Resources.Load("UI/Popup") as GameObject);
		return popUpGo.GetComponent<Popup>();
	}

    public InteractivePopUp CreateInteractivePopup() {
		GameObject popUpGo = Instantiate(Resources.Load("UI/InteractivePopUp") as GameObject);
		return popUpGo.GetComponent<InteractivePopUp>();
	}
}

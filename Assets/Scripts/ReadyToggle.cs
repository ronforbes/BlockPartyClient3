using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyToggle : MonoBehaviour {
	Toggle toggle;

	void Awake() {
		toggle = GetComponent<Toggle>();
	}

	void Start () {
		toggle.isOn = Player.Instance.Ready;
	}
	
	public void SetReady(bool ready) {
		Player.Instance.Ready = ready;
	}
}

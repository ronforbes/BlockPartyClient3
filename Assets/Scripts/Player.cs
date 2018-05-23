using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangedEventArgs : EventArgs {}

public class Player : MonoBehaviour {
	static Player instance;
	public static Player Instance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<Player>();

				if(instance != null) {
					DontDestroyOnLoad(instance.gameObject);
				}
			}

			return instance;
		}
	}

	[SerializeField]
	bool ready;
	public event EventHandler<PlayerChangedEventArgs> OnPlayerChanged = (sender, e) => {};
	public bool Ready {
		get { return ready; }
		set { 
			if(ready != value) {
				ready = value;
				OnPlayerChanged(this, new PlayerChangedEventArgs());
			}
		}
	}

	public bool PlayedLastGame;

	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(this);
		}
		else {
			if(this != instance) {
				Destroy(this.gameObject);
			}
		}
	}

	void Start () {
		ready = false;
	}
}

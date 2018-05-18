using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockChangedEventArgs : EventArgs {}

public class Clock : MonoBehaviour {
	static Clock instance;
	public static Clock Instance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<Clock>();
				DontDestroyOnLoad(instance.gameObject);
			}

			return instance;
		}
	}

	float timeRemaining;
	public event EventHandler<ClockChangedEventArgs> OnClockChanged = (sender, e) => {};
	public float TimeRemaining {
		get { return timeRemaining; }
		set {
			if(timeRemaining != value) {
				timeRemaining = value;
				OnClockChanged(this, new ClockChangedEventArgs());
			}
		}
	}

	const float gameDuration = 120.0f;

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

	// Use this for initialization
	void Start () {
		StartGame();
	}

	public void StartGame() {
		TimeRemaining = gameDuration;
	}
	
	// Update is called once per frame
	void Update () {
		TimeRemaining -= Time.deltaTime;
	}
}

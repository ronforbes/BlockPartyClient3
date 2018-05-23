using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	string state;
	public string State {
		get { return state; }
		set {
			if(state != value) {
				state = value;
				OnClockChanged(this, new ClockChangedEventArgs());
			}
		}
	}

	float duration;
	public float Duration {
		get { return duration; }
		set {
			if(duration != value) {
				duration = value;
				OnClockChanged(this, new ClockChangedEventArgs());
			}
		}
	}

	float elapsed;
	public float Elapsed {
		get { return elapsed; }
		set {
			if(elapsed != value) {
				elapsed = value;
				OnClockChanged(this, new ClockChangedEventArgs());
			}
		}
	}

	Dictionary<string, float> stateDurations;

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

		stateDurations = new Dictionary<string, float>();
		stateDurations.Add("inGame", 120.0f);
		stateDurations.Add("outOfGame", 45.0f);
	}

	// Use this for initialization
	void Start () {
		state = "inGame";
		elapsed = 0.0f;
		duration = stateDurations[state];
	}

	public void SetState(string state, float duration, float elapsed) {
		this.state = state;
		this.duration = duration;
		this.elapsed = elapsed;
		OnClockChanged(this, new ClockChangedEventArgs());
	}
	
	// Update is called once per frame
	void Update () {
		this.elapsed += Time.deltaTime;

		TimeRemaining = this.duration - this.elapsed;

		if(this.elapsed >= this.duration) {
			switch(this.state) {
				case "inGame":
					state = "outOfGame";
					if(Player.Instance.PlayedLastGame) {
						NetworkManager.Instance.SendPlayerScore(Score.Instance.Points);
						if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game")) {
							SceneManager.LoadScene("Results");
						}
						Player.Instance.PlayedLastGame = false;
					}
					Results.Instance.ClearEntries();
					Chat.Instance.AddMessage("System", "The game has ended");
					break;

				case "outOfGame":
					state = "inGame";
					if(Player.Instance.Ready) {
						Player.Instance.PlayedLastGame = true;
						Score.Instance.Reset();
						SceneManager.LoadScene("Game");
					}
					Chat.Instance.AddMessage("System", "The game has started");
					break;
			}

			duration = stateDurations[state];
			elapsed = 0.0f;
		}
	}
}

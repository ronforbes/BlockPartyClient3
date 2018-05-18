using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChangedEventArgs : EventArgs {}
public class Scoreboard : MonoBehaviour {
	static Scoreboard instance;
	public static Scoreboard Instance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<Scoreboard>();

				if(instance != null) {
					DontDestroyOnLoad(instance.gameObject);
				}
			}

			return instance;
		}
	}

	int score;
	public event EventHandler<ScoreChangedEventArgs> OnScoreChanged = (sender, e) => {};
	public int Score {
		get { return score; }
		set {
			if(score != value) {
				score = value;
				OnScoreChanged(this, new ScoreChangedEventArgs());
			}
		}
	}

	const int matchValue = 10;

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
		Reset();
	}
	
	public void Reset() {
		Score = 0;
	}

	public void ScoreMatch() {
		Score += matchValue;
	}

	// Update is called once per frame
	void Update () {
		
	}
}

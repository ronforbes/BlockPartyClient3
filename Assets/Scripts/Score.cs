using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreChangedEventArgs : EventArgs {}
public class Score : MonoBehaviour {
	static Score instance;
	public static Score Instance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<Score>();

				if(instance != null) {
					DontDestroyOnLoad(instance.gameObject);
				}
			}

			return instance;
		}
	}

	int points;
	public event EventHandler<ScoreChangedEventArgs> OnScoreChanged = (sender, e) => {};
	public int Points {
		get { return points; }
		set {
			if(points != value) {
				points = value;
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
		points = 0;
		OnScoreChanged(this, new ScoreChangedEventArgs());
	}

	public void ScoreMatch() {
		Points += matchValue;
	}
}

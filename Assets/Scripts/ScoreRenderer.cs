using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRenderer : MonoBehaviour {
	Text text;

	void Awake() {
		text = GetComponent<Text>();
		Score.Instance.OnScoreChanged += HandleScoreChanged;
	}

	void HandleScoreChanged(object sender, ScoreChangedEventArgs e) {
		text.text = Score.Instance.Points.ToString();
	}
}

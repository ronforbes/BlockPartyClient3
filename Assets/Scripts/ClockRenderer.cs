using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockRenderer : MonoBehaviour {
	Text text;

	void Awake() {
		text = GetComponent<Text>();
		Clock.Instance.OnClockChanged += HandleClockChanged;
	}

	void HandleClockChanged(object sender, ClockChangedEventArgs e) {
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)Clock.Instance.TimeRemaining);
		
		text.text = String.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
	}
}

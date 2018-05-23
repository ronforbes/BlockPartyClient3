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
		string stateString = "";
		switch(Clock.Instance.State) {
			case "inGame":
				stateString = "Game in progress";
				break;
			case "outOfGame":
				stateString = "Setting up next game";
				break;
		}

		if(text != null) {
			text.text = String.Format("<b>{0}</b>\n\n<b>Time remaining:</b>\n{1:0}:{2:00}", stateString, timeSpan.Minutes, timeSpan.Seconds);
		}
	}
}

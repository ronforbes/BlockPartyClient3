using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessageSender : MonoBehaviour {
	InputField inputField;
	
	void Awake() {
		inputField = GameObject.Find("Message InputField").GetComponent<InputField>();
	}

	public void SendMessage() {
		string text = inputField.text;

		if(text == "request state") {
			NetworkManager.Instance.SendRequestState();
			inputField.text = "";
			return;
		}

		if(text.StartsWith("name=")) {
			string name = text.Substring(text.IndexOf("=") + 1);
			NetworkManager.Instance.SendRename(name);
			inputField.text = "";
			return;
		}

		NetworkManager.Instance.SendChatMessage(text);
		inputField.text = "";
	}
}

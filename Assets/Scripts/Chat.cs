using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessage {
	public string Name;
	public string Message;

	public ChatMessage(string name, string message) {
		Name = name;
		Message = message;
	}
}

public class ChatChangedEventArgs : EventArgs {}

public class Chat : MonoBehaviour {
	static Chat instance;
	public static Chat Instance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<Chat>();
				DontDestroyOnLoad(instance.gameObject);
			}

			return instance;
		}
	}

	List<ChatMessage> messages;
	public event EventHandler<ChatChangedEventArgs> OnChatChanged = (sender, e) => {};
	public List<ChatMessage> Messages {
		get { return messages; }
		set {
			if(messages != value) {
				messages = value;
				OnChatChanged(this, new ChatChangedEventArgs());
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

		messages = new List<ChatMessage>();
	}

	void Start() {
		ClearMessages();
	}

	public void ClearMessages() {
		Messages.Clear();
		OnChatChanged(this, new ChatChangedEventArgs());
	}

	public void AddMessage(string name, string message) {
		messages.Add(new ChatMessage(name, message));
		OnChatChanged(this, new ChatChangedEventArgs());
	}
}

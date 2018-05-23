using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatRenderer : MonoBehaviour {
	public GameObject ChatMessagePrefab;
	List<GameObject> chatMessages;
	ScrollRect scrollRect;

	void Awake() {
		chatMessages = new List<GameObject>();
		scrollRect = GameObject.Find("Chat Messages Scroll View").GetComponent<ScrollRect>();

		Chat.Instance.OnChatChanged += HandleChatChanged;
	}

	void Start() {
		PopulateMessages();
	}

	void HandleChatChanged(object sender, ChatChangedEventArgs e) {
		PopulateMessages();
	}

	public void PopulateMessages() {
		foreach(GameObject message in chatMessages) {
			Destroy(message);
		}

		chatMessages.Clear();

		foreach(ChatMessage message in Chat.Instance.Messages) {
			GameObject chatMessage = Instantiate(ChatMessagePrefab, Vector3.zero, Quaternion.identity);
			chatMessage.transform.SetParent(transform);
			chatMessage.transform.localScale = Vector3.one;
			Text text = chatMessage.GetComponentInChildren<Text>();
			text.text = "<b>" + message.Name + "</b>: " + message.Message;
			chatMessages.Add(chatMessage);
		}

		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(0, 100 * Chat.Instance.Messages.Count);
		scrollRect.verticalNormalizedPosition = 0.0f;
	}

	void OnDestroy() {
		Chat.Instance.OnChatChanged -= HandleChatChanged;
	}
}

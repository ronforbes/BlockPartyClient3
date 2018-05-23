using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.SocketIO;
using SimpleJSON;

public class NetworkManager : MonoBehaviour {
	static NetworkManager instance;
	public static NetworkManager Instance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<NetworkManager>();
				
				if(instance != null) {
					DontDestroyOnLoad(instance.gameObject);
				}
			}
			
			return instance;
		}
	}

	SocketManager socketManager;

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

	void Start() {
		//socketManager = new SocketManager(new Uri("http://blockpartyserver.herokuapp.com/socket.io/"));
		socketManager = new SocketManager(new Uri("http://localhost:1337/socket.io/"));
		socketManager.Socket.On(SocketIOEventTypes.Connect, HandleServerConnect);
		socketManager.Socket.On("request state", HandleRequestState);
		socketManager.Socket.On("player connected", HandlePlayerConnected);
		socketManager.Socket.On("chat message", HandleChatMessage);
		socketManager.Socket.On("rename player", HandleRenamePlayer);
		socketManager.Socket.On("game results", HandleGameResults);
		socketManager.Socket.On("player disconnected", HandlePlayerDisconnected);
	}

	void HandleServerConnect(Socket socket, Packet packet, params object[] args)
	{
		SendRequestState();
	}

	void HandleRequestState(Socket socket, Packet packet, params object[] args) {
		string state = JSON.Parse(packet.ToString())[1]["state"];
		int duration = JSON.Parse(packet.ToString())[1]["duration"];
		int elapsed = JSON.Parse(packet.ToString())[1]["elapsed"];

		Clock.Instance.SetState(state, (float)duration, (float)elapsed);
	}

	void HandlePlayerConnected(Socket socket, Packet packet, params object[] args) {
		string name = "System";
		string message = JSON.Parse(packet.ToString())[1]["name"] + " connected";

		Chat.Instance.AddMessage(name, message);
	}

	void HandleChatMessage(Socket socket, Packet packet, params object[] args) {
		string name = JSON.Parse(packet.ToString())[1]["name"];
		string message = JSON.Parse(packet.ToString())[1]["message"];
		
		Chat.Instance.AddMessage(name, message);
	}

	void HandleRenamePlayer(Socket socket, Packet packet, params object[] args) {
		string name = "System";
		string message = JSON.Parse(packet.ToString())[1]["oldName"] + " became " + JSON.Parse(packet.ToString())[1]["newName"];

		Chat.Instance.AddMessage(name, message);
	}

	void HandleGameResults(Socket socket, Packet packet, params object[] args) {
		JSONNode results = JSON.Parse(packet.ToString())[1]["results"];

		for(int n = 0; n < results.Count; n++) {
			Results.Instance.AddEntry(n + 1, results[n]["name"], results[n]["score"]);
		}
	}

	void HandlePlayerDisconnected(Socket socket, Packet packet, params object[] args) {
		string name = "System";
		string message = JSON.Parse(packet.ToString())[1]["name"] + " disconnected";

		Chat.Instance.AddMessage(name, message);
	}

	public void SendRequestState() {
		socketManager.Socket.Emit("request state");
	}

	public void SendChatMessage(string message) {
		socketManager.Socket.Emit("chat message", message);
	}

	public void SendRename(string name) {
		socketManager.Socket.Emit("rename player", name);
	}

	public void SendPlayerScore(int score) {
		socketManager.Socket.Emit("player score", score);
	}
}

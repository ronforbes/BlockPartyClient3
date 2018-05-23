using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonNavigator : MonoBehaviour {
	public void LoadLobby() {
		SceneManager.LoadScene("Lobby");
	}
}

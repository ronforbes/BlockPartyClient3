using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButtonNavigator : MonoBehaviour {
	public void LoadLobby() {
		SceneManager.LoadScene("Lobby");
	}
}

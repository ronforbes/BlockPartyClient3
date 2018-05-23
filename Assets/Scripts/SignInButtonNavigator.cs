using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignInButtonNavigator : MonoBehaviour {
	public void LoadLogin() {
		SceneManager.LoadScene("Login");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResultsRenderer : MonoBehaviour {
	public GameObject ResultsEntryPrefab;
	GameObject playerResultsEntry;

	void Awake() {
		playerResultsEntry = null;

		Results.Instance.OnResultsChanged += HandleResultsChanged;
	}

	void Start() {
		PopulateResults();
	}

	void HandleResultsChanged(object sender, ResultsChangedEventArgs e) {
		PopulateResults();
	}

	void PopulateResults() {
		if(playerResultsEntry != null) {
			Destroy(playerResultsEntry);
		}

		playerResultsEntry = null;

		ResultsEntry playerEntry = Results.Instance.Entries.Find(entry => entry.Name == "Ron");

		if(playerEntry != null) {
			GameObject gameObject = Instantiate(ResultsEntryPrefab, Vector3.zero, Quaternion.identity);
			gameObject.transform.SetParent(transform);
			gameObject.transform.localScale = Vector3.one;
			Text rankText = gameObject.transform.Find("Rank Text").GetComponent<Text>();
			rankText.text = playerEntry.Rank.ToString();
			Text nameText = gameObject.transform.Find("Name Text").GetComponent<Text>();
			nameText.text = playerEntry.Name;
			Text scoreText = gameObject.transform.Find("Score Text").GetComponent<Text>();
			scoreText.text = playerEntry.Score.ToString();

			playerResultsEntry = gameObject;
		}
	}
}

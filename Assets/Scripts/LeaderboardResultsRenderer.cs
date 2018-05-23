using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardResultsRenderer : MonoBehaviour {
	public GameObject ResultsEntryPrefab;
	List<GameObject> resultsEntries;
	ScrollRect scrollRect;

	void Awake() {
		resultsEntries = new List<GameObject>();
		scrollRect = GameObject.Find("Leaderboard Scroll View").GetComponent<ScrollRect>();

		Results.Instance.OnResultsChanged += HandleResultsChanged;
	}

	void Start() {
		PopulateResults();
	}

	void HandleResultsChanged(object sender, ResultsChangedEventArgs e) {
		PopulateResults();
	}

	void PopulateResults() {
		foreach(GameObject entry in resultsEntries) {
			Destroy(entry);
		}

		resultsEntries.Clear();

		foreach(ResultsEntry entry in Results.Instance.Entries) {
			GameObject resultsEntry = Instantiate(ResultsEntryPrefab, Vector3.zero, Quaternion.identity);
			resultsEntry.transform.SetParent(transform);
			resultsEntry.transform.localScale = Vector3.one;
			Text rankText = resultsEntry.transform.Find("Rank Text").GetComponent<Text>();
			rankText.text = entry.Rank.ToString();
			Text nameText = resultsEntry.transform.Find("Name Text").GetComponent<Text>();
			nameText.text = entry.Name;
			Text scoreText = resultsEntry.transform.Find("Score Text").GetComponent<Text>();
			scoreText.text = entry.Score.ToString();
			resultsEntries.Add(resultsEntry);
		}

		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(0, 100 * Results.Instance.Entries.Count);
		scrollRect.verticalNormalizedPosition = 1.0f;
	}

	void OnDestroy() {
		Results.Instance.OnResultsChanged -= HandleResultsChanged;
	}
}

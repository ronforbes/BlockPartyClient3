using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsEntry {
	public int Rank;
	public string Name;
	public int Score;

	public ResultsEntry(int rank, string name, int score) {
		Rank = rank;
		Name = name;
		Score = score;
	}
}

public class ResultsChangedEventArgs : EventArgs {}

public class Results : MonoBehaviour {
	static Results instance;
	public static Results Instance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<Results>();

				if(instance != null) {
					DontDestroyOnLoad(instance.gameObject);
				}
			}

			return instance;
		}
	}

	List<ResultsEntry> entries;
	public event EventHandler<ResultsChangedEventArgs> OnResultsChanged = (sender, e) => {};
	public List<ResultsEntry> Entries {
		get { return entries; }
		set {
			if(entries != value) {
				entries = value;
				OnResultsChanged(this, new ResultsChangedEventArgs());
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

		entries = new List<ResultsEntry>();
	}

	void Start () {
		ClearEntries();

		OnResultsChanged(this, new ResultsChangedEventArgs());
	}
	
	public void ClearEntries() {
		Entries.Clear();
		OnResultsChanged(this, new ResultsChangedEventArgs());
	}

	public void AddEntry(int rank, string name, int score) {
		Entries.Add(new ResultsEntry(rank, name, score));
		OnResultsChanged(this, new ResultsChangedEventArgs());
	}
}

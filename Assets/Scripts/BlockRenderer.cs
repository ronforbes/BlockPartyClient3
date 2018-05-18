using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRenderer : MonoBehaviour {
	Block block;
	BlockSlider slider;
	BlockClearer clearer;
	BlockFaller faller;
	BlockMatcher matcher;
	SpriteRenderer spriteRenderer;
	public List<Color> Colors;

	void Awake() {
		block = GetComponent<Block>();
		slider = GetComponent<BlockSlider>();
		clearer = GetComponent<BlockClearer>();
		faller = GetComponent<BlockFaller>();
		matcher = GetComponent<BlockMatcher>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		block.OnBlockChanged += HandleBlockChanged;
	}

	void HandleBlockChanged(object sender, BlockChangedEventArgs e) {
		transform.position = new Vector3(block.X, block.Y, -1.0f);
		if (block.State == BlockState.Empty || block.State == BlockState.WaitingToEmpty) {
			spriteRenderer.enabled = false;
		} else {
			spriteRenderer.enabled = true;
			spriteRenderer.transform.localScale = Vector3.one;
			spriteRenderer.color = Colors[block.Type];
		}
		block.name = "Block[x=" + block.X.ToString() + ", y=" + block.Y.ToString() + ", type=" + block.Type.ToString() + ", state=" + block.State.ToString() + "]";
	}

	// Update is called once per frame
	void Update () {
		float timePercentage = 0.0f;
		switch(block.State) {
			case BlockState.Empty:
				break;
			case BlockState.Idle:
				break;

			case BlockState.Sliding:
				float distance = 0.0f;
				distance = slider.Direction == BlockSlider.SlideDirection.Left ? -transform.localScale.x : transform.localScale.x;
				timePercentage = slider.Elapsed / BlockSlider.Duration;
				transform.position = Vector3.Lerp (
					new Vector3 (block.X, block.Y, -1.0f), 
					new Vector3 (block.X + distance, block.Y, -1.0f), 
					timePercentage);
				break;

			case BlockState.WaitingToFall:
				break;

			case BlockState.Falling:
				timePercentage = faller.Elapsed / BlockFaller.Duration;
				transform.position = Vector3.Lerp(
					new Vector3(block.X, block.Y, -1.0f),
					new Vector3(block.X, block.Y - transform.localScale.y, -1.0f),
					timePercentage);
				break;

			case BlockState.Matched:
				spriteRenderer.color = matcher.Elapsed % 0.10f < 0.05f ? Color.white : Colors[block.Type];
				break;

			case BlockState.WaitingToClear:
				break;

			case BlockState.Clearing:
				timePercentage = clearer.Elapsed / BlockClearer.Duration;
				spriteRenderer.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timePercentage);
				spriteRenderer.color = Color.Lerp(Colors[block.Type], new Color(Colors[block.Type].r, Colors[block.Type].g, Colors[block.Type].b, 0.0f), timePercentage);
				break;

			case BlockState.WaitingToEmpty:
				break;
		}
	}
}

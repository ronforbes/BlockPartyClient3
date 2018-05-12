using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRenderer : MonoBehaviour {
	Block block;
	BlockSlider slider;
	SpriteRenderer spriteRenderer;
	public List<Color> Colors;

	void Awake() {
		block = GetComponent<Block>();
		slider = GetComponent<BlockSlider>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		float timePercentage = 0.0f;
		switch(block.State) {
			case BlockState.Idle:
				transform.position = new Vector3(block.X, block.Y, -1.0f);
				spriteRenderer.enabled = true;
				spriteRenderer.color = Colors[block.Type];
				break;

			case BlockState.Sliding:
				float distance = 0.0f;
				distance = slider.Direction == BlockSlider.SlideDirection.Left ? -transform.localScale.x : transform.localScale.x;
				timePercentage = slider.Elapsed / BlockSlider.Duration;
				transform.position = Vector3.Lerp (
					new Vector3 (block.X, block.Y, 0.0f), 
					new Vector3 (block.X + distance, block.Y, -1.0f), 
					timePercentage);
				if (block.Type == -1) {
					spriteRenderer.enabled = false;
				} else {
					spriteRenderer.enabled = true;
					spriteRenderer.color = Colors[block.Type];
				}
				break;
		}
	}
}

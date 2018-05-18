using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
	public Block[,] Blocks;
	public Block BlockPrefab;
	public const int Columns = 6, Rows = 7; // 6x6 with an extra row for new blocks

	void Awake() {
		Blocks = new Block[Columns, Rows];

		for(int x = 0; x < Columns; x++) {
			for(int y = 0; y < Rows; y++) {
				Blocks[x, y] = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity) as Block;
				Blocks[x, y].transform.parent = transform;
				Blocks[x, y].X = x;
				Blocks[x, y].Y = y;
			}
		}
	}

	// Use this for initialization
	void Start () {
		// ScoreManager.Instance.Reset();
		PopulateBlocks();
	}
	
	void PopulateBlocks() {
		for(int x = 0; x < Columns; x++) {
			for(int y = 0; y < Rows; y++) {
				Blocks[x, y].Type = GetRandomBlockType(x, y);
				Blocks[x, y].State = BlockState.Idle;
			}
		}
	}

	public int GetRandomBlockType(int x, int y) {
		int type;
		do {
			type = Random.Range(0, Block.TypeCount);
		} while((x != 0 && Blocks[x - 1, y].Type == type) || (y != 0 && Blocks[x, y - 1].Type == type));
		return type;
	}
}

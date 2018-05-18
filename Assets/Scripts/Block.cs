using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockState {
	Empty,
	Idle,
	Sliding,
	WaitingToFall,
	Falling,
	Matched,
	WaitingToClear,
	Clearing,
	WaitingToEmpty
}

public class BlockChangedEventArgs : EventArgs {}

public class Block : MonoBehaviour {
	public event EventHandler<BlockChangedEventArgs> OnBlockChanged = (sender, e) => {};

	public int X, Y;
	
	private int type;
	public int Type {
		get { return type; }
		set {
			if(type != value) {
				type = value;
				OnBlockChanged(this, new BlockChangedEventArgs());
			}
		}
	}
	public const int TypeCount = 6;

	private BlockState state;
	public BlockState State {
		get { return state; }
		set {
			if(state != value) {
				state = value;
				OnBlockChanged(this, new BlockChangedEventArgs());
			}
		}
	}
}

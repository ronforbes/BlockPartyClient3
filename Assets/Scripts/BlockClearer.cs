using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockClearer : MonoBehaviour {
	/// <summary>
    /// The block to be cleared.
    /// </summary>
    Block block;

    /// <summary>
    /// The block's emptying component
    /// </summary>
    BlockEmptier emptier;

	/// <summary>
	/// The sign manager used to create signs when blocks are cleared.
	/// </summary>
	//SignManager signManager;

    /// <summary>
    /// The amount of time that the block has been waiting to clear
    /// </summary>
    float delayElapsed;

    /// <summary>
    /// The interval of time that blocks are delayed from clearing
    /// </summary>
    public const float DelayInterval = 0.25f;

    /// <summary>
    /// The duration of the delay before blocks are cleared
    /// </summary>
    public float DelayDuration;

    /// <summary>
    /// The amount of time that a block has been clearing
    /// </summary>
    public float Elapsed;

    /// <summary>
    /// The duration of time taken to clear a block
    /// </summary>
    public const float Duration = 0.25f;

    /// <summary>
    /// Initializes the clearer by getting the associated block, emptier, and stats tracker
    /// </summary>
    void Awake()
    {
        block = GetComponent<Block>();
        emptier = GetComponent<BlockEmptier>();
		//signManager = GameObject.Find ("Sign Canvas").GetComponent<SignManager> ();
    }
	
    /// <summary>
    /// Starts clearing the block from the board
    /// </summary>
    public void Clear()
    {
        block.State = BlockState.WaitingToClear;

        delayElapsed = 0.0f;
    }

    /// <summary>
    /// Waits for the clear delay and then clears the block from the board
    /// </summary>
    void Update()
    {
        //if(Clock.Instance.State != Clock.ClockState.Game) {
        //    return;
        //}
        
        if (block.State == BlockState.WaitingToClear)
        {
            delayElapsed += Time.deltaTime;

            if (delayElapsed >= DelayDuration)
            {
                block.State = BlockState.Clearing;
                
                Elapsed = 0.0f;

                if (Score.Instance)
                {
                    Score.Instance.ScoreMatch();
                }

				/*Color color;
				switch(block.Type)
				{
				default: 
					color = new Color(0.0f, 0.0f, 0.0f);
					break;
				case 0:
					color = new Color(0.01f, 0.65f, 1.0f);
					break;
				case 1:
					color = new Color(1.0f, 0.18f, 0.0f);
					break;
				case 2:
					color = new Color(1.0f, 0.7f, 0.0f);
					break;
				case 3:
					color = new Color(0.25f, 0.23f, 0.67f);
					break;
				case 4:
					color = new Color(0.93f, 0.24f, 0.57f);
					break;
				case 5:
					color = new Color(0.40f, 0.76f, 0.0f);
					break;
				}
				signManager.CreateSign(block.X, block.Y, ScoreManager.MatchValue.ToString(), color);*/
            }
        }

        if (block.State == BlockState.Clearing)
        {
            Elapsed += Time.deltaTime;

            if (Elapsed >= Duration)
            {
                emptier.Empty();
            }
        }
    }
}
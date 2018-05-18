using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlider : MonoBehaviour {
    Block block;
    MatchDetector matchDetector;

	public enum SlideDirection
    {
        Left,
        Right,
        None
    }

    public SlideDirection Direction;
    public BlockState TargetState;
    public int TargetType;
    public float Elapsed;
    public const float Duration = 0.1f;

    // Use this for initialization
    void Awake()
    {
        block = GetComponent<Block>();
        matchDetector = GameObject.Find("Board").GetComponent<MatchDetector>();
    }
	
    public void Slide(SlideDirection direction)
    {
        block.State = BlockState.Sliding;

        // Reset the sliding timer
        Elapsed = 0.0f;

        Direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Clock.Instance.State != Clock.ClockState.Game) {
        //    return;
        //}
        
        if (block.State == BlockState.Sliding)
        {
            Elapsed += Time.deltaTime;

            if (Elapsed >= Duration)
            {
                block.State = TargetState;
                block.Type = TargetType;

                Direction = SlideDirection.None;

                if (block.State == BlockState.Idle)
                {
                    matchDetector.RequestMatchDetection(block);
                }
            }
        }
    }
}
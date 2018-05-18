using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMatcher : MonoBehaviour {
	Block block;
    BlockClearer clearer;
    BlockEmptier emptier;
    public float Elapsed;
    const float duration = 1.0f;

    // Use this for initialization
    void Awake()
    {
        block = GetComponent<Block>();
        clearer = GetComponent<BlockClearer>();
        emptier = GetComponent<BlockEmptier>();
    }
	
    public void Match(int matchedBlockCount, int delayCounter)
    {
        block.State = BlockState.Matched;

        Elapsed = 0.0f;

        clearer.DelayDuration = (matchedBlockCount - delayCounter) * BlockClearer.DelayInterval;
        emptier.DelayDuration = delayCounter * BlockEmptier.DelayInterval;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Clock.Instance.State != Clock.ClockState.Game) {
        //    return;
        //}
        
        if (block.State == BlockState.Matched)
        {
            Elapsed += Time.deltaTime;

            if (Elapsed >= duration)
            {
                clearer.Clear();
            }
        }
    }
}
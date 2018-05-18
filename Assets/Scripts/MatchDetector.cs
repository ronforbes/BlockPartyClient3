using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchDetection
{
    public Block Block;

    public MatchDetection(Block block)
    {
        Block = block;
    }
}

public class MatchDetector : MonoBehaviour {
	List<MatchDetection> matchDetections;
    Board board;
    //ChainDetector chainDetector;
	//SignManager signManager;
    public const int MinimumMatchLength = 3;

    // Use this for initialization
    void Awake()
    {
        matchDetections = new List<MatchDetection>();
        board = GetComponent<Board>();
        //chainDetector = GetComponent<ChainDetector>();
		//signManager = GameObject.Find ("Sign Canvas").GetComponent<SignManager> ();
    }
	
    public void RequestMatchDetection(Block block)
    {
        matchDetections.Add(new MatchDetection(block));
    }

    // Update is called once per frame
    void Update()
    {
        //if(Clock.Instance.State != Clock.ClockState.Game) {
        //    return;
        //}
        
        while (matchDetections.Count > 0)
        {
            MatchDetection detection = matchDetections [0];
            matchDetections.Remove(detection);

            // ensure that the block is still idle
            if (detection.Block.State == BlockState.Idle)
            {
                DetectMatch(detection.Block);
            }
        }
    }

    void DetectMatch(Block block)
    {
        //bool incrementChain = false;

        // look in four directions for matching blocks   
        int left = block.X;
        while (left > 0 && board.Blocks[left - 1, block.Y].State == BlockState.Idle && board.Blocks[left - 1, block.Y].Type == block.Type)
        {
            left--;
        }
        
        int right = block.X + 1;
        while (right < Board.Columns && board.Blocks[right, block.Y].State == BlockState.Idle && board.Blocks[right, block.Y].Type == block.Type)
        {
            right++;
        }
        
        int bottom = block.Y;
        while (bottom > 0 && board.Blocks[block.X, bottom - 1].State == BlockState.Idle && board.Blocks[block.X, bottom - 1].Type == block.Type)
        {
            bottom--;
        }
        
        int top = block.Y + 1; // Exclude the top row since it's for new incoming blocks
        while (top < Board.Rows - 1 && board.Blocks[block.X, top].State == BlockState.Idle && board.Blocks[block.X, top].Type == block.Type)
        {
            top++;
        }
        
        int width = right - left;
        int height = top - bottom;
        int matchedBlockCount = 0;
        bool horizontalMatch = false;
        bool verticalMatch = false;
        
        if (width >= MinimumMatchLength)
        {
            horizontalMatch = true;
            matchedBlockCount += width;
        }
        
        if (height >= MinimumMatchLength)
        {
            verticalMatch = true;
            matchedBlockCount += height;
        }
        
        if (!horizontalMatch && !verticalMatch)
        {
            return;
        }
        
        // if pattern matches both directions
        if (horizontalMatch && verticalMatch)
            matchedBlockCount--;

        int delayCounter = matchedBlockCount;

        // kill the pattern's blocks
        
        if (horizontalMatch)
        {
            // kill the pattern's blocks
            for (int killX = left; killX < right; killX++)
            {
                board.Blocks [killX, block.Y].GetComponent<BlockMatcher>().Match(matchedBlockCount, delayCounter);
                delayCounter--;
                //if (board.Blocks [killX, block.Y].GetComponent<BlockChaining>().ChainEligible)
                //{
                //    incrementChain = true;
                //}
            }
        }
        
        if (verticalMatch)
        {
            // kill the pattern's blocks
            for (int killY = top - 1; killY >= bottom; killY--)
            {
                board.Blocks [block.X, killY].GetComponent<BlockMatcher>().Match(matchedBlockCount, delayCounter);
                delayCounter--;
                //if (board.Blocks [block.X, killY].GetComponent<BlockChaining>().ChainEligible)
                //{
                //    incrementChain = true;
                //}
            }
        }

        // handle combos
        /*if (matchedBlockCount > 3)
        {
            if (ScoreManager.Instance)
            {
                ScoreManager.Instance.ScoreCombo(matchedBlockCount);
            }
			signManager.CreateSign(block.X, block.Y, matchedBlockCount.ToString(), new Color(0, 0, 0));
        }
        if (incrementChain)
        {
            chainDetector.IncrementChain();
			signManager.CreateSign(block.X, block.Y, chainDetector.ChainLength.ToString() + "x", new Color(0, 0, 0));
        }*/
    }
}
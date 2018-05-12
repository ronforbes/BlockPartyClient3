using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour {
	Board board;
    public Block SelectedBlock;

    // Use this for initialization
    void Awake()
    {
        board = GetComponent<Board>();
    }
	
    // Update is called once per frame
    void Update()
    {
        //if(Clock.Instance.State != Clock.ClockState.Game) {
        //    return;
        //}

        // If the mouse button is clicked, find the block it's over, and select it
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.name.Contains("Block"))
            {
                Block block = hit.collider.gameObject.GetComponent<Block>();
                if (block.State == BlockState.Idle && block.Y < Board.Rows - 1)
                {
                    SelectedBlock = block;
                }
            }
        }

        // If the mouse button is released, clear the selected block
        if (Input.GetMouseButtonUp(0))
        {
            SelectedBlock = null;
        }

        // If a block has been selected, and the mouse has been dragged beyond its left or right edge,
        // determine the left and right blocks to be slid
        if (SelectedBlock != null)
        {
            float leftEdge = (float)SelectedBlock.X - SelectedBlock.transform.localScale.x / 2;
            float rightEdge = (float)SelectedBlock.X + SelectedBlock.transform.localScale.x / 2;

            Block leftBlock = null, rightBlock = null;

            Vector3 mousePosition = Input.mousePosition;

            if (Camera.main.ScreenToWorldPoint(mousePosition).x < leftEdge &&
                SelectedBlock.State == BlockState.Idle &&
                SelectedBlock.X - 1 >= 0 &&
                (board.Blocks [SelectedBlock.X - 1, SelectedBlock.Y].State == BlockState.Idle ||
                board.Blocks [SelectedBlock.X - 1, SelectedBlock.Y].State == BlockState.Empty) &&
                (SelectedBlock.Y + 1 == Board.Rows || (SelectedBlock.Y + 1 < Board.Rows && 
                board.Blocks [SelectedBlock.X - 1, SelectedBlock.Y + 1].State != BlockState.Falling &&
                board.Blocks [SelectedBlock.X - 1, SelectedBlock.Y + 1].State != BlockState.WaitingToFall)))
            {
                leftBlock = board.Blocks [SelectedBlock.X - 1, SelectedBlock.Y];
                rightBlock = SelectedBlock;

                SelectedBlock = leftBlock;
            }

            if (Camera.main.ScreenToWorldPoint(mousePosition).x > rightEdge &&
                SelectedBlock.State == BlockState.Idle &&
                SelectedBlock.X + 1 < Board.Columns &&
                (board.Blocks [SelectedBlock.X + 1, SelectedBlock.Y].State == BlockState.Idle ||
                board.Blocks [SelectedBlock.X + 1, SelectedBlock.Y].State == BlockState.Empty) &&
                (SelectedBlock.Y + 1 == Board.Rows || (SelectedBlock.Y + 1 < Board.Rows &&
                board.Blocks [SelectedBlock.X + 1, SelectedBlock.Y + 1].State != BlockState.Falling &&
                board.Blocks [SelectedBlock.X + 1, SelectedBlock.Y + 1].State != BlockState.WaitingToFall)))
            {
                leftBlock = SelectedBlock;
                rightBlock = board.Blocks [SelectedBlock.X + 1, SelectedBlock.Y];

                SelectedBlock = rightBlock;
            }

            if (leftBlock != null && rightBlock != null)
            {
                SetSlideTarget(leftBlock, BlockSlider.SlideDirection.Right);
                SetSlideTarget(rightBlock, BlockSlider.SlideDirection.Left);

                leftBlock.GetComponent<BlockSlider>().Slide(BlockSlider.SlideDirection.Right);
                rightBlock.GetComponent<BlockSlider>().Slide(BlockSlider.SlideDirection.Left);
            }
        }
    }

    void SetSlideTarget(Block block, BlockSlider.SlideDirection direction)
    {
        // Save off the state of the block that this one will swap with
        Block targetBlock = null;
        if (direction == BlockSlider.SlideDirection.Left)
        {
            targetBlock = board.Blocks [block.X - 1, block.Y];
        }
        
        if (direction == BlockSlider.SlideDirection.Right)
        {
            targetBlock = board.Blocks [block.X + 1, block.Y];
        }

        BlockSlider slider = block.GetComponent<BlockSlider>();

        slider.TargetState = targetBlock.State;
        slider.TargetType = targetBlock.Type;
    }
}
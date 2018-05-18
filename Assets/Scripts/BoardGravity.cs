using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGravity : MonoBehaviour {
	Board board;
    MatchDetector matchDetector;

    // Use this for initialization
    void Awake()
    {
        board = GetComponent<Board>();
        matchDetector = GameObject.Find("Board").GetComponent<MatchDetector>();
    }
	
    // Update is called once per frame
    void Update()
    {
        //if(Clock.Instance.State != Clock.ClockState.Game) {
        //    return;
        //}
        
        for (int x = 0; x < Board.Columns; x++)
        {
            bool emptyBlockDetected = false;

            for (int y = 0; y < Board.Rows; y++)
            {
                if (board.Blocks [x, y].State == BlockState.Empty)
                {
                    emptyBlockDetected = true;
                }

                if (board.Blocks [x, y].State == BlockState.Idle && emptyBlockDetected)
                {
                    board.Blocks [x, y].GetComponent<BlockFaller>().Target = board.Blocks [x, y - 1];
                    board.Blocks [x, y].GetComponent<BlockFaller>().Fall();
                }

                if (board.Blocks [x, y].GetComponent<BlockFaller>().JustFell)
                {
                    if (y > 0 && (board.Blocks [x, y - 1].State == BlockState.Empty || board.Blocks [x, y - 1].State == BlockState.Falling))
                    {
                        board.Blocks [x, y].GetComponent<BlockFaller>().Target = board.Blocks [x, y - 1];
                        board.Blocks [x, y].GetComponent<BlockFaller>().ContinueFalling();
                    } else
                    {
                        board.Blocks [x, y].State = BlockState.Idle;

                        matchDetector.RequestMatchDetection(board.Blocks [x, y]);
                    }

                    board.Blocks [x, y].GetComponent<BlockFaller>().JustFell = false;
                }
            }
        }

		// Add new blocks on the top of the board
		for(int x = 0; x < Board.Columns; x++) {
			if(board.Blocks[x, Board.Rows - 1].State == BlockState.Empty) {
				board.Blocks[x, Board.Rows - 1].Type = board.GetRandomBlockType(x, Board.Rows - 1);
				
				if(board.Blocks[x, Board.Rows - 2].State == BlockState.Idle) {
					board.Blocks[x, Board.Rows - 1].State = BlockState.Idle;
				}

				if(board.Blocks[x, Board.Rows - 2].State == BlockState.Empty || board.Blocks[x, Board.Rows - 2].State == BlockState.Falling) {
					board.Blocks[x, Board.Rows - 1].GetComponent<BlockFaller>().Target = board.Blocks[x, Board.Rows - 2];
					board.Blocks[x, Board.Rows - 1].GetComponent<BlockFaller>().ContinueFalling();
				}
			}
		}
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    int[,] board;
    bool playing = false;

    int score = 0;

    public GameObject uiBoard;
    public Text scoreUI;

	// Use this for initialization
	void Start () {
        ResetBoard ();
	}
	
	// Update is called once per frame
	void Update () {
        if (playing) {
            if (Input.GetKeyDown (KeyCode.UpArrow) && CanMoveUp()) {
                MoveUp ();
            } else if (Input.GetKeyDown (KeyCode.RightArrow) && CanMoveRight()) {
                MoveRight ();
            } else if (Input.GetKeyDown (KeyCode.DownArrow) && CanMoveDown()) {
                MoveDown ();
            } else if (Input.GetKeyDown (KeyCode.LeftArrow) && CanMoveLeft()) {
                MoveLeft ();
            }
        }
	}

    public void MoveUp() {
        for (int x = 0; x < board.GetLength (1); x++) {
            int[] originalArray = new int[board.GetLength (0)];
            for (int y = 0; y < board.GetLength (0); y++) {
                originalArray [y] = board [y, x];
            }
            int[] arr = CondenseArray (originalArray);
            for (int y = 0; y < board.GetLength (0); y++) {
                board [y, x] = arr[y];
            }
        }

        FinishMove ();
    }

    public void MoveRight() {
        for (int y = 0; y < board.GetLength (0); y++) {
            int[] originalArray = new int[board.GetLength (1)];
            for (int x = 0; x < board.GetLength (1); x++) {
                originalArray [x] = board [y, x];
            }
            System.Array.Reverse (originalArray);
            int[] arr = CondenseArray (originalArray);
            System.Array.Reverse (arr);
            for (int x = 0; x < board.GetLength (1); x++) {
                board [y, x] = arr[x];
            }
        }
        FinishMove ();
    }

    public void MoveDown() {
        for (int x = 0; x < board.GetLength (1); x++) {
            int[] originalArray = new int[board.GetLength (0)];
            for (int y = 0; y < board.GetLength (0); y++) {
                originalArray [y] = board [y, x];
            }
            System.Array.Reverse (originalArray);
            int[] arr = CondenseArray (originalArray);
            System.Array.Reverse (arr);
            for (int y = 0; y < board.GetLength (0); y++) {
                board [y, x] = arr[y];
            }
        }
        FinishMove ();
    }

    public void MoveLeft() {
        for (int y = 0; y < board.GetLength (0); y++) {
            int[] originalArray = new int[board.GetLength (1)];
            for (int x = 0; x < board.GetLength (1); x++) {
                originalArray [x] = board [y, x];
            }
            int[] arr = CondenseArray (originalArray);
            for (int x = 0; x < board.GetLength (1); x++) {
                board [y, x] = arr[x];
            }
        }
        FinishMove ();
    }

    public bool CanMoveLeft() {
        for (int y = 0; y < board.GetLength (0); y++) {
            int lastNum = board [y, 0];
            for (int x = 1; x < board.GetLength (1); x++) {
                int currNum = board [y, x];
                if (currNum != 0 && (currNum == lastNum || lastNum == 0)) {
                    return true;
                }
                lastNum = currNum;
            }
        }
        return false;
    }

    public bool CanMoveRight() {
        for (int y = 0; y < board.GetLength (0); y++) {
            int lastNum = board [y, board.GetLength (1) - 1];
            for (int x = 1; x < board.GetLength (1); x++) {
                int currNum = board [y, (board.GetLength (1) - 1) - x];
                if (currNum != 0 && (currNum == lastNum || lastNum == 0)) {
                    return true;
                }
                lastNum = currNum;
            }
        }
        return false;
    }

    public bool CanMoveUp() {
        for (int x = 0; x < board.GetLength (1); x++) {
            int lastNum = board [0, x];
            for (int y = 1; y < board.GetLength (0); y++) {
                int currNum = board [y, x];
                if (currNum != 0 && (currNum == lastNum || lastNum == 0)) {
                    return true;
                }
                lastNum = currNum;
            }
        }
        return false;
    }

    public bool CanMoveDown() {
        for (int x = 0; x < board.GetLength (1); x++) {
            int lastNum = board [board.GetLength (0) - 1, x];
            for (int y = 1; y < board.GetLength (0); y++) {
                int currNum = board [(board.GetLength (0) - 1) - y, x];
                if (currNum != 0 && (currNum == lastNum || lastNum == 0)) {
                    return true;
                }
                lastNum = currNum;
            }
        }
        return false;
    }

    int[] CondenseArray(int[] originalArr) {
        int[] arr = new int[originalArr.Length];
        int n = 0;
        for (int x = 0; x < originalArr.Length; x++) {
            if (originalArr [x] != 0) {
                if (arr [n] == originalArr [x]) {
                    arr [n] += originalArr [x];
                    score += arr [n];
                    n++;
                } else if (arr [n] == 0) {
                    arr [n] = originalArr [x];
                } else {
                    n++;
                    arr [n] = originalArr [x];
                }
            }
        }

        return arr;
    }

    void FinishMove() {
        AddTile ();

        if (!(CanMoveUp() || CanMoveRight() || CanMoveLeft() || CanMoveDown())) {
            GameOver ();
        }
    }

    public void ResetBoard() {
        score = 0;
        board = new int[4, 4];

        AddTile ();
        AddTile ();
        playing = true;
    }

    int NumberOfFreeTiles() {
        int numberOfFreeTiles = 0;

        for (int y = 0; y < board.GetLength(0); y++) {
            for (int x = 0; x < board.GetLength(1); x++) {
                if (board [y, x] == 0) {
                    numberOfFreeTiles++;
                }
            }
        }

        return numberOfFreeTiles;
    }

    void AddTile() {
        int freeTiles = NumberOfFreeTiles ();
        int tileValue = (Random.value < (1.0 / 16)) ? 4 : 2;
        int pos = Random.Range (0, freeTiles);

        AddTileAt (tileValue, pos);
        ReRenderBoard ();
    }

    void ReRenderBoard() {
        for (int y = 0; y < board.GetLength(0); y++) {
            for (int x = 0; x < board.GetLength(1); x++) {
                GameObject button = uiBoard.transform.GetChild (y).GetChild (x).gameObject;
                int value = board [y, x];

                if (value == 0) {
                    button.SetActive (false);
                } else {
                    button.SetActive (true);
                    Text textObj = button.transform.GetChild (0).GetComponent<Text> ();
                    textObj.text = "" + value;
                    Image buttonImage = button.GetComponent<Image> ();
                    buttonImage.color = GetColor (value);
                }
            }
        }
        scoreUI.text = "Score: " + score;
    }

    void AddTileAt(int tileValue, int pos) {
        for (int y = 0; y < board.GetLength(0); y++) {
            for (int x = 0; x < board.GetLength(1); x++) {
                if (board [y, x] == 0) {
                    if (pos == 0) {
                        board [y, x] = tileValue;
                    }
                    pos--;
                }
            }
        }
    }

    void GameOver() {
        Debug.Log ("Game Over");
        scoreUI.text = "Game Over Score: " + score;
        playing = false;
    }



    Color GetColor(int value) {
        switch (value) {
            case 2:
                return Color.red;
            case 4:
                return Color.blue;
            case 8:
                return Color.green;
            case 16:
                return Color.cyan;
            case 32:
                return Color.magenta;
            case 64:
                return Color.yellow;
            default:
                return Color.black;
        }
    }

}

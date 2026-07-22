using UnityEngine;

public class Square : MonoBehaviour
{
    public int Index;
    private BoardView boardView;

    void Start()
    {
        boardView = FindFirstObjectByType<BoardView>();
    }

    void OnMouseDown()
    {
        if (boardView != null)
            boardView.OnSquareClicked(Index);
    }
}
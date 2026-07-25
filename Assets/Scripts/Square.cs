using UnityEngine;

public class Square : MonoBehaviour
{
    public int Index;
    private BoardView boardView;

    void Start()
    {
        boardView = FindFirstObjectByType<BoardView>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Клик по мне!");
                if (boardView != null)
                    boardView.OnSquareClicked(Index);
            }
        }
    }
}
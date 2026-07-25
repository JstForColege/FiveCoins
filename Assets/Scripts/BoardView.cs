using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    int rightSquare = -1;
    int leftSquare = -1;
    public int _activeSquare = -1;
    public List<GameObject> squares;
    private GameLogic gameLogic;

    

    private void Start()
    {
        gameLogic = FindFirstObjectByType<GameLogic>();
    }

    public void OnSquareClicked(int index)
    {
        
        if (gameLogic.IsEmpty(index) || gameLogic.IsDoubled(index))
            return;

        if (index == leftSquare || index == rightSquare)
        {
            gameLogic.Move(_activeSquare, index);
            UpdateBoard();
            DisableActive(0.2f);
            _activeSquare = -1;
            leftSquare = rightSquare = -1;
            if (gameLogic.IsWin())
                Debug.Log("Победа!");
            return;
        }

        if (_activeSquare == -1)
        {
            _activeSquare = index;
            ShowActiveSquare(index);
            ShowWays(index);
            return;
        }

        if (_activeSquare != index)
        {
            DisableActive(0.2f);
            _activeSquare = index;
            ShowActiveSquare(index);
            ShowWays(index);
            return;
        }

        DisableActive(0.2f);
        _activeSquare = -1;
    }

    public void ShowActiveSquare(int index)
    {
        squares[index].gameObject.transform.DOLocalMoveY(squares[index].gameObject.transform.localPosition.y + 0.5f, 0.2f);
    }

    public void MakeColorful(int index)
    {
        squares[index].gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
    }

    public void DisableActive(float duration)
    {

        _activeSquare = -1;
        rightSquare = -1;
        leftSquare = -1;
        int index = -1;
        foreach (var square in squares)
        {
            index++;
            if(gameLogic.IsDoubled(index) || gameLogic.IsEmpty(index))
            {
                square.gameObject.GetComponent<SpriteRenderer>().color = Color.lightGreen;
            }
            else
            {
                square.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                square.gameObject.transform.DOLocalMoveY(0, duration);
            }
        }
    }

    public void ResetSquares()
    {
        Button restartButton = GameObject.Find("RestartBtn").GetComponent<Button>();
        restartButton.interactable = false;
        gameLogic.Restart();
        UpdateBoard();
        DisableActive(1f);
        DOVirtual.DelayedCall(1f, () => restartButton.interactable = true);
    }

    public void ShowWays(int index)
    {
        leftSquare = gameLogic.FindLeftSquare(index);
        Debug.Log("Левый ход " + leftSquare);
        if (leftSquare != -1) MakeColorful(leftSquare);
        rightSquare = gameLogic.FindRightSquare(index);
        Debug.Log("Правый ход " + rightSquare);
        if (rightSquare != -1 && rightSquare < 10) MakeColorful(rightSquare);
    }

    bool DoubledSquare(int index)
    {
        foreach(var item in gameLogic.slots)
        {
            if (item[0] == index || item[1] == index)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateBoard()
    {
        for (int cellIndex = 0; cellIndex < 10; cellIndex++)
        {
            var slot = gameLogic.GetSlot(cellIndex);
            for (int layer = 0; layer < slot.Count; layer++)
            {
                int squareId = slot[layer];
                GameObject square = squares[squareId];
                Vector3 targetPos = new Vector3(cellIndex * 1.2f, layer * 1, 0);
                square.transform.DOLocalMove(targetPos, 1f);
            }
        }
    }


}
using System;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    public void OnSquareClicked(int index)
    {
        Debug.Log($"Клик по {index} квадратику!");
    }
}

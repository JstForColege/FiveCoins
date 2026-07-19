using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameLogic : MonoBehaviour
{
    private const int BOARD_SIZE = 10;
    private const int MAX_STEPS = 3;

    private List<List<int>> slots;

    private Stack<MoveRecord> moveHistory;

    private List<List<int>> initialSlots;

    private struct MoveRecord
    {
        public int FromIndex;
        public int ToIndex;
        public int SquareId;
    }
    public GameLogic()
    {
        Init();
    }

    private void Init()
    {
        slots = new List<List<int>>(BOARD_SIZE);
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            slots.Add(new List<int> { i });
        }
        initialSlots = slots.Select(s => new List<int>(s)).ToList();
        moveHistory = new Stack<MoveRecord>();
    }
    public void Restart()
    {
        slots = initialSlots.Select(s => new List<int>(s)).ToList();
        moveHistory.Clear();
    }

    private bool IsDoubled(int index)
    {
        if (slots[index].Count == 2) return true;
        return false;
    }

    private bool IsEmpty(int index)
    {
        if (slots[index].Count == 0) return true;
        return false;
    }

    private int GetTopCoin(int index)
    {
        return slots[index].Last();
    }

    private int PopCoin(int index)
    {
        int coin = slots[index].Last();
        slots[index].RemoveAt(slots[index].Count - 1);
        return coin;
    }

    private void AddCoin(int index, int coinId)
    {
        slots[index].Add(coinId);
    }


}

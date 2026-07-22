using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public MoveRecord(int fromIndex, int toIndex)
        {
            FromIndex = fromIndex;
            ToIndex = toIndex;
        }
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
    public void MoveBack()
    {
        MoveRecord last = moveHistory.Pop();
        int square = slots[last.ToIndex].Last();
        slots[last.ToIndex].RemoveAt(slots[last.ToIndex].Count - 1);
        slots[last.FromIndex].Add(square);
    }

    public int FindLeftSquare(int index)
    {
        int current = index - 1;
        int spent = 0;

        while (current >= 0 && spent < MAX_STEPS)
        {
            if (IsEmpty(current))
            {
                current--;
                continue;
            }

            if (!IsDoubled(current))
            {
                spent += 1;
                current--;
                continue;
            }

            spent += 2;
            if (spent > MAX_STEPS) return -1;

            int next = current - 1;

            if (next >= 0 && !IsDoubled(next)) return next;
            else return -1;
        }
        return -1;
    }

    public int FindRightSquare(int index)
    {
        int current = index + 1;
        int spent = 0;

        while (current <= 10 && spent < MAX_STEPS)
        {
            if (IsEmpty(current))
            {
                current++;
                continue;
            }
            if (!IsDoubled(current))
            {
                spent += 1;
                current++;
                continue;
            }
            spent += 2;
            if (spent > MAX_STEPS) return -1;

            int next = current + 1;

            if (next <= 10 && IsDoubled(next)) return next;
            else return -1;
        }
        return -1;
    }

    public void Move(int toIndex, int fromIndex)
    {
        AddSquare(toIndex, fromIndex);
        moveHistory.Append(new MoveRecord(fromIndex, toIndex));
        PopSquare(fromIndex);
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

    private int GetTopSquare(int index)
    {
        return slots[index].Last();
    }

    private int PopSquare(int index)
    {
        int coin = slots[index].Last();
        slots[index].RemoveAt(slots[index].Count - 1);
        return coin;
    }

    private void AddSquare(int toIndex, int fromIndex)
    {
        slots[toIndex].Add(fromIndex);
    }

    public bool IsWin()
    {
        for(int i = 0; i< BOARD_SIZE; ++i)
        {
            if (!(IsDoubled(i))) return false;
        }
        return true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private const int BOARD_SIZE = 10;
    private const int MAX_STEPS = 3;

    public List<List<int>> slots;

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
        if (moveHistory.Count == 0) return;
        MoveRecord last = moveHistory.Pop();
        int coin = slots[last.ToIndex].Last();
        slots[last.ToIndex].RemoveAt(slots[last.ToIndex].Count - 1);
        slots[last.FromIndex].Add(coin);
    }

    public int FindLeftSquare(int index)
    {
        int current = index - 1;
        int spent = 0;

        while (current >= 0 && spent < MAX_STEPS)
        {
            if (current < 0)
            {
                return -1;
            }
            if (IsEmpty(current))
            {
                current--;
                continue;
            }

            if (!IsDoubled(current))
            {
                spent += 1; 
                if (spent >= 3)
                {
                    return current;
                }
                current--;
                continue;
            }

            spent += 2;
            if (spent > MAX_STEPS) return -1;

            int next = current - 1;

            if (next >= 0 && !IsDoubled(next) && !IsEmpty(next)) return next;
            else if (next >= 0 && IsEmpty(next))
            {
                for(int i = next; i>0; i--)
                {
                    if (IsDoubled(i)) return -1;
                    if (IsEmpty(i)) continue;
                    return i;
                }
            }
            else return -1;
        }

        return current;
    }

    public int FindRightSquare(int index)
    {
        int current = index + 1;
        int spent = 0;

        while (current < BOARD_SIZE && spent < MAX_STEPS)
        {
            if(current > BOARD_SIZE)
            {
                return -1;
            }
            if (IsEmpty(current)) 
            {
                current++; continue;
            }

            if (!IsDoubled(current))
            {
                spent++;
                if(spent >= 3)
                {
                    return current;
                }
                current++;
                continue;
            }

            spent += 2;
            if (spent > MAX_STEPS) return -1;

            int next = current + 1;

            if (next >= 0 && !IsDoubled(next) && !IsEmpty(next)) return next;
            else if (next >= 0 && IsEmpty(next))
            {
                for (int i = next; i < BOARD_SIZE; i++)
                {
                    if (IsDoubled(i)) return -1;
                    if (IsEmpty(i)) continue;
                    return i;
                }
            }
            else return -1;
        }
        return current;
    }

    public void Move(int fromIndex, int toIndex)
    {
        int coin = PopSquare(fromIndex);
        slots[toIndex].Add(coin);
        moveHistory.Push(new MoveRecord(fromIndex, toIndex));
    }

    public bool IsDoubled(int index)
    {
        if (slots[index].Count == 2) return true;
        return false;
    }

    public bool IsEmpty(int index)
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

    public bool IsWin()
    {
        for(int i = 0; i< BOARD_SIZE; ++i)
        {
            if (!(IsDoubled(i))) return false;
        }
        Debug.Log("Вы выиграли!");
        return true;
    }

    public IReadOnlyList<int> GetSlot(int index)
    {
        return slots[index].AsReadOnly();
    }
}
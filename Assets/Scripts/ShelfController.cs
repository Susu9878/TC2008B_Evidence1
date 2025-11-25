using UnityEngine;

public class ShelfController : MonoBehaviour
{
    public Transform[] slots;
    private bool[] isSlotFull;

    void Start()
    {
        isSlotFull = new bool[slots.Length];
    }

    public bool HasSpace()
    {
        for (int i = 0; i < isSlotFull.Length; i++)
        {
            if (!isSlotFull[i]) return true;
        }
        return false;
    }

    public Transform GetEmptySlot()
    {
        for (int i = 0; i < isSlotFull.Length; i++)
        {
            if (!isSlotFull[i])
            {
                isSlotFull[i] = true;
                return slots[i];
            }
        }
        return null;
    }
}
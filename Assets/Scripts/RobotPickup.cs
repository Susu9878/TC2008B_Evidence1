using UnityEngine;
using System.Collections.Generic;

public class RobotPickup : MonoBehaviour
{
    public Transform stackPoint;
    public float stackHeight = 0.4f;
    public int maxStack = 1;


    private List<GameObject> carriedBoxes = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("box")) return;

        BoxController box = other.GetComponent<BoxController>();
        if (box == null) return;
        if (box.isPickedUp) return;
        if (carriedBoxes.Count >= maxStack) return;

        box.isPickedUp = true;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        carriedBoxes.Add(other.gameObject);
        other.transform.SetParent(stackPoint);

        other.transform.localPosition = new Vector3(0, carriedBoxes.Count * stackHeight, 0);
        other.transform.localEulerAngles = Vector3.zero;
        other.transform.localScale = Vector3.one;
    }

    public int GetCarriedCount() => carriedBoxes.Count;

    public GameObject GetLastCarriedBox()
    {
        if (carriedBoxes.Count == 0) return null;
        return carriedBoxes[carriedBoxes.Count - 1];
    }

    public void RemoveLastBox()
    {
        if (carriedBoxes.Count > 0)
            carriedBoxes.RemoveAt(carriedBoxes.Count - 1);
    }
}
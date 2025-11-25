using UnityEngine;
using TMPro;   

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;  
    public ShelfController[] shelves;  
    
    private float timer = 0f;
    private bool running = true;

    void Update()
    {
        if (!running) return;

        
        timer += Time.deltaTime;
        UpdateTimerUI();


        if (AllShelvesFull())
        {
            running = false;    
            Debug.Log("Simulation Finished. Total Time: " + timer);
        }
    }

    bool AllShelvesFull()
    {
        foreach (var shelf in shelves)
        {
            if (shelf.HasSpace())  
                return false;
        }
        return true;
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
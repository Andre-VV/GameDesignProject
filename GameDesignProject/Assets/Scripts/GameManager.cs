using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 120f; // Total duration of the game in seconds

    private float currentTime = 0f; // Current time elapsed in the game
    private bool isGameRunning = false; // Flag to check if the game is running

    public TextMeshProUGUI timerText; // Reference to the UI Text component to display the timer

    public EnemySpawning[] spawners; // Reference to the EnemySpawning script

    public float spawnStartTime = 20f; // Time before the first enemy spawns



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GameStartRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameRunning)
            return;

        currentTime += Time.deltaTime; // Increment the current time by the time elapsed since the last frame
        UpdateTimerUI(); // Update the timer UI

        if (currentTime >= gameDuration)
        {
            EndGame(); // End the game when the duration is reached
        }
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            EndGame(); // End the game if the player is destroyed
        }
    }

    IEnumerator GameStartRoutine()
    {
        currentTime = 0f;
        isGameRunning = true;

        //wait before starting a spawner
        yield return new WaitForSeconds(spawnStartTime);

        foreach (EnemySpawning spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.startSpawning(gameDuration - currentTime);
                yield return new WaitForSeconds(spawnStartTime); // Wait before starting the next spawner
            }
        }
    }

    /* This method can be called to start the game manually, for example from a UI button.
     * It resets the timer and starts the enemy spawners.
     
    public void StartGame()
    {
        currentTime = 0f;
        isGameRunning = true;
        if (spawners != null)
        {
            spawners.startSpawning();
        }
    }
    */

    void EndGame()
    {
        isGameRunning = false;

        foreach (EnemySpawning spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.stopSpawning();
            }
        }
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            timerText.text = "Game Over: Win!!!";
        }
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            float timeRemaining = Mathf.Max(0f, gameDuration - currentTime);
            timerText.text = "Game Over: Lose! " + timeRemaining.ToString();
        }

        Debug.Log("Game Over!");
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float timeRemaining = Mathf.Max(0f, gameDuration - currentTime);
            timerText.text = $"Time: {timeRemaining:F1}s";
        }
    }
}

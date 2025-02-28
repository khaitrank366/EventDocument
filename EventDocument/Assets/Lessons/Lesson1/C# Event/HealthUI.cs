using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    private void OnEnable()
    {
        Health.onPlayerDeath += DisplayGameOver;
    }

    private void OnDisable()
    {
        Health.onPlayerDeath -= DisplayGameOver;
    }

    void DisplayGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}

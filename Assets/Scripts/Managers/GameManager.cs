using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string GAME_VERSION = "0.0.1";
    private static GameManager Instance;

    private float batteryPercentage = 100;
    private float staminaPercentage = 100;
    private float healthPercentage = 100;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

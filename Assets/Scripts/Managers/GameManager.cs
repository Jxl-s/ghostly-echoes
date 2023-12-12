using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string GAME_VERSION = "0.0.1";
    public static GameManager Instance;

    public float BatteryPercentage = 100;
    public float StaminaPercentage = 100;
    public float HealthPercentage = 100;

    public bool ControlsEnabled = true;
    public bool SprintEnabled = true;
    public bool SFXEnabled = true;
    public bool isCutscene = false;
    public bool Dead = false;

    public bool MonsterActive = false;

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
        SprintEnabled = true;
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void ReduceHealth(int damage)
    {
        if (Dead) return;

        HealthPercentage -= damage;
        StartCoroutine(HUDManager.Instance.ShowDamageMask());

        if (HealthPercentage <= 0)
        {
            HealthPercentage = 0;
            Dead = true;
            StartCoroutine(HUDManager.Instance.ShowDeath());
        }
    }

    public void PauseGame()
    {
        ControlsEnabled = false;
    }

    public void ResumeGame()
    {
        ControlsEnabled = true;
    }

    public void SetCutscene(bool isScene){
        if(isScene){
            isCutscene = true;
            ControlsEnabled = false;
        }
        else{
            isCutscene = false;
            ControlsEnabled = true;
        }
    }
}

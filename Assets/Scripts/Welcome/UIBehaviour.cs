using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionLabel;
    [SerializeField] private TextMeshProUGUI titleLabel;

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private int titleOriginalY;
    private int startOriginalY;
    private int quitOriginalY;

    // Start is called before the first frame update
    void Start()
    {
        titleOriginalY = (int)titleLabel.rectTransform.anchoredPosition.y;
        startOriginalY = (int)startButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition.y;
        quitOriginalY = (int)quitButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition.y;

        versionLabel.text = "Version: " + GameManager.GAME_VERSION;

        // Make the title fade in, after 1 second
        titleLabel.CrossFadeAlpha(0, 0, true);
        startButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0, 0, true);
        quitButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0, 0, true);

        Invoke(nameof(FadeInTitle), 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Make the title bounce up and down, rotate a bit too, and fade in and out
        float newY = titleOriginalY + Mathf.Sin(Time.time * 2) * 10;
        titleLabel.rectTransform.anchoredPosition = new Vector2(titleLabel.rectTransform.anchoredPosition.x, newY);

        float newRot = Mathf.Sin(Time.time * 3) * 0.5f;
        titleLabel.rectTransform.rotation = Quaternion.Euler(0, 0, newRot);

        // Make the buttons bounce up and down, and fade in and out
        float newStartY = startOriginalY + Mathf.Sin(Time.time * 2 + 3) * 5;
        startButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(startButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition.x, newStartY);

        float newStartRot = Mathf.Sin(Time.time * 3 + 3) * 0.2f;
        startButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.rotation = Quaternion.Euler(0, 0, newStartRot);

        float newQuitY = quitOriginalY + Mathf.Sin(Time.time * 2 + 4) * 5;
        quitButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition = new Vector2(quitButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.anchoredPosition.x, newQuitY);

        float newQuitRot = Mathf.Sin(Time.time * 3 + 4) * 0.2f;
        quitButton.GetComponentInChildren<TextMeshProUGUI>().rectTransform.rotation = Quaternion.Euler(0, 0, newQuitRot);

        // Make the camera rotate (increment Y rot by 1 degree)
        Camera.main.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y + Time.deltaTime, 0);
    }

    private void FadeInTitle()
    {
        titleLabel.CrossFadeAlpha(1, 1, false);

        startButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(1, 1, false);
        quitButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(1, 1, false);

        StartExit();
        LeaveExit();
    }

    private void FadeOutTitle()
    {
        titleLabel.CrossFadeAlpha(0, 1, false);

        startButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0, 1, false);
        quitButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0, 1, false);
    }

    public void StartEnter()
    {
        startButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeColor(new Color(0.3f, 0.5f, 0.3f, 0.5f), 0.5f, false, true);
    }

    public void StartExit()
    {
        startButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeColor(new Color(0.1f, 0.3f, 0.1f, 0.5f), 0.5f, false, true);
    }

    public void LeaveEnter()
    {
        quitButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeColor(new Color(0.5f, 0.3f, 0.3f, 0.5f), 0.5f, false, true);
    }

    public void LeaveExit()
    {
        quitButton.GetComponentInChildren<TextMeshProUGUI>().CrossFadeColor(new Color(0.3f, 0.1f, 0.1f, 0.5f), 0.5f, false, true);
    }

    public void StartGame()
    {
        FadeOutTitle();
        Invoke(nameof(LoadNextLevel), 1.0f);
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}

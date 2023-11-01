using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionLabel;
    [SerializeField] private TextMeshProUGUI titleLabel;

    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private int titleOriginalY;
    private int startOriginalY;
    private int settingsOriginalY;
    private int quitOriginalY;

    private TextMeshProUGUI startLabel;
    private TextMeshProUGUI settingsLabel;
    private TextMeshProUGUI quitLabel;


    // Start is called before the first frame update
    void Start()
    {
        startLabel = startButton.GetComponentInChildren<TextMeshProUGUI>();
        settingsLabel = settingsButton.GetComponentInChildren<TextMeshProUGUI>();
        quitLabel = quitButton.GetComponentInChildren<TextMeshProUGUI>();

        titleOriginalY = (int)titleLabel.rectTransform.anchoredPosition.y;
        startOriginalY = (int)startLabel.rectTransform.anchoredPosition.y;
        settingsOriginalY = (int)settingsLabel.rectTransform.anchoredPosition.y;
        quitOriginalY = (int)quitLabel.rectTransform.anchoredPosition.y;

        versionLabel.text = "Version: " + GameManager.GAME_VERSION;

        // Make the title fade in, after 1 second
        titleLabel.CrossFadeAlpha(0, 0, true);

        startLabel.CrossFadeAlpha(0, 0, true);
        settingsLabel.CrossFadeAlpha(0, 0, true);
        quitLabel.CrossFadeAlpha(0, 0, true);

        Invoke(nameof(FadeInTitle), 1.0f);

        Invoke(nameof(StartExit), 0.5f);
        Invoke(nameof(SettingsExit), 0.5f);
        Invoke(nameof(LeaveExit), 0.5f);
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
        startLabel.rectTransform.anchoredPosition = new Vector2(startLabel.rectTransform.anchoredPosition.x, newStartY);

        float newStartRot = Mathf.Sin(Time.time * 3 + 3) * 0.2f;
        startLabel.rectTransform.rotation = Quaternion.Euler(0, 0, newStartRot);

        float newQuitY = quitOriginalY + Mathf.Sin(Time.time * 2 + 4) * 5;
        quitLabel.rectTransform.anchoredPosition = new Vector2(quitLabel.rectTransform.anchoredPosition.x, newQuitY);

        float newQuitRot = Mathf.Sin(Time.time * 3 + 4) * 0.2f;
        quitLabel.rectTransform.rotation = Quaternion.Euler(0, 0, newQuitRot);

        float newSettingsY = settingsOriginalY + Mathf.Sin(Time.time * 2 + 5) * 5;
        settingsLabel.rectTransform.anchoredPosition = new Vector2(settingsLabel.rectTransform.anchoredPosition.x, newSettingsY);

        float newSettingsRot = Mathf.Sin(Time.time * 3 + 5) * 0.2f;
        settingsLabel.rectTransform.rotation = Quaternion.Euler(0, 0, newSettingsRot);

        // Make the camera rotate (increment Y rot by 1 degree)
        Camera.main.transform.rotation = Quaternion.Euler(45, Camera.main.transform.rotation.eulerAngles.y + Time.deltaTime, 0);
    }

    private void FadeInTitle()
    {
        titleLabel.CrossFadeAlpha(1, 1, false);

        startLabel.CrossFadeAlpha(1, 1, false);
        quitLabel.CrossFadeAlpha(1, 1, false);
    }

    private void FadeOutTitle()
    {
        titleLabel.CrossFadeAlpha(0, 1, false);
        startLabel.CrossFadeAlpha(0, 1, false);
        settingsLabel.CrossFadeAlpha(0, 1, false);
        quitLabel.CrossFadeAlpha(0, 1, false);

        // makr them not visible
        Invoke(nameof(DeactiveUI), 0.5f);
    }

    private void DeactiveUI() {
        startLabel.gameObject.SetActive(false);
        settingsLabel.gameObject.SetActive(false);
        quitLabel.gameObject.SetActive(false);
    }

    public void StartEnter()
    {
        startLabel.CrossFadeColor(new Color(1.0f, 1.0f, 1.0f, 0.75f), 0.5f, false, true);
    }

    public void StartExit()
    {
        startLabel.CrossFadeColor(new Color(0.3f, 0.3f, 0.3f), 0.5f, false, true);
    }

    public void SettingsEnter()
    {
        settingsLabel.CrossFadeColor(new Color(1.0f, 1.0f, 1.0f), 0.5f, false, true);
    }

    public void SettingsExit()
    {
        settingsLabel.CrossFadeColor(new Color(0.3f, 0.3f, 0.3f), 0.5f, false, true);
    }

    public void LeaveEnter()
    {
        quitLabel.CrossFadeColor(new Color(1.0f, 1.0f, 1.0f), 0.5f, false, true);
    }

    public void LeaveExit()
    {
        quitLabel.CrossFadeColor(new Color(0.3f, 0.3f, 0.3f), 0.5f, false, true);
    }

    public void StartGame()
    {
        FadeOutTitle();
        Invoke(nameof(LoadNextLevel), 1.0f);
    }

    public void ExitGame()
    {
        FadeOutTitle();
        Invoke(nameof(QuitGame), 1.0f);
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionLabel;
    [SerializeField] private TextMeshProUGUI titleLabel;
    [SerializeField] private CanvasGroup backgroundPanel;

    private int titleOriginalY;
    // Start is called before the first frame update
    void Start()
    {
        titleOriginalY = (int)titleLabel.rectTransform.anchoredPosition.y;
        versionLabel.text = "Version: " + GameManager.GAME_VERSION;

        // Make the title fade in, after 1 second
        titleLabel.CrossFadeAlpha(0, 0, true);
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
    }

    private void FadeInTitle()
    {
        titleLabel.CrossFadeAlpha(1, 1, false);
    }

    private void FadeOutTitle()
    {
        titleLabel.CrossFadeAlpha(0, 1, false);
    }
}

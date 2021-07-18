using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Sprite autoPlayOn;
    [SerializeField] private Sprite autoPlayOff;
    [SerializeField] private Animator panelAnimator;
    [SerializeField] private Image autoPlayImage;
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private GameObject btnRestart;


    private void Start()
    {
        FindObjectOfType<PlayerController>().onScore += OnScoreChange;
    }

    private void OnScoreChange()
    {
        int score = int.Parse(txtScore.GetParsedText().Trim());
        txtScore.SetText(++score + "");
    }

    public void OnSettingsClicked()
    {
        panelAnimator.SetBool("Open",!panelAnimator.GetBool("Open"));
    }

    public void OnAutoPlayClicked()
    {
        if (GameSettings.autoPlayOn)
        {
            autoPlayImage.sprite = autoPlayOff;
        }
        else
        {
            autoPlayImage.sprite = autoPlayOn;
        }

        GameSettings.autoPlayOn = !GameSettings.autoPlayOn;
        OnSettingsClicked();
    }

    public void OnPlayClicked()
    {
        playButton.gameObject.SetActive(false);
        txtScore.gameObject.SetActive(true);
        GameManager.Instance.StartGame();
    }

    public void OnRestartClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void EndGame()
    {
        btnRestart.SetActive(true);
    }
}

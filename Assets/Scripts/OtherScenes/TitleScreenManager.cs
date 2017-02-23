using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    public SoundManager     soundManager;

    public GameObject logo;
    public GameObject credits;

    public GameObject playButton;
    public GameObject backButton;
    public GameObject creditsButton;

    public Image    carImage;
    public Sprite   car0;
    public Sprite   car1;

    private void Start()
    {
        soundManager = SoundManager.GetInstance();
        soundManager.PlayBGM();
    }
    private void Update()
    {
        if (Time.timeSinceLevelLoad - Mathf.RoundToInt(Time.timeSinceLevelLoad) <= 0)
            carImage.sprite = car0;
        else
            carImage.sprite = car1;
    }
    public void PlayClicked()
    {
        soundManager.PlaySFX(SFXType.BUTTON_PRESS);
        SceneManager.LoadScene("TutorialScene");
    }
    public void CreditsClicked()
    {
        soundManager.PlaySFX(SFXType.BUTTON_PRESS);
        playButton.SetActive(false);
        credits.SetActive(true);
        backButton.SetActive(true);
        creditsButton.SetActive(false);
        logo.SetActive(false);
    }
    public void BackClicked()
    {
        soundManager.PlaySFX(SFXType.BUTTON_PRESS);
        playButton.SetActive(true);
        credits.SetActive(false);
        backButton.SetActive(false);
        creditsButton.SetActive(true);
        logo.SetActive(true);
    }
}

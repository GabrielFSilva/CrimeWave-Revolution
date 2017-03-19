using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    public SoundManager     soundManager;

    public GameObject logo;
    public GameObject credits;

    public GameObject titleContainer;
    public GameObject creditsContainer;

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
        if (PlayerPrefs.HasKey("SeenTutorial"))
            SceneManager.LoadScene("GameScene");
        else
        {
            PlayerPrefs.SetInt("SeenTutorial", 1);
            SceneManager.LoadScene("TutorialScene");
        }
    }
    public void TutorialClicked()
    {
        soundManager.PlaySFX(SFXType.BUTTON_PRESS);
        SceneManager.LoadScene("TutorialScene");
    }
    public void CreditsClicked()
    {
        soundManager.PlaySFX(SFXType.BUTTON_PRESS);
        titleContainer.SetActive(false);
        creditsContainer.SetActive(true);
    }
    public void ExitClicked()
    {
        soundManager.PlaySFX(SFXType.BUTTON_PRESS);
        Application.Quit();
    }
    public void BackClicked()
    {
        Debug.Log("here");
        soundManager.PlaySFX(SFXType.BUTTON_PRESS);
        titleContainer.SetActive(true);
        creditsContainer.SetActive(false);
    }
}

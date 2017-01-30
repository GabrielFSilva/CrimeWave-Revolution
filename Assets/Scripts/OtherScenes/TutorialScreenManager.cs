using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TutorialScreenManager : MonoBehaviour
{
    public List<GameObject> slides;
    public int slideIndex = 0;
    public Text nextButton;
    public Text returnButton;

    private void Start()
    {
        UpdateSlides();
        UpdateButtons();
    }
    public void UpdateSlides()
    {
        for(int i = 0; i < slides.Count; i ++)
            slides[i].SetActive(i == slideIndex ? true : false);
    }
    public void UpdateButtons()
    {
        returnButton.text = slideIndex == 0 ? "Home" : "Return";
        nextButton.text = slideIndex == slides.Count -1 ? "Play" : "Next";
    }
    public void NextButtonClicked()
    {
        if (slideIndex == slides.Count - 1)
            SceneManager.LoadScene("GameScene");
        else
        {
            slideIndex++;
            UpdateSlides();
            UpdateButtons();
        }
    }
    public void ReturnButtonClicked()
    {
        if (slideIndex == 0)
            SceneManager.LoadScene("TitleScreen");
        else
        {
            slideIndex--;
            UpdateSlides();
            UpdateButtons();
        }
    }

}

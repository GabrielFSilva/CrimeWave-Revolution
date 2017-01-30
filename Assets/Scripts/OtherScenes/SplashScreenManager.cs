using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    public Image logo;
    public float fadeDuration;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(StartFade());
	}
	
	IEnumerator StartFade()
    {
        yield return new WaitForSeconds(1f);
        float __alpha = 0f;
        while (__alpha < 1f)
        {
            __alpha += Time.deltaTime / fadeDuration;
            logo.color = new Color(1f, 1f, 1f, __alpha);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (__alpha > 0f)
        {
            __alpha -= Time.deltaTime / fadeDuration;
            logo.color = new Color(1f, 1f, 1f, __alpha);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("TitleScreen");
    }
}

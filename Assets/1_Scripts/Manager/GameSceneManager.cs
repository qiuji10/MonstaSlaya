using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public AudioData BGM;
    public GameObject crossFadeIn, crossFadeOut;

    private void Start()
    {
        crossFadeOut.SetActive(true);
        StartCoroutine(FadeOut());
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        switch (buildIndex)
        {
            case 0:
                AudioManager.instance.PlayBGM(BGM, "Menu");
                break;
            case 1:
                break;
            case 2:
                AudioManager.instance.PlayBGM(BGM, "GameOver");
                break;
            case 3:
                AudioManager.instance.PlayBGM(BGM, "Win");
                break;
            case 4:
                AudioManager.instance.PlayBGM(BGM, "Menu");
                break;
        }
    }

    public void SwitchScene(int indexBuild)
    {
        StartCoroutine(LoadGameScene(indexBuild));
    }

    public int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    IEnumerator LoadGameScene(int iB)
    {
        crossFadeIn.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(iB);
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.9f);
        crossFadeOut.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game is closed");
    }
}

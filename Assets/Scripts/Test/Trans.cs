using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Trans : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool DestroyManager = false;
    private void Start()
    {
        if (DestroyManager) 
        {
            Destroy(FindObjectOfType<GameManager>().gameObject);
        }
    }
    public void LoadScene(int number)
    {
        StartCoroutine(LoadSceneFunction(number));
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneFunction(int number)
    {

        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }

        Time.timeScale = 1;
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(number);
        yield return new WaitForSeconds(0.7f);
    }

    public void QuickLoad(int number)
    {
        SceneManager.LoadScene(number);
    }

}

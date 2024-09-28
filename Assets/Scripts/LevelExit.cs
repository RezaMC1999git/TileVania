using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(WaitAndLoadNextScene());
    }
    IEnumerator WaitAndLoadNextScene() 
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
        Load();
    }
    public void Load() 
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        if (FindObjectOfType<ScenePersist>())
            FindObjectOfType<ScenePersist>().DestroyME();
        if (currentScene != 4)
            SceneManager.LoadScene(currentScene + 1);
        else
        {
            SceneManager.LoadScene(0);
            FindObjectOfType<KillGameSession>().KillME();
        }
    }
    public void LoadMenu() 
    {
        SceneManager.LoadScene(0);
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
}

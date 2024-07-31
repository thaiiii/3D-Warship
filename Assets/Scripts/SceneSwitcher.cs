using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Load2NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void Load2PreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void playGame()
    {
        Invoke("LoadNextScene", 0.3f);    
    }
    public void tutorial()
    {
        Invoke("Load2NextScene", 0.3f);
    }
    public void backFromGame()
    {
        Invoke("LoadPreviousScene", 0.3f);     
    }
    public void backFromTutorial()
    {
        Invoke("Load2PreviousScene", 0.3f);
    }
    public void Reload()
    {
        Invoke("ReloadScene", 0.3f);    
    }
    public void quitGame() {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    
}

using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{

    public void PlayGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

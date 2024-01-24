using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Texture2D defaultMouseCursor;
    
    private void Awake()
    {
        Cursor.SetCursor(defaultMouseCursor, new Vector2(0, 0), CursorMode.Auto);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void StartTutorial()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

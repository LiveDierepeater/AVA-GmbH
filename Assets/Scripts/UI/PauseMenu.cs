using Task;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;

    private void Awake()
    {
        pauseMenu = transform.GetChild(0).gameObject;
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
                ContinueGame();
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                TaskManager.Instance.soundManager.PauseMenuMusic();
            }
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        TaskManager.Instance.soundManager.ResumeMusic();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

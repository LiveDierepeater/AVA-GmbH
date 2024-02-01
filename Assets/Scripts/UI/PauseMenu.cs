using UnityEngine;
using UnityEngine.SceneManagement;
using Task;

public class PauseMenu : MonoBehaviour
{
    public AudioClip PauseSFX;
    public AudioClip ResumeSFX;

    public Transform ControlsUI;
    
    private GameObject pauseMenu;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
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

                audioSource.clip = PauseSFX;
                audioSource.Play();
            }
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        ControlsUI.gameObject.SetActive(false);
        
        TaskManager.Instance.soundManager.ResumeMusic();
        
        audioSource.clip = ResumeSFX;
        audioSource.Play();
    }

    public void Controls()
    {
        if (ControlsUI.gameObject.activeSelf)
        {
            audioSource.clip = ResumeSFX;
            audioSource.Play();
            
            ControlsUI.gameObject.SetActive(false);
        }
        else
        {
            audioSource.clip = PauseSFX;
            audioSource.Play();
            
            ControlsUI.gameObject.SetActive(true);
        }
    }

    public void ExitToMainMenu()
    {
        audioSource.clip = ResumeSFX;
        audioSource.Play();
        
        Time.timeScale = 1;
        
        Invoke(nameof(LoadMainMenu), 0.2f);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

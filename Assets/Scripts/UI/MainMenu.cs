using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    public Texture2D defaultMouseCursor;
    public Transform ControlsUI;

    public AudioClip MenuSFX;
    public AudioClip MenuSFX_02;

    private AudioSource audioSource;
    
    private void Awake()
    {
        Cursor.SetCursor(defaultMouseCursor, new Vector2(0, 0), CursorMode.Auto);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = MenuSFX;
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        PlayMenuSFX();
        Invoke(nameof(LoadMainGame), 0.2f);
    }

    public void StartTutorial()
    {
        PlayMenuSFX();
        Invoke(nameof(LoadTutorial), 0.2f);
    }

    public void Controls()
    {
        PlayMenuSFX();
        
        if (ControlsUI.gameObject.activeSelf)
        {
            PlayMenuSFX02();
            ControlsUI.gameObject.SetActive(false);
        }
        else
        {
            PlayMenuSFX();
            ControlsUI.gameObject.SetActive(true);
        }
    }

    public void QuitGame()
    {
        PlayMenuSFX02();
        Invoke(nameof(CloseApplication), 0.5f);
    }

    private void LoadMainGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    private void LoadTutorial()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    private void CloseApplication()
    {
        Application.Quit();
    }
    
    private void PlayMenuSFX()
    {
        audioSource.clip = MenuSFX;
        audioSource.Play();
    }

    private void PlayMenuSFX02()
    {
        audioSource.clip = MenuSFX_02;
        audioSource.Play();
    }
}

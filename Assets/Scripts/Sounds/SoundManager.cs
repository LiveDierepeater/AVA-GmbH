using System.Collections.Generic;
using UnityEngine;
using Task;

public class SoundManager : MonoBehaviour
{
    public enum Menu
    {
        MainMenu,
        MainGame,
        Tutorial
    }

    public Menu menuType;

    public List<AudioClip> mainMenuClips = new List<AudioClip>();
    public List<AudioClip> mainGameClips = new List<AudioClip>();
    public List<AudioClip> tutorialClips = new List<AudioClip>();
    public AudioClip pauseMenu;

    private AudioSource audioSource;

    private int currentIndex;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        TaskManager.Instance.soundManager = this;
        
        switch (menuType)
        {
            case Menu.MainMenu:
                currentIndex = Random.Range(0, mainMenuClips.Count);
                audioSource.clip = mainMenuClips.ToArray()[currentIndex];
                audioSource.Play();
                break;
            
            case Menu.MainGame:
                currentIndex = Random.Range(0, mainGameClips.Count);
                audioSource.clip = mainGameClips.ToArray()[currentIndex];
                audioSource.Play();
                break;
            
            case Menu.Tutorial:
                currentIndex = Random.Range(0, tutorialClips.Count);
                audioSource.clip = tutorialClips.ToArray()[currentIndex];
                audioSource.Play();
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            audioSource.Pause();
        
        if (!audioSource.isPlaying)
        {
            switch (menuType)
            {
                case Menu.MainMenu:
                    currentIndex++;
                    
                    if (currentIndex == mainMenuClips.Count)
                    {
                        currentIndex = 0;
                        audioSource.clip = mainMenuClips.ToArray()[currentIndex];
                    }
                    else
                    {
                        audioSource.clip = mainMenuClips.ToArray()[currentIndex];
                    }
                    
                    audioSource.Play();
                    break;
                
                case Menu.MainGame:
                    currentIndex++;
                    
                    if (currentIndex == mainGameClips.Count)
                    {
                        currentIndex = 0;
                        audioSource.clip = mainGameClips.ToArray()[currentIndex];
                    }
                    else
                        audioSource.clip = mainGameClips.ToArray()[currentIndex];

                    audioSource.Play();
                    break;
                
                case Menu.Tutorial:
                    currentIndex++;
                    
                    if (currentIndex == tutorialClips.Count)
                    {
                        currentIndex = 0;
                        audioSource.clip = tutorialClips.ToArray()[currentIndex];
                    }
                    else
                        audioSource.clip = tutorialClips.ToArray()[currentIndex];
                    
                    audioSource.Play();
                    break;
            }
        }
    }

    public void ResumeMusic()
    {
        audioSource.clip = mainGameClips.ToArray()[currentIndex];
        audioSource.Play();
    }

    public void PauseMenuMusic()
    {
        audioSource.clip = pauseMenu;
        audioSource.Play();
    }
}

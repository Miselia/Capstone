using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundAudio : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource menu;
    [SerializeField] private AudioSource deckBuilder;
    [SerializeField] private AudioSource battle;
    void Awake()
    {
        if (GameObject.Find("BackgroundAudio") != transform.gameObject) Object.Destroy(transform.gameObject);
        DontDestroyOnLoad(transform.gameObject);
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenu") || SceneManager.GetActiveScene().name.Equals("DeckBuilderLobby") ||  SceneManager.GetActiveScene().name.Equals("LobbyScene") || SceneManager.GetActiveScene().name.Equals("Credits"))
        {
            if (!menu.isPlaying) menu.Play();
            if (deckBuilder.isPlaying) deckBuilder.Stop();
            if (battle.isPlaying) battle.Stop();
        }

        if (SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            if (menu.isPlaying) menu.Stop();
            if (deckBuilder.isPlaying) deckBuilder.Stop();
            if (!battle.isPlaying) battle.Play();
        }

        if (SceneManager.GetActiveScene().name.Equals("DeckBuilder"))
        {
            if (menu.isPlaying) menu.Stop();
            if (!deckBuilder.isPlaying) deckBuilder.Play();
            if (battle.isPlaying) battle.Stop();
        }
    }
}

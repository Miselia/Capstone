using Assets.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.Entities;
using UnityEngine.UI;

public class GameOverListener : MonoBehaviour, IGenericEventListener
{
    [SerializeField] private Button deckSelect;
    [SerializeField] private Button mainMenu;
    [SerializeField] private Text winnerText;
    private Game game;
    
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Start()
    {
        game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
        EventManager.instance.RegisterListener<GameOverEvent>(this);
        deckSelect.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        //winnerText.gameObject.SetActive(false);
    }
    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is GameOverEvent)
        {
            
            GameOverEvent ge = evt as GameOverEvent;
            World.Active.GetExistingSystem<DrawSystem>().Enabled = false;
            World.Active.GetExistingSystem<CollisionDetectionSystem>().Enabled = false;
            World.Active.GetExistingSystem<ControlSystem>().Enabled = false;
            World.Active.GetExistingSystem<DeletionSystem>().Enabled = false;
            World.Active.GetExistingSystem<MovementSystem>().Enabled = false;
            World.Active.GetExistingSystem<PlayerValueSystem>().Enabled = false;
            foreach (Entity e in World.Active.EntityManager.GetAllEntities())
            {
                World.Active.EntityManager.DestroyEntity(e);
            }
            deckSelect.gameObject.SetActive(true);
            mainMenu.gameObject.SetActive(true);
            //winnerText.gameObject.SetActive(true);
            string p1 = game.DeckGenres()[0];
            string p2 = game.DeckGenres()[1];
            if (ge.pID == 1)
            {
                winnerText.text = "Player 2 win!";
                EventManager.instance.QueueEvent(new SoundEvent(p1, "Loss"));
                EventManager.instance.QueueEvent(new AnimatorEvent(2, "Loss"));
                EventManager.instance.QueueEvent(new SoundEvent(p2, "Victory"));
                EventManager.instance.QueueEvent(new AnimatorEvent(1, "Victory"));

            }
            if (ge.pID == 2)
            {
                winnerText.text = "Player 1 win!";
                EventManager.instance.QueueEvent(new SoundEvent(p1, "Victory"));
                EventManager.instance.QueueEvent(new AnimatorEvent(1, "Victory"));
                EventManager.instance.QueueEvent(new SoundEvent(p2, "Loss"));
                EventManager.instance.QueueEvent(new AnimatorEvent(2, "Loss"));
            }


            return true;
        }
        return false;
    }
}

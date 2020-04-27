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


public class AnimatorListener: MonoBehaviour, IGenericEventListener
{
    // Start is called before the first frame update
    [SerializeField] private Animator fantasyAnimator;
    [SerializeField] private Animator horrorAnimator;
    [SerializeField] private Animator scifiAnimator;
    [SerializeField] private Animator steampunkAnimator;

    [SerializeField] private Animator fantasyAnimator2;
    [SerializeField] private Animator horrorAnimator2;
    [SerializeField] private Animator scifiAnimator2;
    [SerializeField] private Animator steampunkAnimator2;
    public string p1;
    public string p2;
    private Animator firstAnimator;
    private Animator secondAnimator;

    private void Start()
    {
        EventManager.instance.RegisterListener<AnimatorEvent>(this);
        if (p1 == "Fantasy") firstAnimator = fantasyAnimator;
        if (p1 == "Horror") firstAnimator = horrorAnimator;
        if (p1 == "Sci-Fi") firstAnimator = scifiAnimator;
        if (p1 == "Steampunk") firstAnimator = steampunkAnimator;
        firstAnimator.gameObject.SetActive(true);

        if (!SceneManager.GetActiveScene().name.Equals("DeckBuilder")) {
            if (p2 == "Fantasy") secondAnimator = fantasyAnimator2;
            if (p2 == "Horror") secondAnimator = horrorAnimator2;
            if (p2 == "Sci-Fi") secondAnimator = scifiAnimator2;
            if (p2 == "Steampunk") secondAnimator = steampunkAnimator2;
            secondAnimator.gameObject.SetActive(true);
        }
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is AnimatorEvent)
        {
            AnimatorEvent ae = evt as AnimatorEvent;
            int player = ae.pID;
            string action = ae.action;

            if (player == 1) firstAnimator.SetTrigger(action);
            if (player == 2) secondAnimator.SetTrigger(action);


            return true;
        }
        return false;
    }
}

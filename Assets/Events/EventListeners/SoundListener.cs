﻿using Assets.Resources;
//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.Entities;

public class SoundListener : MonoBehaviour, IGenericEventListener
{
    [SerializeField] private List<AudioSource> library;
    private void Start()
    {
        EventManager.instance.RegisterListener<SoundEvent>(this);
    }
    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is SoundEvent)
        {

            SoundEvent se = evt as SoundEvent;
            if (se.sound == 1) library[se.sound].pitch = Random.Range(1.0f - 0f, 1.0f + 0.5f);
            if (se.sound == 0) library[se.sound].pitch = Random.Range(1.0f - 0f, 1.0f + 0.5f);
            library[se.sound].Play();
            return true;
        }
        return false;
    }
}

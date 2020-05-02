using Assets.Resources;
//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.Entities;
using UnityEngine.Audio;

public class SoundListener : MonoBehaviour, IGenericEventListener
{
    public Sound[] sounds;
    private Dictionary<string, Dictionary<string, List<Sound>>> library;
    void Awake ()
    {
        library = new Dictionary<string, Dictionary<string, List<Sound>>>();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            //Debug.Log("Genre: " + s.genre + " Type: " + s.type);
            if (library.ContainsKey(s.genre))
            {
                if (library[s.genre].ContainsKey(s.type))
                {
                    library[s.genre][s.type].Add(s);
                    Debug.Log("Adding Sound");
                }
                else
                {
                    library[s.genre].Add(s.type, new List<Sound>());
                    library[s.genre][s.type].Add(s);
                    Debug.Log("Adding Type");
                }
            }
            else
            {
                library.Add(s.genre, new Dictionary<string, List<Sound>>());
                library[s.genre].Add(s.type, new List<Sound>());
                library[s.genre][s.type].Add(s);
                Debug.Log("Adding Genre " + s.genre);
            }
        }
    }
    private void Start()
    {
        EventManager.instance.RegisterListener<SoundEvent>(this);
        Debug.Log(library.ToString());
    }
    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is SoundEvent)
        {

            SoundEvent se = evt as SoundEvent;
            if (library.ContainsKey(se.genre) && library[se.genre].ContainsKey(se.type))
            {
                int max = library[se.genre][se.type].Count;
                int rand = Random.Range(0, max);

            // if (se.sound == 1) library[se.sound].pitch = Random.Range(1.0f - 0f, 1.0f + 0.5f);
            //if (se.sound == 0) library[se.sound].pitch = Random.Range(1.0f - 0f, 1.0f + 0.5f);
            //Debug.Log(library[se.genre][se.type][0].source.ToString());
            if(se.delay!=0) library[se.genre][se.type][rand].source.PlayDelayed(se.delay);
            else library[se.genre][se.type][rand].source.Play();
            return true;
        }
        return false;
    }
}

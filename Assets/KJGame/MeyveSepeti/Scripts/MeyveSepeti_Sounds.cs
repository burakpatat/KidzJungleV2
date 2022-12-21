using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeyveSepeti_Sounds : MonoBehaviour
{
    public static MeyveSepeti_Sounds aManager;
    public AudioClip[] sounds;
    public AudioSource aSource;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (aManager == null)
        {
            aManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
       
    }

    public void TrueSound()
    {
        aSource.PlayOneShot(sounds[0]);
       
    }
    public void FalseSound()
    {
        aSource.PlayOneShot(sounds[1]);
       
    }
    public void ButtonClickSound()
    {
        aSource.PlayOneShot(sounds[2]);
        
    }
    public void FruitSliceSound()
    {
       aSource.PlayOneShot(sounds[3]);
        
    }
    public void FruitSceneSound(AudioClip aClip)
    {
        if (aSource.isPlaying)
        {
           aSource.Stop();

        }
         aSource.clip = aClip;

        
        
        aSource.Play();

    }
    
}

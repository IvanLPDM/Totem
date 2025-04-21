using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip ground_sound;

    public void Play_ground_sound()
    {
        audioSource.PlayOneShot(ground_sound);
    }
}

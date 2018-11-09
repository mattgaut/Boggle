using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour {


    [SerializeField] AudioSource click_sound, buzz_sound, accept_sound;

    public void PlayBuzz() {
        buzz_sound.Stop();

        buzz_sound.Play();
    }

    public void PlayClick() {
        click_sound.Stop();

        click_sound.Play();
    }

    public void PlayAccept() {
        accept_sound.Stop();

        accept_sound.Play();
    }
}

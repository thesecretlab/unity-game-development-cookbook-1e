using UnityEngine;
using System.Collections.Generic;

// BEGIN audio_manager
public class AudioManager : Singleton<AudioManager> {
    
    // The list of references to SoundEffect assets.
    public SoundEffect[] effects;

    // A dictionary that maps the names of SoundEffects to the
    // objects themselves, to make it faster to look them up.
    private Dictionary<string, SoundEffect> _effectDictionary;

    // A reference to the current audio listener, which we use
    // to place audio clips.
    private AudioListener _listener;

    // BEGIN audio_code_examples_holder
    private void AudioPlayingDemo() {
        // The code in this method is never called; it exists so that
        // we can bring it into the book using our snippet system.

        AudioSource source = null;
        AudioClip clip = null;
        
        // BEGIN audio_code_examples
        // Play the AudioClip that's currently set
        source.Play();

        // Play the AudioClip with a delay, measured in seconds
        source.PlayDelayed(0.5f);

        // Play a specified AudioClip; the volume scale is optional
        source.PlayOneShot(clip, 0.5f); // play at half volume
        source.PlayOneShot(clip, 1f); // play at full volume

        // Stop playing the current clip
        source.Stop();
        // END audio_code_examples

        // BEGIN audio_manager_examples
        // Play a sound called "laser" at the same place as the listener
        AudioManager.instance.PlayEffect("laser");
        
        // Play the same sound at the origin
        AudioManager.instance.PlayEffect("laser", Vector3.zero);
        // END audio_manager_examples
    }
    // END audio_code_examples_holder

    private void Awake() {
        // When the manager wakes up, build a dictionary of named
        // sounds, so that we can quickly access them when needed
        _effectDictionary = new Dictionary<string, SoundEffect>();
        foreach (var effect in effects) {
            Debug.LogFormat("registered effect {0}", effect.name);
            _effectDictionary[effect.name] = effect;
        }

    }

    // Plays a sound effect by name, at the same position as the 
    // audio listener.
    public void PlayEffect(string effectName) {
        // If we don't currently have a listener (or the reference we
        // had was destroyed), find one to use
        if (_listener == null) {
            _listener = FindObjectOfType<AudioListener>();
        }

        // Play the effect at the listener's position
        PlayEffect(effectName, _listener.transform.position);
        
    }

    // Plays a sound effect by name, at a specified position in the world
    public void PlayEffect(string effectName, Vector3 worldPosition) {

        // Does the sound effect exist?
        if (_effectDictionary.ContainsKey(effectName) == false) {
            Debug.LogWarningFormat("Effect {0} is not registered.", effectName);
            return;
        }

        // Get a clip from the effect
        var clip = _effectDictionary[effectName].GetRandomClip();

        // Make sure it wasn't null
        if (clip == null) {
            Debug.LogWarningFormat("Effect {0} has no clips to play.", effectName);
            return;
        }

        // Play the selected clip at the specified point
        AudioSource.PlayClipAtPoint(clip, worldPosition);
        
    }

}
// END audio_manager
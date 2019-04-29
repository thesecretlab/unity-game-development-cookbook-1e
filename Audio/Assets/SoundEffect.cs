using UnityEngine;

// BEGIN soundeffect_asset
// An asset that contains a collection of audio clips.
[CreateAssetMenu]
public class SoundEffect : ScriptableObject {
    
    // The list of AudioClips that might be played when
    // this sound effect is played.
    public AudioClip[] clips;

    // Randomly selects an AudioClip from the 'clips' array,
    // if one is available.
    public AudioClip GetRandomClip() {
        if (clips.Length == 0) {
            return null;
        }
        return clips[Random.Range(0, clips.Length)];
    }
}
// END soundeffect_asset
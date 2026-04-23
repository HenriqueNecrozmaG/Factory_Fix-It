using JetBrains.Annotations;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    private bool playJetSound;

    private Player playerScript;
    private AudioSource aus;

    void Start()
    {
        playerScript = GetComponent<Player>();
        aus = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerScript.Direction() != 0 && Player.isHoverPowerUp && !playJetSound)
        {
            aus.clip = audioClips[0];
            aus.Play();
            playJetSound = true;
        }
        else if (playerScript.Direction() == 0 || !Player.isHoverPowerUp)
        {
            playJetSound = false;
        }
    }

    public void PlayPlayerAudio(int i)
    {
        aus.clip = audioClips[i];
        aus.Play();
    }
}

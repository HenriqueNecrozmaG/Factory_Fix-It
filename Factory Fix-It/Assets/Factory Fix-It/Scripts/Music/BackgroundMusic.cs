using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] musics;
    private bool defeatMusicPlaying;

    private AudioSource aus;
    private Player playerScript;

    void Start()
    {
        aus = GetComponent<AudioSource>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        aus.clip = musics[0];
        aus.Play();
        aus.loop = true;
    }

    void Update()
    {
        if (playerScript.isDead && !defeatMusicPlaying)
        {
            aus.clip = musics[1];
            aus.Play();
            aus.loop = false;
            defeatMusicPlaying = true;
        }
    }
}

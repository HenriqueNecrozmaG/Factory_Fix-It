using UnityEngine;

public class BackgroundMusicBoss : MonoBehaviour
{
    [SerializeField] private AudioClip[] musics;
    private bool defeatMusicPlaying;
    private bool victoryMusicPlaying;

    private AudioSource aus;
    private Player playerScript;
    private BossScript bossScript;

    void Start()
    {
        aus = GetComponent<AudioSource>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        bossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>();
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
        else if (bossScript.isDefeated && !victoryMusicPlaying)
        {
            aus.clip = musics[2];
            aus.Play();
            aus.loop = false;
            victoryMusicPlaying = true;
        }
    }
}

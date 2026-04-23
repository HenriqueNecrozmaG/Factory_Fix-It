using System.Collections;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private Player playerScript;
    [SerializeField] private AudioClip[] audioClips;

    private AudioSource aus;
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        aus = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerScript.isThrown)
        {
            transform.parent = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && playerScript.isThrown)
        {
            StartCoroutine(Destroy());
            aus.clip = audioClips[0];
            aus.Play();
            playerScript.isThrown = false;
            if (collision.gameObject.tag == "Crate")
            {
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                Destroy(collision.gameObject);
                aus.clip = audioClips[1];
                aus.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SmasherBase" && collision.gameObject.tag == "BossBase")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Destroy()
    {
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}

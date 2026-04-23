using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    private Player playerScript;
    private AudioSource aus;
    private SpriteRenderer sr;
    private Collider2D col;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        aus = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && playerScript.isThrown)
        {
            playerScript.isThrown = false;
            if (collision.gameObject.tag == "Crate")
            {
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Door")
            {
                StartCoroutine(Open());
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SmasherBase")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Open()
    {
        aus.Play();
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}

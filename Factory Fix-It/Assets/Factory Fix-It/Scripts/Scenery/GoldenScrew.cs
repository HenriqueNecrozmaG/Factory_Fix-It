using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenScrew : MonoBehaviour
{
    public static float collectedGoldenScrews;
    private AudioSource aus;
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        aus = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        col = sr.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(Collect());
        }
    }

    IEnumerator Collect()
    {
        collectedGoldenScrews++;
        aus.Play();
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}

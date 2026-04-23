using System.Collections;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] private float attackTime;
    [SerializeField] private BossPivot bossPivot;
    [SerializeField] private Canvas canvasVictory;
    public float life;
    public bool isAttacking;
    public bool isDizzy;
    public bool isDefeated;
    private bool canBeDefeated = true;

    [SerializeField] private AudioClip[] audioClips;
    private bool playAttackSound;

    private Animator anim;
    private AudioSource aus;

    void Start()
    {
        anim = GetComponent<Animator>();
        aus = GetComponent<AudioSource>();

        StartCoroutine(Attack());
        life = 10;
        canvasVictory.enabled = false;
    }
    
    void Update()
    {
        anim.SetBool("pAttacking", isAttacking);
        anim.SetBool("pDizzy", isDizzy);
        anim.SetBool("pDefeated", isDefeated);

        if (life <= 0 && canBeDefeated)
        {
            StartCoroutine(Defeat());
            canBeDefeated = false;
        }

        if (isAttacking && !playAttackSound)
        {
            aus.clip = audioClips[0];
            aus.Play();
            playAttackSound = true;
        }
        else if (!isAttacking)
        {
            playAttackSound = false;
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(Random.Range(1, attackTime + 1));

        if (!bossPivot.isAwake || isDizzy)
        {
            yield return null;
        }
        else if (bossPivot.isAwake && !isDizzy)
        {
            isAttacking = true;

            yield return new WaitForSeconds(3);

            isAttacking = false;
        }

        StartCoroutine(Attack());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Crate")
        {
            StartCoroutine(Stunned());
        }
    }

    IEnumerator Stunned()
    {
        isDizzy = true;
        aus.clip = audioClips[1];
        aus.Play();

        yield return new WaitForSeconds(5);

        isDizzy = false;
    }

    public float LifeLoss()
    {
        life--;
        return life;
    }

    IEnumerator Defeat()
    {
        life = 0;
        isDizzy = false;
        isDefeated = true;

        yield return new WaitForSeconds(5);

        Time.timeScale = 0;
        canvasVictory.enabled = true;
    }
}

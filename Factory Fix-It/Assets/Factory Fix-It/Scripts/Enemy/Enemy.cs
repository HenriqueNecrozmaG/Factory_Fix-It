using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float wakeUpDistance;

    private AudioSource aus;

    private Transform playerPosition;
    private bool isAwake;
    //private bool canAwake = true;
    private Animator anim;
    private bool sirenSoundPlay;

    void Start()
    {
        if (playerPosition == null)
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerPosition == null)
            {
                Debug.Log("Erro: Inimigo não encontrou o player");
            }
        }

        anim = GetComponent<Animator>();
        aus = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerPosition == null)
        {
            return;
        }
        
        if (!isAwake)
        {
            isAwake = WakeUp(playerPosition.position, transform.position, wakeUpDistance);
        }

        if (isAwake)
        {
            OnMove();
        }

        anim.SetBool("pAwake", isAwake);

        if (isAwake && !sirenSoundPlay)
        {
            aus.Play();
            sirenSoundPlay = true;
        }
    }

    private bool WakeUp(Vector2 p, Vector2 e, float d)
    {
        return Vector2.Distance (p, e) <= d;
    }

    private void OnMove()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void Invert()
    {
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else if (transform.eulerAngles.y == 180)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Invert();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Invert")
        {
            Invert();
        }

        if (collision.gameObject.tag == "SmasherBase")
        {
            Destroy(gameObject);
        }
    }
}

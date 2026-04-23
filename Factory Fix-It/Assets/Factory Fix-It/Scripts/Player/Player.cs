using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float speed;
    private Vector2 direction;

    [Header("Stairs and Physics Settings")]
    [SerializeField] private float gravity;
    [SerializeField] private float speedClimbStairs;
    [SerializeField] private float exitStairsForce;
    public bool onStairs;
    private float directionClimbStairs;

    [Header("Color Settings")]
    [SerializeField] private Color32 normalColor;
    [SerializeField] private Color32 damagedColor;
    
    [Header("Hover Settings")]
    [SerializeField] private Transform sensorGround;
    [SerializeField] private float sensorGroundRadius;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private float hoverTime;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float hoverGravity;
    public static bool isHoverPowerUp;
    public bool canHover;
    public bool onHover;

    [Header("Object Settings")]
    [SerializeField] private Transform sensorObject;
    [SerializeField] private float sensorObjectRadius;
    [SerializeField] private Transform objectPosition;
    [SerializeField] private float throwForce;
    [SerializeField] private LayerMask layerObject;
    public GameObject pickedObject;
    public Rigidbody2D objectRb;
    public bool canPick;
    public bool isPicked = false;
    public bool isThrown = false;

    [Header("Life and Damage Settings")]
    [SerializeField] private float damageTime;
    [SerializeField] private Canvas canvasDefeat;
    public static float life = 2;
    public static bool isDamaged;
    public bool isDead;
    private bool canBeDamaged = true;

    [Header("Other Settings")]
    [SerializeField] private string bossSceneName;
    private PlayerAudioManager playerAudioScript;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        playerAudioScript = GetComponent<PlayerAudioManager>();
        
        rb.gravityScale = gravity;

        normalColor = new Color32(normalColor.r, normalColor.g, normalColor.b, 255);
        damagedColor = new Color32(damagedColor.r, damagedColor.g, damagedColor.b, 255);

        if (!isDamaged)
        {
            sr.color = normalColor;
        }
        else if (!isDamaged)
        {
            sr.color = damagedColor;
        }

        isDead = false;
        canvasDefeat.enabled = false;
    }
    
    void Update()
    {
        Move();
        Invert();
        OnStairs();
        OnGround();
        OnPickUpRadius();

        if (OnGround() && isHoverPowerUp)
        {
            canHover = true;
        }
        else if (!OnGround() && canHover && !onStairs)
        {
            StartCoroutine(Hover(hoverTime));
            canHover = false;
        }
        else if (!isHoverPowerUp)
        {
            canHover = false;
        }

        if (OnGround() && onHover)
        {
            rb.gravityScale = gravity;
            onHover = false;

            StopCoroutine(Hover(hoverTime));
        }

        if (isPicked)
        {
            canPick = false;
        }

        if (Input.GetButtonDown("Jump") && !onStairs)
        {
            if (canPick && !isPicked)
            {
                PickUp();
            }
            else if (isPicked)
            {
                ThrowObject();
            }
        }

        if (isPicked && pickedObject != null)
        {
            pickedObject.transform.position = objectPosition.position;
        }
        else if (pickedObject == null)
        {
            isPicked = false;
            canPick = true;
        }

        if (life <= 0 && !isDead)
        {
            Destroy(gameObject);
            Time.timeScale = 0;
            isDead = true;
            canvasDefeat.enabled = true;
        }
    }

    private void Move()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
    }

    private void Invert()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.0f && transform.eulerAngles.y == 180.0f)
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 0.0f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0.0f && transform.eulerAngles.y == 0.0f)
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 180.0f);
        }
    }

    void FixedUpdate()
    {
        OnMove();
    }

    private void OnMove()
    {
        if (!onHover)
        {
            rb.linearVelocity = new Vector2(speed * direction.x, rb.linearVelocity.y);
        }
        else if (onHover)
        {
            rb.linearVelocity = new Vector2(hoverSpeed * direction.x, rb.linearVelocity.y);
        }
    }

    private void OnStairs()
    {
        directionClimbStairs = Input.GetAxisRaw("Vertical");

        if (onStairs && directionClimbStairs != 0.0f)
        {
            transform.Translate(Vector2.up * directionClimbStairs * speedClimbStairs * Time.deltaTime);
        }
    }

    IEnumerator Hover(float t)
    {
        rb.gravityScale = hoverGravity;
        onHover = true;

        yield return new WaitForSeconds(t);

        rb.gravityScale = gravity;
        onHover = false;
    }

    private bool OnPickUpRadius()
    {
        canPick = Physics2D.OverlapCircle(sensorObject.position, sensorObjectRadius, layerObject);
        return canPick;
    }

    private void PickUp()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll
            (transform.position, sensorObjectRadius);
        
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Crate") || col.CompareTag("Key"))
            {
                pickedObject = col.gameObject;
                objectRb = pickedObject.GetComponent<Rigidbody2D>();

                objectRb.bodyType = RigidbodyType2D.Kinematic;
                objectRb.linearVelocity = Vector2.zero;

                objectRb.transform.position = sensorObject.position;
                objectRb.transform.parent = sensorObject;

                isPicked = true;
                break;
            }
        }
    }


    private void ThrowObject()
    {
        if (pickedObject != null)
        {
            pickedObject.transform.parent = null;
            objectRb.bodyType = RigidbodyType2D.Dynamic;

            Vector2 throwDir = transform.eulerAngles.y == 180 ? Vector2.left : Vector2.right;
            objectRb.AddForce(throwDir * throwForce, ForceMode2D.Impulse);

            pickedObject = null;
            objectRb = null;
            isPicked = false;
            isThrown = true;
        }
    }

    public bool OnGround()
    {
        return Physics2D.OverlapCircle(sensorGround.position, sensorGroundRadius, layerGround);      
    }

    IEnumerator Damage()
    {
        canBeDamaged = false;

        if (isHoverPowerUp)
        {
            isHoverPowerUp = false;
            life = 2;
        }
        else if (!isHoverPowerUp && !isDamaged)
        {
            isDamaged = true;
            life = 1;
        }
        else if (life == 1)
        {
            life = 0;
        }
        
        if (isPicked)
        {
            ThrowObject();
        }

        if (!isDamaged)
        {
            sr.color = new Color32(normalColor.r, normalColor.g, normalColor.b, 100);
        }
        else if (isDamaged)
        {
            sr.color = new Color32(damagedColor.r, damagedColor.g, damagedColor.b, 100);
        }

        col.isTrigger = true;

        yield return new WaitForSeconds(damageTime);

        canBeDamaged = true;

        if (!isDamaged)
        {
            sr.color = normalColor;
        }
        else if (isDamaged)
        {
            sr.color = damagedColor;
        }

        col.isTrigger = false;
    }

    public float Direction()
    {
        return direction.x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (canBeDamaged)
            {
                StartCoroutine(Damage());
                playerAudioScript.PlayPlayerAudio(1);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform" && Input.GetAxisRaw("Vertical") < 0.0f)
        {
            if (onStairs)
            {
                collision.gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = 180.0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Platform")
        {
            collision.gameObject.GetComponent<PlatformEffector2D>().rotationalOffset = 0.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HoverPowerUp")
        {
            if (!isHoverPowerUp)
            {
                isDamaged = false;
                isHoverPowerUp = true;
                life = 3;
                Destroy(collision.gameObject);
                sr.color = normalColor;
                playerAudioScript.PlayPlayerAudio(2);
            }
        }

        if (collision.tag == "HealthUp")
        {
            if (!isHoverPowerUp && isDamaged)
            {
                isDamaged = false;
                life = 2;
                Destroy(collision.gameObject);
                sr.color = normalColor;
                playerAudioScript.PlayPlayerAudio(2);
            }
        }

        if (collision.tag == "SmasherBase" || collision.tag == "BossBase")
        {
            Destroy(gameObject);
            isDead = true;
            canvasDefeat.enabled = true;
            Time.timeScale = 0;
        }

        if (collision.tag == "Void")
        {
            Destroy(gameObject);
            isDead = true;
            canvasDefeat.enabled = true;
            Time.timeScale = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Stairs")
        {
            rb.gravityScale = 0.0f;
            onStairs = true;
        }
        else if (collision.tag == "ExitStairs" && directionClimbStairs > 0.0f)
        {
            rb.AddForce(Vector2.up * exitStairsForce, ForceMode2D.Impulse);
        }
        else if (collision.tag == "BossDoor")
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                StartCoroutine(EnterBossRoom());
            }
        }
    }

    IEnumerator EnterBossRoom()
    {
        playerAudioScript.PlayPlayerAudio(3);

        yield return new WaitForSeconds(0.8f);

        SceneManager.LoadScene(bossSceneName);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Stairs")
        {
            rb.gravityScale = gravity;
            onStairs = false;
        }
    }
}

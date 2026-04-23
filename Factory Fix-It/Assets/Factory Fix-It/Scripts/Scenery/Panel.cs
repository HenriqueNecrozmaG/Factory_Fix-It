using UnityEngine;

public class Panel : MonoBehaviour
{
    private BossScript bossScript;
    public bool canBePressed;
    public bool activation;
    public bool onPanel;

    [SerializeField] private Color32 deactivatedColor;
    [SerializeField] private Color32 activatedColor;

    private SpriteRenderer sr;
    private AudioSource aus;

    void Start()
    {
        bossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>();
        sr = GetComponent<SpriteRenderer>();
        aus = GetComponent<AudioSource>();

        canBePressed = false;
        activation = true;

        deactivatedColor = new Color32(deactivatedColor.r, deactivatedColor.g, deactivatedColor.b, 255);
        activatedColor = new Color32(activatedColor.r, activatedColor.g, activatedColor.b, 255);
    }

    void Update()
    {
        if (activation && bossScript.isDizzy)
        {
            canBePressed = true;
            activation = false;
        }
        else if (!bossScript.isDizzy)
        {
            canBePressed = false;
            activation = true;
        }

        if (!canBePressed)
        {
            sr.color = deactivatedColor;
        }
        else if (canBePressed)
        {
            sr.color = activatedColor;
        }

        if (Input.GetButtonDown("Fire1") && onPanel && canBePressed)
        {
            bossScript.LifeLoss();
            canBePressed = false;
            aus.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            onPanel = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            onPanel = false;
        }
    }
}

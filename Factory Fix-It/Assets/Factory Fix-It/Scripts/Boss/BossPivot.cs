 using UnityEngine;

public class BossPivot : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private GameObject bossWall;
    [SerializeField] private Transform bossWallPosition;
    [SerializeField] private float wakeUpDistance;
    private BossScript bossScript;
    private CameraManager cameraScript;
    public bool isAwake;
    private bool isBossWall;

    void Start()
    {
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
        bossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>();
    }

    void Update()
    {
        if (playerPosition == null)
        {
            return;
        }

        if (WakeUp(playerPosition.position, transform.position, wakeUpDistance))
        {
            isAwake = true;
            cameraScript.leftLimit = 17;

            if (!isBossWall)
            {
                Instantiate(bossWall);
                bossWall.transform.position = bossWallPosition.position;
                isBossWall = true;
            }
        }

        if (isAwake && !bossScript.isAttacking && !bossScript.isDizzy && !bossScript.isDefeated)
        {
            OnMove();
        }
    }

    private bool WakeUp(Vector2 p, Vector2 e, float d)
    {
        return Vector2.Distance(p, e) <= d;
    }

    private void OnMove()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void Turn()
    {
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 180);
        }
        else if (transform.eulerAngles.y == 180)
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Turn();
        }
    }
}

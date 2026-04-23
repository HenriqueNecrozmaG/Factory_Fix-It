using UnityEngine;
using UnityEngine.UI;

public class UIBoss : MonoBehaviour
{
    private Animator animBossHealth;
    [SerializeField] private BossScript bossScript;

    void Start()
    {
        animBossHealth = GetComponent<Animator>();
    }

    void Update()
    {
        animBossHealth.SetInteger("pLife", (int)bossScript.life);
    }
}

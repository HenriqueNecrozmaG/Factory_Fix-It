using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Player playerScript;
    private Animator anim;

    void Start()
    {
        playerScript = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        anim.SetInteger("pMove", (int)playerScript.Direction());
        anim.SetBool("pHoverPowerUp", Player.isHoverPowerUp);
        anim.SetBool("pDamaged", Player.isDamaged);
        anim.SetBool("pHolding", playerScript.isPicked);
        anim.SetBool("pStairs", playerScript.onStairs);
    }
}

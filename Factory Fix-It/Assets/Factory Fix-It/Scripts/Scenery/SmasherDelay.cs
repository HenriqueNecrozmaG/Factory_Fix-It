using System.Collections;
using UnityEngine;

public class SmasherDelay : MonoBehaviour
{
    [SerializeField] private float delayTime;
    private bool canPress;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Delay(delayTime));
    }

    
    void Update()
    {
        anim.SetBool("pPress", canPress);   
    }

    IEnumerator Delay(float t)
    {
        yield return new WaitForSeconds(t);

        canPress = true;
    }
}

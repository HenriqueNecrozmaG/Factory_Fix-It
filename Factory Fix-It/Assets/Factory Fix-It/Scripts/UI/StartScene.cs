using System.Collections;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    private AudioSource aus;

    void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    public void CloseGame()
    {
        StartCoroutine(ButtonPress());
    }

    IEnumerator ButtonPress()
    {
        aus.Play();

        yield return new WaitForSeconds(0.5f);

        Application.Quit();
    }
}

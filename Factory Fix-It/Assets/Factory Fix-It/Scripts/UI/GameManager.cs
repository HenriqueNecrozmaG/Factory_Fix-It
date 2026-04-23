using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private AudioSource aus;

    void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    public void OpenScene()
    {
        StartCoroutine(ButtonPress());
    }

    IEnumerator ButtonPress()
    {
        aus.Play();
        GoldenScrew.collectedGoldenScrews = 0;
        Player.life = 2;
        Player.isHoverPowerUp = false;
        Player.isDamaged = false;
        Time.timeScale = 1;

        yield return new WaitForSeconds(0.5f);


        SceneManager.LoadScene(sceneName);
    }
}

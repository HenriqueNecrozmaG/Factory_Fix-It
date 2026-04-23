using UnityEngine;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    [SerializeField] string nextScene;

    void Update()
    {
        LoadScene();
    }

    void LoadScene()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
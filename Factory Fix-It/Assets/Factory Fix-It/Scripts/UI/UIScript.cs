using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private Image[] imageScrew;

    void Update()
    {
        if (GoldenScrew.collectedGoldenScrews == 0)
        {
            imageScrew[0].enabled = false;
            imageScrew[1].enabled = false;
            imageScrew[2].enabled = false;
        }
        else if (GoldenScrew.collectedGoldenScrews == 1)
        {
            imageScrew[0].enabled = true;
            imageScrew[1].enabled = false;
            imageScrew[2].enabled = false;
        }
        else if (GoldenScrew.collectedGoldenScrews == 2)
        {
            imageScrew[0].enabled = true;
            imageScrew[1].enabled = true;
            imageScrew[2].enabled = false;
        }
        else if (GoldenScrew.collectedGoldenScrews == 3)
        {
            imageScrew[0].enabled = true;
            imageScrew[1].enabled = true;
            imageScrew[2].enabled = true;
        }
    }
}

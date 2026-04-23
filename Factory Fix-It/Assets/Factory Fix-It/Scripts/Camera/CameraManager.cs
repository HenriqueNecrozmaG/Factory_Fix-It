using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    private Transform target;
    [SerializeField] private float speed;
    public float leftLimit, rightLimit, upLimit, bottomLimit;

    private float x;
    private float y;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (target == null)
        {
            Debug.Log("Erro. Player não esta atribuido a câmera.");            
        }
    }

    void Update()
    {
        if (target == null) return;

        CameraFollow();
    }

    void CameraFollow()
    {
        // LIMITA O VALOR DE X Y
        x = Mathf.Clamp(target.position.x, leftLimit, rightLimit);
        y = Mathf.Clamp(target.position.y, bottomLimit, upLimit);

        // APLICA O VALOR E MOVIMENTO DA CAMERA
        Vector3 cameraPosition = new Vector3(x, y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, cameraPosition, speed * Time.deltaTime);
    }
}

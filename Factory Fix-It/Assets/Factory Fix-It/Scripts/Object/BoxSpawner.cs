using System.Collections;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private Transform boxPosition;
    [SerializeField] private float spawnTime;

    void Start()
    {
        StartCoroutine(Spawn(spawnTime));
    }

    IEnumerator Spawn(float t)
    {
        yield return new WaitForSeconds(t);

        Instantiate(boxPrefab, boxPosition.position, transform.rotation);

        StartCoroutine(Spawn(spawnTime));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static List<GameObject> boxSpawned = new List<GameObject>();

    public GameObject cube;
    public int maxCubes;

    public Material red;
    public Material blue;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    IEnumerator SpawnCubes()
    {
        for (int i = 0; i < maxCubes; i++)
        {
            GameObject box = Instantiate(cube, new Vector3(12f, 2f, UnityEngine.Random.Range(-10f, 10f)), Quaternion.identity);
            int random = UnityEngine.Random.Range(0, 10);
            box.GetComponent<MeshRenderer>().material = random <= 4f ? red : blue;
            box.name = random <= 4f ? "Red" : "Blue";
            Destroy(box, 120f);
            boxSpawned.Add(box);
        }

        yield return new WaitForSeconds(20f);

        StartCoroutine(SpawnCubes());

    }
}

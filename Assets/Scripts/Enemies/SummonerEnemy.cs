using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerEnemy : MonoBehaviour
{
    public GameObject miniSpherePrefab;
    public float spawnRate = 3f;

    void Start() { InvokeRepeating("Summon", spawnRate, spawnRate); }

    void Summon()
    {
        Instantiate(miniSpherePrefab, transform.position + Vector3.up, Quaternion.identity);
    }
}
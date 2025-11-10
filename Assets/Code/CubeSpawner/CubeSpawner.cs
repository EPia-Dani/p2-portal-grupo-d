using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [Header("Refs")]
    public Transform spawnPoint;
    public GameObject cubePrefab;

    [Header("Rules")]
    public int maxAlive = 3;
    public float spawnCooldown = 1.0f;
    public bool alignToSpawnRotation = true;

    [Header("Sound")]
    public AudioSource spawnSound;

    private float lastSpawnTime = -999f;
    private readonly List<GameObject> alive = new List<GameObject>();


    public void SpawnOnce()
    {

        if (Time.time - lastSpawnTime < spawnCooldown) return;

        alive.RemoveAll(go => go == null);

        if (alive.Count >= maxAlive) return;
        if (cubePrefab == null || spawnPoint == null) return;

        Quaternion rot = alignToSpawnRotation ? spawnPoint.rotation : Quaternion.identity;
        GameObject cube = Instantiate(cubePrefab, spawnPoint.position, rot);

        cube.tag = "Cube";

        if (!cube.TryGetComponent<Rigidbody>(out _))
        {
            var rb = cube.AddComponent<Rigidbody>();
            rb.mass = 20f;
        }
        if (!cube.TryGetComponent<Collider>(out _))
        {
            var bc = cube.AddComponent<BoxCollider>();
            bc.material = null;
        }

        alive.Add(cube);

        if (spawnSound != null) spawnSound.Play();

        lastSpawnTime = Time.time;
    }

    public void Unregister(GameObject cube)
    {
        alive.Remove(cube);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(spawnPoint.position, new Vector3(0.6f, 0.6f, 0.6f));
        Gizmos.DrawRay(spawnPoint.position, spawnPoint.forward * 0.4f);
    }
}
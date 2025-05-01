using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{

    //Public Fields
    public GameObject Map;
    public float radiusOfFoodSource;
    public GameObject foodPrefab;
    public int foodCount;
    public bool maintainAmount;
    int blobCount = 3; //Do not modify
    public int seed;

    System.Random prng;
    Vector3[] blobs;

    void Awake() {
        prng = new System.Random(seed);
        blobs = new Vector3[blobCount +1];
        blobs[0] = new Vector3(transform.position.x, transform.position.y, radiusOfFoodSource); //Spawn at center
        for (int i = 0; i < blobCount; i++)
        {
            Vector2 newPos = (Vector2)transform.position + Random.insideUnitCircle * radiusOfFoodSource;
            float newRad = radiusOfFoodSource * Mathf.Lerp(radiusOfFoodSource * 0.2f, radiusOfFoodSource * 0.5f, Random.value);
            blobs[i + 1] = new Vector3(newPos.x, newPos.y, newRad);
        }
        for (int i = 0; i < foodCount; i++)
        {
            SpawnFood();
        }
    }
    void SpawnFood() {
        Vector3 blob = blobs[prng.Next(0, blobs.Length)];
        Vector2 centre = (Vector2)blob + Random.insideUnitCircle.normalized * blob.z * Mathf.Min(Random.value, Random.value);
        GameObject foodSource = Instantiate(foodPrefab, centre, Quaternion.identity, transform);
        foodSource.layer = 7;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radiusOfFoodSource);
    }

    void Update() {
        if (transform.childCount < foodCount && maintainAmount) {
            SpawnFood();
        }
    }
}
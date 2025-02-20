using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour {
    [SerializeField] private GameObject parent;
    [SerializeField] private Material material;
    [SerializeField] private int numberOfCities = 1;
    private float xBound = 8.5f;
    private float yBound = 4.5f;
    public List<Vector3> cityPositions = new List<Vector3>();  // [ (-1, -2, 0), (-4, 6, 0), (8.5, 4.5, 0), (-3, 7, 0)]

    void Start() {
        for (int i = 0; i < numberOfCities; i++) {
            Vector3 position = GetRandomCityPosition();
            cityPositions.Add(position);
            CreateCity(position);
        }
    }

    private Vector3 GetRandomCityPosition() {
        float xPos = Random.Range(-xBound, xBound); // -8.5 to 8.5 4 times
        float yPos = Random.Range(-yBound, yBound); //-4.5 to 4.5
        return new Vector3(xPos, yPos, 0);
    }

    private void CreateCity(Vector3 position) {
        GameObject city = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        city.transform.position = position;
        city.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        city.GetComponent<Renderer>().material = material;
        city.transform.SetParent(parent.transform);
    }



}

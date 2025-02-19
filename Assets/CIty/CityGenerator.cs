using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour {
    [SerializeField] private GameObject parent; 
    [SerializeField] private Material cityMaterial;
    [SerializeField] private int numberOfCities = 1;
    private float xBound = 8.5f; 
    private float yBound = 4.5f; 
    private List<Vector3> cityPositions = new List<Vector3>();  

    void Start() {
        for (int i = 0; i < numberOfCities; i++) {
            Vector3 position = GetRandomCityPosition();
            cityPositions.Add(position);
            CreateCity(position);
        }
    }

    private Vector3 GetRandomCityPosition() {
        float xPos = Random.Range(-xBound, xBound); 
        float yPos = Random.Range(-yBound, yBound); 
        return new Vector3(xPos, yPos, 0);
    }

    private void CreateCity(Vector3 position) {
        GameObject city = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        city.transform.position = position;
        city.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        city.GetComponent<Renderer>().material = cityMaterial;
        city.transform.SetParent(parent.transform);
    }
}

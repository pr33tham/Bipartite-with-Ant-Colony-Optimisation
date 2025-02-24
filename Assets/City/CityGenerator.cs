using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [SerializeField] public int noOfCities = 1;
    [SerializeField] GameObject cityPrefab;
    [SerializeField] GameObject Parent;
    public List<Vector3> cityPoistions;

    private float xBounds = 8.5f;
    private float yBounds = 4.5f;
    // Start is called before the first frame update
    void Start() {
        for(int i = 0; i < noOfCities; i++) {
            Vector3 cityPos = GenerateRandomCityPositions();
            cityPoistions.Add(cityPos);
            GenerateCity(cityPos);
        }
    }

    private void GenerateCity(Vector3 position) {
        GameObject city = Instantiate(cityPrefab, position, Quaternion.identity);
        city.transform.SetParent(Parent.transform);
    }

    private Vector3 GenerateRandomCityPositions() {
        float xPos = UnityEngine.Random.Range(-xBounds, xBounds);
        float yPos = UnityEngine.Random.Range(-yBounds, yBounds);
        Vector3 cityPos = new Vector3(xPos, yPos, 0);
        return cityPos;
    }

    // Update is called once per frame
    //void Update() {

    //}
}

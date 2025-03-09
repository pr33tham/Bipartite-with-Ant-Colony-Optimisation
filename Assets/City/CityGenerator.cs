using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CityGenerator : MonoBehaviour {

    public int noOfCities = 8;
    public GameObject cityPrefab;
    public Transform cityHolder;

    [DoNotSerialize] 
    public Graph<City> graph;

    private float xBounds = 8.5f;
    private float yBounds = 4.5f;

    // Start is called before the first frame update
    void Start() {
        graph = new Graph<City>(noOfCities);
        GenerateBipartiteGraph();
    }


    public void GenerateBipartiteGraph() {
        int setSize = noOfCities / 2; // Divide into two sets
      
        List<City> XCities = new List<City>(); //
        List<City> YCities = new List<City>();

        // Generate cities for set X (e.g., left side)
        for (int i = 0; i < setSize; i++) {
            Vector3 pos = GenerateRandomCityPosition();
            City city = new City("X" + (i+1), pos);
            XCities.Add(city);
            graph.AddNode(city);
            GameObject xCity = Instantiate(cityPrefab, pos, Quaternion.identity, cityHolder);
            xCity.GetComponent<SpriteRenderer>().color = Color.red;
        }

        // Generate cities for set Y (e.g., right side)
        for (int i = 0; i < setSize; i++) {
            Vector3 pos = GenerateRandomCityPosition();
            City city = new City("Y" + (i+1), pos);
            YCities.Add(city);
            graph.AddNode(city);
            Instantiate(cityPrefab, pos, Quaternion.identity, cityHolder);
            GameObject yCity = Instantiate(cityPrefab, pos, Quaternion.identity, cityHolder);
            yCity.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        graph.AddEdge(XCities[0], YCities[0]);    //X1 -> Y1
        graph.AddEdge(XCities[0], YCities[1]);    //X1 -> Y2
        graph.AddEdge(XCities[0], YCities[3]);    //X1 -> Y4

        graph.AddEdge(XCities[1], YCities[0]);    //X2 -> Y1
        graph.AddEdge(XCities[1], YCities[1]);    //X2 -> Y2
        graph.AddEdge(XCities[1], YCities[2]);    //X2 -> Y3

        graph.AddEdge(XCities[2], YCities[1]);    //X3 -> Y2
        graph.AddEdge(XCities[2], YCities[2]);    //X3 -> Y3
        graph.AddEdge(XCities[2], YCities[3]);    //X3 -> Y4

        graph.AddEdge(XCities[3], YCities[0]);    //X4 -> Y1
        graph.AddEdge(XCities[3], YCities[2]);    //X4 -> Y3
        graph.AddEdge(XCities[3], YCities[3]);    //X4 -> Y4
    }

    private Vector3 GenerateRandomCityPosition() {
        float xPos = UnityEngine.Random.Range(-xBounds, xBounds);
        float yPos = UnityEngine.Random.Range(-yBounds, yBounds);
        Vector3 cityPos = new Vector3(xPos, yPos, 0);
        return cityPos;
    } 
}

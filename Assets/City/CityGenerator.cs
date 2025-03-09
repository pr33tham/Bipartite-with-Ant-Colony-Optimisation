using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CityGenerator : MonoBehaviour {

    public int noOfCities = 8;
    public GameObject cityPrefab;
    public Transform cityHolder;
    public Transform edgeHolder;

    [DoNotSerialize] 
    public Graph<City> graph;

    private float xBounds = 8.5f;
    private float yBounds = 4.5f;

    //Sets
    List<City> XCities;
    List<City> YCities;
    int setSize;

    //Edge Renderers
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    private void Start() {
        graph = new Graph<City>(noOfCities);
        XCities = new List<City>();
        YCities = new List<City>();
        setSize = noOfCities / 2;
        GenerateCities();
        AddEdges();
        GenerateEdges();
    }

    private void GenerateCities() {

        for (int i = 0; i < setSize; i++) {
            Vector3 pos = GenerateRandomCityPosition();
            City city = new City("X" + (i + 1), "X", pos);
            XCities.Add(city);
            graph.AddNode(city);
            GameObject xCity = Instantiate(cityPrefab, pos, Quaternion.identity, cityHolder);
            xCity.GetComponent<SpriteRenderer>().color = Color.red;
        }

        for (int i = 0; i < setSize; i++) {
            Vector3 pos = GenerateRandomCityPosition();
            City city = new City("Y" + (i + 1), "Y", pos);
            YCities.Add(city);
            graph.AddNode(city);
            Instantiate(cityPrefab, pos, Quaternion.identity, cityHolder);
            GameObject yCity = Instantiate(cityPrefab, pos, Quaternion.identity, cityHolder);
            yCity.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    private void AddEdges() {
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


    private void GenerateEdges() {

        for(int i = 0; i < graph.currentListSize; i++) {

            LinkedList<City> currentLList = graph.adj_list[i];

            City Head = currentLList.First.Value;   

            LinkedListNode<City> nextNode = currentLList.First.Next;
            while(nextNode != null) {
                City nextCity = nextNode.Value;

                GameObject line = new GameObject("Line");
                line.transform.SetParent(edgeHolder);
                lineRenderer = line.AddComponent<LineRenderer>();
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.white;
                lineRenderer.endColor = Color.white;
                lineRenderer.positionCount = 2;

                lineRenderer.SetPosition(0, Head.position);
                lineRenderer.SetPosition(1, nextCity.position);
            }
        }
    }

    private Vector3 GenerateRandomCityPosition() {
        float xPos = UnityEngine.Random.Range(-xBounds, xBounds);
        float yPos = UnityEngine.Random.Range(-yBounds, yBounds);
        Vector3 cityPos = new Vector3(xPos, yPos, 0);
        return cityPos;
    } 
}

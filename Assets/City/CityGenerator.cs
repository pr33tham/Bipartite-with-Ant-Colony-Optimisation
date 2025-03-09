using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

<<<<<<< Updated upstream
public class CityGenerator : MonoBehaviour
{
    [SerializeField] public int noOfCities = 1;
    [SerializeField] GameObject cityPrefab;
    [SerializeField] GameObject Parent;
    public List<Vector3> cityPoistions;
=======

public class CityGenerator : MonoBehaviour {

    public int noOfCities = 8;
    public GameObject cityPrefab;
    public Transform cityHolder;
    public Transform edgeHolders;

    [DoNotSerialize]
    public Graph<City> graph;
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
    private void GenerateCity(Vector3 position) {
        GameObject city = Instantiate(cityPrefab, position, Quaternion.identity);
        city.transform.SetParent(Parent.transform);
=======

    public void GenerateBipartiteGraph() {
        int setSize = noOfCities / 2; // Divide into two sets

        List<City> XCities = new List<City>();
        List<City> YCities = new List<City>();

        // Generate cities for set X (e.g., left side)
        for (int i = 0; i < setSize; i++) {
            Vector3 pos = GenerateRandomCityPosition();
            City city = new City("X" + (i + 1), pos);
            XCities.Add(city);
            graph.AddNode(city);
            InstantiateCity(city.Name, pos, Color.red);
        }

        // Generate cities for set Y (e.g., right side)
        for (int i = 0; i < setSize; i++) {
            Vector3 pos = GenerateRandomCityPosition();
            City city = new City("Y" + (i + 1), pos);
            YCities.Add(city);
            graph.AddNode(city);
            InstantiateCity(city.Name, pos, Color.blue);
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

        //Debug.Log("Current List Size " + graph.currentListSize);
        //graph.VisualizeGraph();


        for (int i = 0; i < graph.currentListSize; i++) {
            LinkedList<City> currentList = graph.adj_list[i];
            City Head = currentList.First.Value;
            Debug.Log("current head node " + Head.Name);
            LinkedListNode<City> currentNode = currentList.First.Next;
            while (currentNode != null) {
                City connectedCity = currentNode.Value;
                Debug.Log("Connected City: " + connectedCity.Name);
                DrawEdgesOnScene(Head, connectedCity);
                currentNode = currentNode.Next;
            }
        }

    }

    private void DrawEdgesOnScene(City head, City connectedCity) {
        LineRenderer lineRenderer;
        GameObject lineObject = new GameObject("Line");
        lineObject.transform.SetParent(edgeHolders);
        lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f; 
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, head.position); 
        lineRenderer.SetPosition(1, connectedCity.position);
    }

    private void InstantiateCity(String name, Vector3 pos, Color col) {
        GameObject city = Instantiate(cityPrefab, pos, Quaternion.identity, cityHolder);
        city.GetComponent<SpriteRenderer>().color = col;

        GameObject cityNameText = new GameObject("CityNameText");
        cityNameText.transform.SetParent(city.transform); // Make the text a child of the city object
        cityNameText.transform.localPosition = new Vector3(0, 1, 0); // Position the text above the city (1 unit above in the Y-axis)

        TextMeshPro textMeshPro = cityNameText.AddComponent<TextMeshPro>();
        textMeshPro.text = name;

        textMeshPro.fontSize = 2;
        textMeshPro.color = Color.white;
        textMeshPro.alignment = TextAlignmentOptions.Center;

        textMeshPro.isTextObjectScaleStatic = true; // Disable scaling based on camera distance
>>>>>>> Stashed changes
    }

    private Vector3 GenerateRandomCityPositions() {
        float xPos = UnityEngine.Random.Range(-xBounds, xBounds);
        float yPos = UnityEngine.Random.Range(-yBounds, yBounds);
        Vector3 cityPos = new Vector3(xPos, yPos, 0);
        return cityPos;
    }
<<<<<<< Updated upstream

    // Update is called once per frame
    //void Update() {

    //}
}
=======
}
>>>>>>> Stashed changes

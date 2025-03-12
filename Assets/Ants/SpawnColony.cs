using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnColony
    : MonoBehaviour {

    [SerializeField] GameObject antPrefab;
    [SerializeField] Transform antHolder;
    [SerializeField] GameObject CityGenerator;
    private float desirabilityThreshold = 1f;
    CityGenerator cityGen;

    private void Start() {
        cityGen = CityGenerator.GetComponent<CityGenerator>();
    }

    public void GenerateAntsOnClick() {
        for (int i = 0; i < cityGen.graph.currentListSize; i++) {
            City currentCity = cityGen.graph.adj_list[i].First.Value;
            GameObject ant = Instantiate(antPrefab, currentCity.position, 
                Quaternion.Euler(0, 0, Random.value * 360), antHolder);
        }
    }

    private void Update() {

    }
}
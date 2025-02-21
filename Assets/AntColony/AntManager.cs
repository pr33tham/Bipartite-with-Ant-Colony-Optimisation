using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntManager : MonoBehaviour {

    [SerializeField] GameObject AntPrefab;
    private int noOfAnts;
    //public List<>
    // Start is called before the first frame update
    void Start() {
        noOfAnts = GetComponent<CityGenerator>().numberOfCities;


        for(int i = 0; i < noOfAnts; i++) {

        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}

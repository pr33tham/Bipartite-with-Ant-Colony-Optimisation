using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour {

    //Only an Agent that moves around cities instead of biological ants
    //Can implement later
    public enum State { searchingSetA, searchingSetB };
    public Transform pheramoneDepositPos;



    List<City> VisitedCities;

    private void Start() {

    }

    private void Update() {
        
    }
}
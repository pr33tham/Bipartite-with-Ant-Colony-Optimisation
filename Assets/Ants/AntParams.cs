using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntParams : ScriptableObject {

    [Header("Movement")]
    public float antSpeed;

    [Header("Pheromones")]
    public float dstBetweenMarkers = 0.75f;
    public float pheromoneEvaporateTime = 45;
    public float pheromoneRunOutTime = 30;
    public float pheromoneWeight = 1;
    public float perceptionRadius = 2.5f;
    public bool useHomeMarkers = true;
    public bool useFoodMarkers = true;

    [Header("Sensing")]
    public float sensorSize = 0.75f;
    public float sensorDst = 1.25f;
    public float sensorSpacing = 1;
    public float antennaDst = 0.25f;

}

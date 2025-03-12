using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AntParams : ScriptableObject {

    [Header("Movement")]
    public float antSpeed = 1f;

    [Header("Pheromones")]
    public float dstBetweenMarkers = 0.75f;
    public float pheromoneEvaporateTime = 45;
    public float pheromoneRunOutTime = 30;
    public float pheromoneWeight = 1;
    public bool useFirstMarkers = true;
    public bool useSecondMarkers = true;

    [Header("Sensing")]
    public float perceptionRadius = 2.5f;
    public float sensorSize = 0.75f;
    public float sensorDst = 1.25f;
    public float sensorSpacing = 1;
    public float antennaDst = 0.25f;
}
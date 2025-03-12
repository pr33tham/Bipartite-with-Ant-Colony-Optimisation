using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSensors : MonoBehaviour {

    //Ant
    public AntParams settings;
    float sqrPerceptionRadius;

    //Particle System
    public ParticleSystem pheramonesParticleSystem;
    ParticleSystem.EmitParams emissionParam;

    //Pheramones
    Color pheremoneColor;
    public float pheremoneSize = 0.1f;
    public float pheremoneColorAlpha = 1;
    public float pheremoneWeight = 1f;

    //Grid
    public Vector2 area;
    int numCellsX;
    int numCellsY;
    Vector2 halfSize;
    float cellSizeReciprocal;
    Cell[,] cells;

    private void Start() {
        float perceptionRadius = Mathf.Max(0.01f, settings.sensorSize);
        sqrPerceptionRadius = perceptionRadius * perceptionRadius;
        numCellsX = Mathf.CeilToInt(area.x / perceptionRadius);
        numCellsY = Mathf.CeilToInt(area.y / perceptionRadius);
        halfSize = new Vector2(numCellsX * perceptionRadius, numCellsY * perceptionRadius) * 0.5f;
        cellSizeReciprocal = 1 / perceptionRadius;
        cells = new Cell[numCellsX, numCellsY];

        for (int y = 0; y < numCellsY; y++) {
            for (int x = 0; x < numCellsX; x++) {
                cells[x, y] = new Cell();
            }
        }

        emissionParam.startLifetime = settings.pheromoneEvaporateTime;
        emissionParam.startSize = pheremoneSize;
        var m = pheramonesParticleSystem.main;
        m.maxParticles = 100 * 1000;
        var c = pheramonesParticleSystem.colorOverLifetime;
        c.enabled = true;

        Gradient grad = new Gradient();
        grad.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.white, 1) };
        grad.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(pheremoneColorAlpha, 0.0f), new GradientAlphaKey(0.0f, 1.0f) };

        c.color = grad;
    }

    //private void Update() {
    //    emissionParam.startLifetime = settings.pheromoneEvaporateTime;
    //}

    public void AddDeposit(Vector2 point) {
        Vector2Int cellCoord = CoordinationfromPosition(point);
        Cell cell = cells[cellCoord.x, cellCoord.y];
        Entry entry = new Entry() { position = point, creationTime = Time.time, initialWeight = pheremoneWeight };
        cell.Add(entry);
        emissionParam.startColor = new Color(pheremoneColor.r, pheremoneColor.g, pheremoneColor.b, pheremoneWeight);
        emissionParam.position = point;
        pheramonesParticleSystem.Emit(emissionParam, 1);
    }

    public Vector2Int CoordinationfromPosition(Vector2 point) {
        int x = (int)((point.x + halfSize.x) * cellSizeReciprocal);
        int y = (int)((point.y + halfSize.y) * cellSizeReciprocal);
        return new Vector2Int(Mathf.Clamp(x, 0, numCellsX - 1), Mathf.Clamp(y, 0, numCellsY - 1));
    }
}

public class Cell {
    public LinkedList<Entry> entries;

    public Cell() {
        entries = new LinkedList<Entry>();
    }

    public void Add(Entry entry) {
        entries.AddLast(entry);
    }

}

public struct Entry {
    public Vector2 position;
    public float initialWeight;
    public float creationTime;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PheromoneType { Home, Food, Relay }  

public class PheramoneSensory : MonoBehaviour {

    public Vector2 area;
    public AntParams antSettings;
    public ParticleSystem particleDisplay;
    ParticleSystem.EmitParams particleEmitParams;
    float sqrPerceptionRadius;

    public Color pheremoneColor;
    public float pheremoneSize = 0.05f;
    public float initialAlpha = 1;

    public Color homePheromoneColor = Color.red;
    public Color foodPheromoneColor = Color.green;
    public Color relayPheromoneColor = Color.blue;

    int numCellsX;
    int numCellsY;
    Vector2 halfSize;
    float cellSizeReciprocal;
    Cell[,] cells;

    void Awake() {
        Init();
    }

    void Update() {
        particleEmitParams.startLifetime = antSettings.pheromoneEvaporateTime;
    }

    void Init() {
        float perceptionRadius = Mathf.Max(0.01f, antSettings.sensorSize);
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

        particleEmitParams.startLifetime = antSettings.pheromoneEvaporateTime;
        particleEmitParams.startSize = pheremoneSize;
        var m = particleDisplay.main;
        m.maxParticles = 100 * 1000;
        var c = particleDisplay.colorOverLifetime;
        c.enabled = true;

        Gradient grad = new Gradient();
        grad.colorKeys = new GradientColorKey[] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.white, 1) };
        grad.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(initialAlpha, 0.0f), new GradientAlphaKey(0.0f, 1.0f) };

        c.color = grad;
    }

    public void Add(Vector2 point, float initialWeight, PheromoneType type) {
        Vector2Int cellCoord = CellCoordFromPos(point);
        Cell cell = cells[cellCoord.x, cellCoord.y];
        Entry entry = new Entry() { position = point, creationTime = Time.time, initialWeight = initialWeight, type = type };
        cell.Add(entry);

        Color markerColor;
        switch (type) {
            case PheromoneType.Home:
                markerColor = homePheromoneColor;
                break;
            case PheromoneType.Food:
                markerColor = foodPheromoneColor;
                break;
            case PheromoneType.Relay:
                markerColor = relayPheromoneColor;
                break;
            default:
                markerColor = pheremoneColor;
                break;
        }
        particleEmitParams.startColor = new Color(markerColor.r, markerColor.g, markerColor.b, initialWeight);
        particleEmitParams.position = point;
        particleDisplay.Emit(particleEmitParams, 1);
    }

    public int GetAllInCircle(Entry[] result, Vector2 centre) {
        Vector2Int cellCoord = CellCoordFromPos(centre);
        int i = 0;
        float currentTime = Time.time;

        for (int offsetY = -1; offsetY <= 1; offsetY++) {
            for (int offsetX = -1; offsetX <= 1; offsetX++) {
                int cellX = cellCoord.x + offsetX;
                int cellY = cellCoord.y + offsetY;
                if (cellX >= 0 && cellX < numCellsX && cellY >= 0 && cellY < numCellsY) {
                    Cell cell = cells[cellX, cellY];

                    var currentEntryNode = cell.entries.First;
                    while (currentEntryNode != null) {
                        Entry currentEntry = currentEntryNode.Value;
                        float currentLifetime = currentTime - currentEntry.creationTime;
                        // Remove expired entries
                        if (currentLifetime > antSettings.pheromoneEvaporateTime) {
                            cell.entries.Remove(currentEntryNode);
                        }
                        else if ((currentEntry.position - centre).sqrMagnitude < sqrPerceptionRadius) {
                            if (i >= result.Length) {
                                return result.Length;
                            }
                            result[i] = currentEntry;
                            i++;
                        }
                        currentEntryNode = currentEntryNode.Next;
                    }
                }
            }
        }
        return i;
    }

    Vector2Int CellCoordFromPos(Vector2 point) {
        int x = (int)((point.x + halfSize.x) * cellSizeReciprocal);
        int y = (int)((point.y + halfSize.y) * cellSizeReciprocal);
        return new Vector2Int(Mathf.Clamp(x, 0, numCellsX - 1), Mathf.Clamp(y, 0, numCellsY - 1));
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
        public PheromoneType type; 
    }
}

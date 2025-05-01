using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour {
    public enum AntBehaviorState { Searching, Returning }

    [Header("References and Settings")]
    public AntParams settings;
    public Transform head;
    public LayerMask foodMask;
    public LayerMask homeMask;
    public LayerMask collisionMask;
    public LayerMask relayMask;

    [Header("Antennae and Perception")]
    public Transform antennaLeft;
    public Transform antennaRight;
    public Transform perceptionCenter;

    AntBehaviorState antState;
    Vector2 velocity;
    Vector2 steerFromObstacles;
    float nextRandomSteerTime;

    Vector2[] sensorPositions = new Vector2[3];
    float[] sensorStrengths = new float[3];

    Transform heldFood;

    Vector2 lastPheromoneLocation;
    Collider2D[] nearbyFood = new Collider2D[1];
    PheramoneSensory.Entry[] pheromoneBuffer;
    SpwanColony colony;
    float nextDirectionUpdate;

    Vector2 randomSteer;
    Vector2 pheromoneSteer;

    Vector2 forwardDir;
    Vector2 currentPosition;
    float collisionHalfDist;
    Vector2 avoidForce;
    float avoidForceResetTime;
    Vector2 nestLocation;

    enum AntennaSide { None, Left, Right }
    AntennaSide lastAntennaSide;

    Transform targetFood;
    float expireTime;
    bool isTurning;
    Vector2 turnForce;
    float turnEndTime;

    float timeLeftNest;
    float timeLeftFood;


    public void AssignColony(SpwanColony newColony) {
        colony = newColony;
    }


    void Start() {
        lastPheromoneLocation = transform.position;
        antState = AntBehaviorState.Searching;
        transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
        forwardDir = transform.right;
        currentPosition = transform.position;
        velocity = forwardDir * settings.maxSpeed;

        nestLocation = transform.position;
        nearbyFood = new Collider2D[1];

        const int maxPheromoneCount = 1024;
        pheromoneBuffer = new PheramoneSensory.Entry[maxPheromoneCount];
        nextDirectionUpdate = Random.value * settings.timeBetweenDirUpdate;
        collisionHalfDist = settings.collisionRadius * 0.5f;
        expireTime = Time.time + settings.lifetime + Random.Range(0, settings.lifetime / 2f);
        timeLeftNest = Time.time;
    }

    void Update() {
        if (Time.time > expireTime && settings.useDeath) {
            Destroy(gameObject);
            return;
        }

        PlacePheromones();
        UpdateRandomSteer();

        if (antState == AntBehaviorState.Searching) {
            SearchForFood();
        }
        else if (antState == AntBehaviorState.Returning) {
            ReturnProcess();
        }

        ProcessObstacleAvoidance();
        MoveAnt();
    }


    void MoveAnt() {
        Vector2 compositeSteer = randomSteer + pheromoneSteer + avoidForce;
        if (isTurning) {
            compositeSteer += turnForce * settings.targetSteerStrength;
            if (Time.time > turnEndTime) {
                isTurning = false;
            }
        }

        Vector2 desiredVelocity = compositeSteer.normalized * settings.maxSpeed;
        ApplySteering(desiredVelocity);
        forwardDir = velocity.normalized;
        float displacement = velocity.magnitude * Time.deltaTime;
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime;

        // Check obstacles ahead using a raycast.
        RaycastHit2D obstacleHit = Physics2D.Raycast(currentPosition, forwardDir, Mathf.Max(settings.collisionRadius, displacement), collisionMask);
        if (obstacleHit) {
            if (!isTurning) {
                InitiateTurnAround(Vector2.Reflect(forwardDir, obstacleHit.normal), 2);
            }
            newPosition = obstacleHit.point - forwardDir * settings.collisionRadius;
        }

        currentPosition = newPosition;
        transform.SetPositionAndRotation(new Vector3(currentPosition.x, currentPosition.y, -0.1f),
            Quaternion.FromToRotation(Vector3.right, forwardDir));
    }

    void ApplySteering(Vector2 targetVelocity) {
        Vector2 delta = targetVelocity - velocity;
        Vector2 acceleration = Vector2.ClampMagnitude(delta * settings.acceleration, settings.acceleration);
        velocity += acceleration * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, settings.maxSpeed);
    }


    void ProcessObstacleAvoidance() {
        RaycastHit2D hitLeft = Physics2D.Raycast(antennaLeft.position, antennaLeft.right, settings.antennaDst, collisionMask);
        RaycastHit2D hitRight = Physics2D.Raycast(antennaRight.position, antennaRight.right, settings.antennaDst, collisionMask);

        if (Time.time > avoidForceResetTime) {
            avoidForce = Vector2.zero;
            lastAntennaSide = AntennaSide.None;
        }

        if (hitLeft || hitRight) {
            if (hitLeft && lastAntennaSide != AntennaSide.Right && (!hitRight || hitLeft.distance < hitRight.distance)) {
                avoidForce = -transform.up * settings.collisionAvoidSteerStrength;
                lastAntennaSide = AntennaSide.Left;
            }
            if (hitRight && lastAntennaSide != AntennaSide.Left && (!hitLeft || hitRight.distance < hitLeft.distance)) {
                avoidForce = transform.up * settings.collisionAvoidSteerStrength;
                lastAntennaSide = AntennaSide.Right;
            }
            avoidForceResetTime = Time.time + 0.5f;
            randomSteer = avoidForce.normalized * settings.randomSteerStrength;
        }
    }

    void InitiateTurnAround(Vector2 newDir, float randomness = 0.2f) {
        isTurning = true;
        turnEndTime = Time.time + 1.5f;
        Vector2 perpendicular = new Vector2(-newDir.y, newDir.x);
        turnForce = newDir + perpendicular * (Random.value - 0.5f) * 2 * randomness;
    }

    void InitiateTurnAround(float randomness = 0.2f) {
        InitiateTurnAround(-forwardDir, randomness);
    }

    void SearchForFood() {
        if (colony) {
            if (Vector2.SqrMagnitude(currentPosition - nestLocation) < colony.radius * colony.radius) {
                expireTime = Time.time + settings.lifetime;
                timeLeftNest = Time.time;
            }
        }

        if (targetFood == null) {
            int foundFood = Physics2D.OverlapCircleNonAlloc(perceptionCenter.position, settings.perceptionRadius, nearbyFood, foodMask);
            if (foundFood > 0) {
                targetFood = nearbyFood[Random.Range(0, foundFood)].transform;
                targetFood.gameObject.layer = 0;
            }
        }

        if (targetFood != null) {
            Vector2 vectorToFood = targetFood.position - transform.position;
            float distanceToFood = vectorToFood.magnitude;
            Vector2 dirToFood = vectorToFood.normalized;
            pheromoneSteer = dirToFood * settings.targetSteerStrength;
            if (distanceToFood < targetFood.localScale.x * 1f) {
                heldFood = targetFood;
                targetFood.position = head.position;
                targetFood.SetParent(transform, true);
                targetFood.gameObject.layer = 0;
                antState = AntBehaviorState.Returning;
                nextDirectionUpdate = 0;
                InitiateTurnAround();
                timeLeftFood = Time.time;
                targetFood = null;
            }
        }
        else {
            EvaluatePheromoneSteering();
        }
    }

    void ReturnProcess() {
        Vector2 currentPos = transform.position;
        Collider2D relayNode = Physics2D.OverlapCircle(perceptionCenter.position, settings.perceptionRadius, relayMask);
        if (relayNode != null) {
            DepositToRelay(relayNode);
            antState = AntBehaviorState.Searching;
            nextDirectionUpdate = 0;
            InitiateTurnAround();
            colony.FoodCollected();
            timeLeftFood = Time.time;
            return;
        }

        Collider2D homeBase = Physics2D.OverlapCircle(perceptionCenter.position, settings.perceptionRadius, homeMask);
        if (homeBase != null) {
            pheromoneSteer = ((Vector2)homeBase.transform.position - currentPos).normalized * settings.targetSteerStrength;

            if (Vector2.SqrMagnitude(currentPos - nestLocation) < colony.radius * colony.radius) {
                expireTime = Time.time + settings.lifetime;
                Destroy(heldFood.gameObject);
                antState = AntBehaviorState.Searching;
                nextDirectionUpdate = 0;
                InitiateTurnAround();
                colony.FoodCollected();
                timeLeftNest = Time.time;
            }
        }
        else {
            EvaluatePheromoneSteering();
        }
    }

    void DepositToRelay(Collider2D relayCollider) {
        heldFood.SetParent(relayCollider.transform, true);
        heldFood.position = relayCollider.transform.position;
        heldFood = null;
    }


    void PlacePheromones() {

        if (Vector2.Distance(transform.position, lastPheromoneLocation) > settings.dstBetweenMarkers) {
            if (antState == AntBehaviorState.Searching && settings.useHomeMarkers && (Time.time - timeLeftNest) < settings.pheromoneRunOutTime) {
                float markerWeight = Mathf.Lerp(0.5f, 1f, 1 - ((Time.time - timeLeftNest) / settings.pheromoneRunOutTime));
                colony.homeMarkers.Add(transform.position, markerWeight, PheromoneType.Home);
                lastPheromoneLocation = transform.position + (Vector3)Random.insideUnitCircle * settings.dstBetweenMarkers * 0.2f;
            }
            else if (antState == AntBehaviorState.Returning && settings.useFoodMarkers && (Time.time - timeLeftFood) < settings.pheromoneRunOutTime) {
                float markerWeight = Mathf.Lerp(0.5f, 1f, 1 - ((Time.time - timeLeftFood) / settings.pheromoneRunOutTime));
                colony.foodMarkers.Add(transform.position, markerWeight, PheromoneType.Food);
                lastPheromoneLocation = transform.position + (Vector3)Random.insideUnitCircle * settings.dstBetweenMarkers * 0.2f;
            }
        }
    }

    void EvaluatePheromoneSteering() {
        if (Time.time > nextDirectionUpdate) {
            Vector2 sensorCenter = currentPosition + forwardDir * settings.sensorDst;
            Vector2 leftDir = (forwardDir + (Vector2)transform.up * settings.sensorDst).normalized;
            Vector2 sensorLeft = currentPosition + leftDir * settings.sensorDst;
            Vector2 rightDir = (forwardDir - (Vector2)transform.up * settings.sensorDst).normalized;
            Vector2 sensorRight = currentPosition + rightDir * settings.sensorDst;

            sensorPositions[0] = sensorCenter;
            sensorPositions[1] = sensorLeft;
            sensorPositions[2] = sensorRight;

            float now = Time.time;
            for (int i = 0; i < 3; i++) {
                sensorStrengths[i] = 0;
                int foundMarkers = 0;
                if (antState == AntBehaviorState.Searching && settings.useFoodMarkers)
                    foundMarkers = colony.foodMarkers.GetAllInCircle(pheromoneBuffer, sensorPositions[i]);
                if (antState == AntBehaviorState.Returning && settings.useHomeMarkers)
                    foundMarkers = colony.homeMarkers.GetAllInCircle(pheromoneBuffer, sensorPositions[i]);

                for (int j = 0; j < foundMarkers; j++) {
                    float elapsed = (now - pheromoneBuffer[j].creationTime) / settings.pheromoneEvaporateTime;
                    float strength = Mathf.Clamp01(1 - elapsed);
                    sensorStrengths[i] += strength;
                }
            }

            float centerStrength = sensorStrengths[0];
            float leftStrength = sensorStrengths[1];
            float rightStrength = sensorStrengths[2];

            if (centerStrength > leftStrength && centerStrength > rightStrength)
                pheromoneSteer = forwardDir * settings.pheromoneWeight;
            else if (leftStrength > rightStrength)
                pheromoneSteer = leftDir * settings.pheromoneWeight;
            else if (rightStrength > leftStrength)
                pheromoneSteer = rightDir * settings.pheromoneWeight;

            nextDirectionUpdate = Time.time + settings.timeBetweenDirUpdate;
        }
    }


    void UpdateRandomSteer() {
        if (targetFood != null) {
            randomSteer = Vector2.zero;
            return;
        }

        if (Time.time > nextRandomSteerTime) {
            nextRandomSteerTime = Time.time + Random.Range(settings.randomSteerMaxDuration / 3, settings.randomSteerMaxDuration);
            randomSteer = ChooseRandomDirection(forwardDir, 5) * settings.randomSteerStrength;
        }
    }

    Vector2 ChooseRandomDirection(Vector2 baseDir, int iterations = 4) {
        Vector2 bestRandom = Vector2.zero;
        float bestScore = -1f;
        for (int i = 0; i < iterations; i++) {
            Vector2 candidate = Random.insideUnitCircle.normalized;
            float score = Vector2.Dot(baseDir, candidate);
            if (score > bestScore) {
                bestScore = score;
                bestRandom = candidate;
            }
        }
        return bestRandom;
    }
}

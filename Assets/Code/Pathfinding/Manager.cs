using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    // Grid
    public int gridWidth, gridHeight;
    public int startX, startY, endX, endY;

    //Pathing
    private Pathfinding pathfinding;
    private Vector3 startPosition, endPosition;
    List<Vector3> path;
    private bool pathfindingDirty;

    // Mob management
    public float mobTimer = 3f;
    public GameObject mob;
    private float currentMobTimer = 0f;
    public float mobsInRound;
    public float mobsSpawned;
    public float mobsDied;

    // Flow management
    public float buildTimer = 5f;
    private float currentBuildTimer = 0f;
    private bool building;
    public int round = 0;

    public GameObject endPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        transform.position = new Vector3(-gridWidth / 2f, -gridHeight / 2f, 0f);

        pathfinding = new Pathfinding(gridWidth, gridHeight, transform.position);

        startPosition = pathfinding.GetGrid().GetWorldPosition(startX, startY);
        endPosition = pathfinding.GetGrid().GetWorldPosition(endX, endY);

        Instantiate(endPrefab, endPosition, Quaternion.identity);

        pathfindingDirty = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(pathfindingDirty || path.Count == 0)
        {
            path = pathfinding.FindPath(startPosition, endPosition);
            pathfindingDirty = false;
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.green);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.GetGrid().GetNode(worldPosition).isWalkable = false;
            pathfindingDirty = true;
        }

        if (building)
        {
            currentBuildTimer += Time.deltaTime;
            if (currentBuildTimer >= buildTimer)
            {
                currentBuildTimer = 0;
                building = false;
                StartRound();
            }
        }
        else if (mobsSpawned < mobsInRound)
        {
            HandleMobSpawning();
        }
        else if(mobsDied == mobsInRound)
        {
            building = true;
        }
    }

    void HandleMobSpawning()
    {
        currentMobTimer += Time.deltaTime;

        if(currentMobTimer >= mobTimer)
        {
            GameObject newMob = Instantiate(mob, startPosition, Quaternion.identity);

            newMob.GetComponent<Enemy>().path = path;

            currentMobTimer = 0;
            mobsSpawned++;
        }
    }

    public void mobDied(Enemy mob)
    {
        mobsDied++;
        // Todo increase currency by amount on the mob
    }

    void StartRound() {
        round++;

        // TODO Decide how many mobs
        mobsInRound = 5;
        mobsSpawned = 0;
    }
}

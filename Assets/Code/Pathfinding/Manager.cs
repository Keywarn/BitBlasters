using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    // Grid
    public int gridWidth, gridHeight;
    public int startX, startY, endX, endY;

    //Pathing
    private Pathfinding pathfinding;
    private Vector3 startPosition, endPosition;
    private List<Vector3> path;
    private List<Vector3> previewPath;
    private bool pathfindingDirty;

    // Mob management
    public float mobTimer = 3f;
    public GameObject mob;
    private float currentMobTimer = 0f;

    // How oftten the round difficulty goes up
    public int difficultyFactor = 5;
    private int difficulty = 1;

    private int mobsInRound;
    private int mobsSpawned;
    private int mobsDied;

    // Flow management
    public float buildTimer = 5f;
    private float currentBuildTimer = 0f;
    public bool building;
    public int round = 0;

    private GameObject currentPlaceable;
    private GameObject previewPlaceable;
    private int oldPreviewX, oldPreviewY;
    private bool canPlace;

    // Currency
    public int data = 50;

    public GameObject endPrefab;
    public GameObject bitBlaster;
    public Color tileColor;
    public Color pathColor;
    public Color previewPathColor;
    public GameObject[] tiles;
    private GameObject[,] tileObjects;

    // UI
    public TMPro.TextMeshProUGUI roundText;
    public TMPro.TextMeshProUGUI dataText;
    public Image RoundTimerFill;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        transform.position = new Vector3(-gridWidth / 2f, -gridHeight / 2f, 0f);

        pathfinding = new Pathfinding(gridWidth, gridHeight, transform.position);

        startPosition = pathfinding.GetGrid().GetWorldPosition(startX, startY);
        endPosition = pathfinding.GetGrid().GetWorldPosition(endX, endY);

        SetupTiles();

        Instantiate(endPrefab, endPosition, Quaternion.identity);

        pathfindingDirty = true;

        building = true;
        StartRound();

        // Assign the starting data amount
        ModifyData(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(pathfindingDirty || path.Count == 0)
        {
            DoPathing();
        }

        if (currentPlaceable != null)
        {
            int cost = currentPlaceable.GetComponent<Placeable>().data;
            if (cost > data)
            {
                CancelPlace();

            }

            // Has enough money
            else
            {
                PlacePreview();
                if(Input.GetMouseButtonDown(0))
                {
                    Place();
                }
                else if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Cancel"))
                {
                    CancelPlace();
                }
            }
        }

        if (building)
        {
            currentBuildTimer += Time.deltaTime;
            RoundTimerFill.fillAmount = Mathf.Max(buildTimer-currentBuildTimer, 0f) / buildTimer;

            if (currentBuildTimer >= buildTimer)
            {
                // Remove the current placeable object if it is not an active
                if (currentPlaceable != null && !currentPlaceable.GetComponent<Placeable>().isActive)
                {
                    CancelPlace();
                }

                currentBuildTimer = 0;
                building = false;
                
            }
        }
        else if (mobsSpawned < mobsInRound)
        {
            HandleMobSpawning();
        }
        else if(mobsDied >= mobsInRound)
        {
            StartRound();
            building = true;
        }
    }

    private bool GetPath(out List<Vector3> tempPath)
    {
        tempPath = pathfinding.FindPath(startPosition, endPosition);

        if (tempPath == null)
        {
            return false;
        }

        return true;
    }

    private void DoPathing()
    {
        if (GetPath(out List<Vector3> tempPath)) {
            if (path != null)
            {
                ColorTiles(path, tileColor);
            }
            ColorTiles(tempPath, pathColor);

            path = tempPath;
            pathfindingDirty = false;
        }
    }

    private void ColorTiles (List<Vector3> path, Color color)
    {
        foreach (Vector3 tile in path)
        {
            pathfinding.GetGrid().GetXY(tile, out int x, out int y);
            tileObjects[x, y].GetComponent<SpriteRenderer>().color = color;
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

        if(mob.health > 0)
        {
            ModifyData(-mob.data);
        }
        else
        {
           ModifyData(mob.data);
        }
    }

    void StartRound() {
        round++;
        roundText.text = "Round: " + round;

        mobsInRound = GetMobsCount();

        mobTimer *= 0.95f;
        mobsSpawned = 0;
        mobsDied = 0;
    }

    private int GetMobsCount()
    {
        if(round/difficultyFactor == 1)
        {
            difficultyFactor *= 5;
            difficulty++;
        }

        return (int)(0.15f * round) * (24 + 6 * (difficulty - 1));
        
    }

    void SetupTiles()
    {
        tileObjects = new GameObject[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for(int y = 0; y < gridHeight; y++)
            {
                GameObject tile = Instantiate(tiles[Random.Range(0, tiles.Length - 1)], pathfinding.GetGrid().GetWorldPosition(x, y), Quaternion.identity);
                tileObjects[x, y] = tile;
            }
        }
    }

    public Vector3 GetEndPosition()
    {
        return endPosition;
    }

    public void BeginPlacement(GameObject prefab)
    {
        Placeable placeable = prefab.GetComponent<Placeable>();

        // Trying to place a building not in the build phase
        if(!placeable.isActive && !building)
        {
            return;
        }

        currentPlaceable = prefab;
    }

    private void Place()
    {
        RemovePreview();
        previewPlaceable = null;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (pathfinding.GetGrid().GetXY(worldPosition, out int x, out int y))
        {
            PathNode node = pathfinding.GetGrid().GetNode(x, y);

            // Double check the cost here
            int cost = currentPlaceable.GetComponent<Placeable>().data;
            if(cost > data)
            {
                CancelPlace();
                return;
            }

            if (canPlace)
            {
                GameObject placed = GameObject.Instantiate(currentPlaceable, pathfinding.GetGrid().GetWorldPosition(x, y), Quaternion.identity);
                node.placeable = placed.GetComponent<Placeable>();
                pathfindingDirty = true;
                ModifyData(-cost);
            }
        }
    }

    private void PlacePreview()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (pathfinding.GetGrid().GetXY(worldPosition, out int x, out int y))
        {
            if(oldPreviewX != x || oldPreviewY != y)
            {
                PathNode node = pathfinding.GetGrid().GetNode(x, y);

                if(previewPlaceable == null)
                {
                    previewPlaceable = GameObject.Instantiate(currentPlaceable, pathfinding.GetGrid().GetWorldPosition(x, y), Quaternion.identity);
                }
                else
                {
                    previewPlaceable.transform.position = pathfinding.GetGrid().GetWorldPosition(x, y);
                }

                // Check there isn't already an object at this node
                canPlace = node.placeable == null;

                // Check we aren't preventing pathing
                if (canPlace)
                {
                    node.placeable = previewPlaceable.GetComponent<Placeable>();

                    RemovePreviewPath();

                    if (GetPath(out List<Vector3> tempPath))
                    {
                        if(!SamePath(tempPath, path)) ColorTiles(tempPath, previewPathColor);
                        previewPath = tempPath;
                    }
                    else
                    {
                        canPlace = false;
                    }
                    node.placeable = null;
                }

                // Set the colour
                if (canPlace)
                {
                    previewPlaceable.GetComponent<SpriteRenderer>().color = new Color(1f, 1, 1f, 0.5f);
                }
                else
                {
                    previewPlaceable.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.5f);
                }

                oldPreviewX = x;
                oldPreviewY = y;
            }
        }
    }

    private bool SamePath(List<Vector3> a, List<Vector3> b)
    {
        if (a.Count != b.Count) return false;
        else
        {
            for (int i = 0; i < a.Count; i++)
            {
                if(Vector3.Distance(a[i], b[i]) > 0.1f)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void RemovePreview()
    {
        if (previewPlaceable != null)
        {
            Destroy(previewPlaceable);
            previewPlaceable = null;
        }

        RemovePreviewPath();
    }

    private void RemovePreviewPath()
    {
        if(previewPath != null && !SamePath(previewPath, path))
        {
            ColorTiles(previewPath, tileColor);
            previewPath = null;
            ColorTiles(path, pathColor);
        }
    }

    private void CancelPlace()
    {
        RemovePreview();
        currentPlaceable = null;
    }

    private void ModifyData(int amount)
    {
        data += amount;
        dataText.text = data.ToString();
    }

    public void SkipBuilding()
    {
        if (building) currentBuildTimer = buildTimer;
    }
}

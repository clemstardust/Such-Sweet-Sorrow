using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class RoomSpawns : MonoBehaviour
{
    [Header("Level seed")]
    public int seed;
    [Header("Desired maximum size of level")]
    public int levelSize = 20;
    [Header("Maximum number of special loot rooms to spawn")]
    public int specialRoomCount = 2;
    [Header("Have random deadends spawn")]
    [InspectorName("Enable Random Deadends")] public bool enableRandDeadends = true;
    public int randDeadEndChance = 10;

    [Header("Level Prefabs")]
    public GameObject[] mainRooms;
    public GameObject[] hallways;
    public List<GameObject> specialRooms;
    public GameObject bossArena;
    public GameObject deadEnd;
    public Material[] Skyboxes;

    private int newRoomCode = -1;
    private GameObject roomToSpawn = null;
    private GameObject[] spawnJoints;
    private int maxLevelSize;

    [Header("Loaded Level Joints")]
    [SerializeField] protected List<OpenJoint> loadedJoints = new List<OpenJoint>();
    protected IEnumerable<Collider> LoadedColliders => loadedJoints.SelectMany(s => s.Bounds.Colliders);

    void Awake()
    {
        StartGeneration();
    }

    public void StartGeneration()
    {
        maxLevelSize = levelSize;
        if (seed == 0)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(seed);
        spawnJoints = GameObject.FindGameObjectsWithTag("Joint");
        foreach (var i in spawnJoints)
        {
            levelSize = maxLevelSize;
            i.GetComponent<OpenJoint>().Build(this);
            
        }
        foreach (var c in LoadedColliders)
            c.enabled = false;
        EndGeneration();
        NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
        var allObjects = FindObjectsOfType<GameObject>();
        foreach (var i in allObjects)
        {
            i.isStatic = false;
        }
        //RenderSettings.skybox = Skyboxes[GameManager.globalDarkness];
        var allBounds = FindObjectsOfType<Bounds>();
        foreach (Bounds b in allBounds)
        {
            b.col.enabled = true;
            foreach (Bounds newSection in allBounds)
            {
                newSection.col.enabled = true;
                if (b.col.bounds.Intersects(newSection.col.bounds) && !ReferenceEquals(b.GetComponentInParent<TileProperties>().gameObject, newSection.GetComponentInParent<TileProperties>().gameObject) /*&& b.GetComponentInParent<TileProperties>().gameObject.name != newSection.GetComponentInParent<TileProperties>().gameObject.name*/)
                {
                    print("Destroyed " + newSection.GetComponentInParent<TileProperties>().gameObject.name + " because " + b.GetComponentInParent<TileProperties>().gameObject.name + " intersected with " + newSection.GetComponentInParent<TileProperties>().gameObject.name);
                    Destroy(newSection.GetComponentInParent<TileProperties>().gameObject);
                }
            }
            
        }
        if (GameObject.FindGameObjectWithTag("BossRoom") == null)
        {
            GameManager.spawnedEndRoom = false;
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                if (!child.gameObject.CompareTag("Spawn"))
                    Destroy(child.gameObject);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            GameManager.spawnedEndRoom = true;
        }
        //GameManager.spawnedEndRoom = true;
        
        foreach (var c in LoadedColliders)
            c.enabled = false;
        print("Spawned end room: " + GameManager.spawnedEndRoom);
        //GameObject.FindGameObjectWithTag("LoadingOverlay").SetActive(false);
    }

    public GameObject NextRoom(TileProperties lastRoomProperties)
    {
        switch (lastRoomProperties.leadsTo)
        {
            case TileProperties.LeadsTo.hallway:
                roomToSpawn = hallways[Mathf.RoundToInt(Random.Range(0, hallways.Length))];
                break;
            case TileProperties.LeadsTo.mainroom:
                roomToSpawn = mainRooms[Mathf.RoundToInt(Random.Range(0, mainRooms.Length))];
                if (levelSize <= maxLevelSize * 0.5f && GameObject.FindGameObjectWithTag("BossRoom") == null)
                {
                    roomToSpawn = bossArena;
                }
                else if (ShouldSpawnSpecialRoom())
                {
                    roomToSpawn = specialRooms[specialRooms.Count - 1];
                    //specialRooms.RemoveAt(specialRooms.Count - 1);
                    specialRoomCount--;
                }
                break;
            default:
                newRoomCode = Mathf.RoundToInt(Random.value);
                switch (newRoomCode)
                {
                    case 0:
                        roomToSpawn = mainRooms[Mathf.RoundToInt(Random.Range(0, mainRooms.Length))];
                        break;
                    case 1:
                        roomToSpawn = hallways[Mathf.RoundToInt(Random.Range(0, hallways.Length))];
                        break;
                }
                break;
        }
        return roomToSpawn;
    }
    /*
    public bool IsSectionValid(Bounds newSection, Bounds sectionToIgnore) =>
            !LoadedColliders.Except(sectionToIgnore.Colliders).Any(c => c.bounds.Intersects(newSection.Colliders.First().bounds));*/
    public bool IsSectionValid(Bounds newSection, Bounds sectionToIgnore)
    {
        var allBounds = FindObjectsOfType<Bounds>();
        foreach (Bounds b in allBounds)
        {
            if (b.col.bounds.Intersects(newSection.col.bounds) && !ReferenceEquals(b.GetComponentInParent<TileProperties>().gameObject, newSection.GetComponentInParent<TileProperties>().gameObject)) {
                print(b.GetComponentInParent<TileProperties>().gameObject.name + " intersected with " + newSection.GetComponentInParent<TileProperties>().gameObject.name);
                return false;
            }
        }
        return true;
        /*
        foreach (Collider col in LoadedColliders)
        {
            if (col.bounds.Intersects(newSection.Colliders.First().bounds))
            {
                return false;
            }
        }
        return true;*/
    }


    public void RegisterNewSection(OpenJoint newSection)
    {
        loadedJoints.Add(newSection);

        levelSize--;
    }

    public void RunGeneration()
    {
        foreach (OpenJoint joint in loadedJoints)
        {
            joint.Build(this);
        }
    }

    public bool ShouldSpawnDeadEnd()
    {
        if (Random.Range(0, randDeadEndChance) == 0)
        {
            return true;
        }
        return false;
    }

    public bool ShouldSpawnSpecialRoom()
    {
        if (specialRoomCount <= 0)
        {
            return false;
        }
        if (Random.Range(0, levelSize) == 0)
        {
            return true;
        }
        return false;
    }
    private void EndGeneration()
    {
        SendMessageUpwards("Block", deadEnd, SendMessageOptions.DontRequireReceiver);
    }
}
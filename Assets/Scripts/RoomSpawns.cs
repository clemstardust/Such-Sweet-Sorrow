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

    LoadSave loadSave;
    [Header("Loaded Level Joints")]
    [SerializeField] protected List<OpenJoint> loadedJoints = new List<OpenJoint>();
    protected IEnumerable<Collider> LoadedColliders => loadedJoints.SelectMany(s => s.Bounds.Colliders);

    void Awake()
    {
        loadSave = FindObjectOfType<LoadSave>();
        if (GameManager.loadFromSave)
        {
            loadSave.LoadGame();
        }
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
            i.GetComponent<OpenJoint>().Build(this);

        }
        foreach (var c in LoadedColliders)
            c.enabled = false;
        EndGeneration();
        NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();

        //RenderSettings.skybox = Skyboxes[GameManager.globalDarkness];
        
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
                if (levelSize <= maxLevelSize * 0.2f && GameObject.FindGameObjectWithTag("BossRoom") == null)
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
        
        foreach (Collider col in LoadedColliders)
        {
            if (col.bounds.Intersects(newSection.Colliders.First().bounds))
            {
                return false;
            }
        }
        return true;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OpenJoint : MonoBehaviour
{
    public bool isOpen = true;
    public Bounds Bounds;
    private GameObject deadEnd;
    private RoomSpawns rs;
    public void Awake()
    {   
        Bounds = gameObject.GetComponent<Bounds>();
        rs = FindObjectOfType<RoomSpawns>();
        deadEnd = rs.deadEnd;
    }
    public void Build(RoomSpawns roomSpawns)
    {
        if (roomSpawns.levelSize > 0 && isOpen)
        {
            roomSpawns.RegisterNewSection(this);
            OpenJoint newRoom = Instantiate(roomSpawns.NextRoom(this.GetComponentInParent<TileProperties>()), gameObject.transform.position, gameObject.transform.rotation).GetComponentInChildren<OpenJoint>();
            if (roomSpawns.ShouldSpawnDeadEnd() && roomSpawns.enableRandDeadends)
            {
                isOpen = false;
                GameObject newDeadEnd = Instantiate(deadEnd, this.transform.position, this.transform.rotation);
                newDeadEnd.transform.parent = FindObjectOfType<RoomSpawns>().gameObject.transform;
                //Destroy(newRoom.GetComponentInParent<TileProperties>().gameObject);
                newRoom.GetComponentInParent<TileProperties>().gameObject.SetActive(false);

            }
            else if (roomSpawns.IsSectionValid(newRoom.Bounds, Bounds))
            {
                newRoom.GetComponentInParent<TileProperties>().gameObject.transform.parent = FindObjectOfType<RoomSpawns>().gameObject.transform;
                var newJoints = newRoom.transform.parent.gameObject.GetComponentsInChildren<OpenJoint>();
                if (newJoints[0] != null)
                {
                    foreach (OpenJoint newJoint in newJoints)
                    {
                        newJoint.Build(roomSpawns);
                    }
                    isOpen = false;
                    roomSpawns.levelSize--;
                }
            }
            else
            {
                isOpen = false;
                GameObject newDeadEnd = Instantiate(deadEnd, gameObject.transform.position, gameObject.transform.rotation);
                newDeadEnd.transform.parent = FindObjectOfType<RoomSpawns>().gameObject.transform;
                newRoom.GetComponentInParent<TileProperties>().gameObject.SetActive(false);
                //Destroy(newRoom.GetComponentInParent<TileProperties>().gameObject);

            }
        }
        else
        {
            Block(deadEnd);
        }
    }

    public void Block(GameObject deadEnd)
    {
        if (isOpen)
        {
            isOpen = false;
            GameObject newDeadEnd = Instantiate(deadEnd, gameObject.transform.position, gameObject.transform.rotation);
            newDeadEnd.transform.parent = FindObjectOfType<RoomSpawns>().gameObject.transform;
        }
    }
}

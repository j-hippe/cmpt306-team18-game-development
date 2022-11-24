using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] doors;
    [Header("Only used for endrooms")]
    public GameObject Portal;
    [Header("Only used for endrooms")]
    public GameObject Shop;

    public GameObject[] spawners;

    public bool[] testStatus;


    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }
    public void UpdateRoom(bool[] status)
    {
        for(int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

    public void UpdateRotation(bool[] direction)
    {
        var currentPos = transform.position;
        if (CompareTag("endroom"))
        {
            
            // only room with 1 opening is the end room, rotate it so it faces towards the direction given
            // default is right
            if (direction[0])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -90, transform.rotation.z));
            }

            if (direction[1])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 90, transform.rotation.z));
            }

            if (direction[2])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
            }

            if (direction[3])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z));
            }


            // we dont need an else because default entrance faces east
        }
        else if (CompareTag("hallway"))
        {
            // only has 2 directions as exits face opposites
            if (direction[0] && direction[1])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
            }
            if (direction[2] && direction[3])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 90, transform.rotation.z));
            }
        }
        else if (CompareTag("Lroom"))
        {
            // has 4 situations for each possible bend
            // situation 1: up and right
            if(direction[0] && direction[2])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
                transform.position = new Vector3(currentPos.x, 0, currentPos.z + 21);
            }
            // situation 2: down and right
            if (direction[1] && direction[2])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 90, transform.rotation.z));
                transform.position = new Vector3(currentPos.x + 21, 0, currentPos.z);
            }
            // situation 3: down and left
            if (direction[1] && direction[3])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z));
                transform.position = new Vector3(currentPos.x, 0, currentPos.z - 21);
            }
            // situation 4: up and left
            if (direction[0] && direction[3])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -90, transform.rotation.z));
                transform.position = new Vector3(currentPos.x - 21, 0, currentPos.z);
            }
        }
        else if (CompareTag("Troom"))
        {
            // has 4 situations for each possible junction
            // situation 1: up, left and right
            if (direction[0] && direction[2] && direction[3])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
                transform.position = new Vector3(currentPos.x - 21, 0, currentPos.z);
            }
            // situation 2: up, down and right
            if (direction[0] && direction[1] && direction[2])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 90, transform.rotation.z));
                transform.position = new Vector3(currentPos.x, 0, currentPos.z + 21);
            }
            // situation 3: down, right and left
            if (direction[1] && direction[2] && direction[3])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z));
                transform.position = new Vector3(currentPos.x + 21, 0, currentPos.z);
            }
            // situation 4: up, down and left
            if (direction[0] && direction[1] && direction[3])
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, -90, transform.rotation.z));
                transform.position = new Vector3(currentPos.x, 0, currentPos.z - 21);
            }
        }
        else if (CompareTag("4wayroom"))
        {
            //doesnt need adjustment, just make sure its to zero
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
        }
    }

    public void CreatePortal()
    {
        if(CompareTag("endroom"))
            Instantiate(Portal, new Vector3(transform.position.x - 9, transform.position.y, transform.position.z), transform.rotation, transform);
            Debug.Log("Portal Added");
    }
    public void CreateShop()
    {
        if (CompareTag("endroom"))
            Instantiate(Shop, new Vector3(transform.position.x - 24.5f, transform.position.y, transform.position.z), transform.rotation, transform);
        
    }

}

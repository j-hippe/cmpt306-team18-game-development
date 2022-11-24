using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20.0f;
    [SerializeField] GameObject rotationTarget;
    [SerializeField] Rigidbody rb;
    private float downSpeed = 0.0f;
    private Vector3 mouse_pos;
    private Vector3 object_pos;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LookAtCamera();
    }

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.15f))
        {
            if (hit.collider.gameObject.CompareTag("Portal")){
                ReloadScene();
            }
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            //Debug.Log(hit.collider.gameObject.name);
            downSpeed = 0.0f;
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1.15f, Color.white);
            //Debug.Log("Did not Hit");
            downSpeed = -1f;
        }
    }

    private void Movement()
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), downSpeed, Input.GetAxis("Vertical"));
        /*if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        */
        rb.velocity = dir * moveSpeed;
    }

    private void LookAtCamera()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        object_pos = Camera.main.WorldToScreenPoint(rotationTarget.transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        rotationTarget.transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 90, 0));
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}


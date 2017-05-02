using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : NetworkBehaviour {

    // Use this for initialization
	void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }


    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float speed;
    public float jumpHeight;

    public Camera cam;
    //public MovementSettings movementSettings = new MovementSettings();
    //public MouseLook mouseLook = new MouseLook();

    // Update is called once per frame
    void Update ()
    {
        if (!isLocalPlayer)
            return;

        float translation = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        float straffe = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        transform.Translate(straffe, 0, translation);

        
        if (Input.GetButtonDown("Jump"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 velocity = rb.velocity;
            velocity.y = jumpHeight;
            rb.velocity = velocity;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            CmdFire();
        }


        /*
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;

        //RotateView();

        //transform.Rotate(0, x, 0);  // Need this?
        //transform.Translate(x, 0, z);
        transform.Translate(0, 0, z);

        //if (Input.GetKeyDown("escape"))
        //    Cursor.lockState = CursorLockMode.None;


        if (Input.GetButtonDown("Jump"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Vector3 velocity = rb.velocity;
            velocity.y = jumpHeight;
            rb.velocity = velocity;
        }

        if (Input.GetMouseButtonDown(0))
        {
            CmdFire();
        }
        */
    }

    /*
    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation(transform, cam.transform);

        if (m_IsGrounded || advancedSettings.airControl)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
        }
    }
    */

    [Command]
    private void CmdFire()
    {
        // Creat Bullet
        //GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, bulletSpawn.rotation);

        // Add Velocity
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6.0f;

        // Spawn the bullet on the clients
        NetworkServer.Spawn(bullet);

        // Destroy after 2 seconds
        Destroy(bullet, 2);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
}

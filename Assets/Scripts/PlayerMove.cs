using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float moveSpeed;
    public float maxSpeed;
    public float jumpForce;
    public float rotationSpeed;
    public float viewSpeed;
    public float fireRate;

    public static bool alive;

    public static int projAmmo;
    public int baskAmmo;

    public bool jumpState;

    public GameObject projectilePrefab;
    public GameObject basketballPrefab;
    private float lastShotTime = 0f;

    private float viewX;
    private float viewY;
    private Vector2 viewTurn;
    
    private Rigidbody rBody;
    private Vector3 inputVector;

    private AudioSource deathSound;


    // Start is called before the first frame update
    void Start()
    {
        alive = true;

        rBody = GetComponent<Rigidbody>();

        jumpState = false;

        Cursor.lockState = CursorLockMode.Locked;

        projAmmo = 5;

        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Creates a movement vector based on horizontal and vertical input keys (default WASD)
        //Then adds force to the rigidbody using this vector to allow movement
        inputVector = new Vector3(Input.GetAxis("Horizontal")*moveSpeed, 0, Input.GetAxis("Vertical")*moveSpeed);
        rBody.AddRelativeForce(inputVector, ForceMode.VelocityChange);

        //Manually adds gravity force back onto player after above force applied
        rBody.AddForce(Physics.gravity * 0.5f, ForceMode.Acceleration);

        //If player goes above the set maximum movement speed, sets the current speed to the max, while retaining y axis speed (jump/fall)
        if(rBody.velocity.magnitude > maxSpeed)
        {
            Vector3 temp = rBody.velocity.normalized * maxSpeed;
            temp.y = rBody.velocity.y;
            rBody.velocity = temp;
        }
        
        //When spacebar is pressed, initiates an impulse force on the player in the up direction, affected by the set jumpForce
        if (Input.GetKeyDown(KeyCode.Space) && !jumpState)
        {
            rBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpState = true;
        }
        
        //Sets vector values based on mouse input for rotating player/camera
        viewTurn.x += Input.GetAxis("Mouse X");
        viewTurn.y += Input.GetAxis("Mouse Y");

        //Rotates rigidbody based on above vector values
        rBody.MoveRotation(Quaternion.Euler(-viewTurn.y, viewTurn.x, 0));

        //If the left mouse button is clicked, the player has ammo, and the fire rate has passed, launches a projectile
        if (Input.GetMouseButtonDown(0) && (Time.time > lastShotTime + fireRate) && projAmmo > 0)
        {

            //Sets vector for spawnpoint of projectile in forward direction from player location
            Vector3 projSpawn = rBody.transform.position + (rBody.transform.forward * 2);

            //Initiates an instance of the set projectilePrefab at the set spawn
            GameObject proj = Instantiate(projectilePrefab, projSpawn, Quaternion.identity);

            //Adds force in the forward direction to projectile to shoot it
            proj.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 50, ForceMode.VelocityChange);

            lastShotTime = Time.time;

            projAmmo--;
        }

        //If the right mouse button is clicked and the fire rate has passed, launches a basketball
        if (Input.GetMouseButtonDown(1) && (Time.time > lastShotTime + fireRate))
        {
            //Same process as the projectile spawning above
            Vector3 projSpawn = rBody.transform.position + (rBody.transform.forward * 2);
            GameObject proj = Instantiate(basketballPrefab, projSpawn, Quaternion.identity);
            proj.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 100, ForceMode.VelocityChange);

            lastShotTime = Time.time;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        //If player collides with an enemy, GameOver function called
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            deathSound.Play();
            alive = false;
            Destroy(this);
        }

        //If player is colliding with the ground, removes jump state from player allowing another jump
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            jumpState = false;
        }

    }


}

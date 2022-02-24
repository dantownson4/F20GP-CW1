using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public HingeJoint joint;
    private bool isHit = false;
    private AudioSource beep;

    // Start is called before the first frame update
    void Start()
    {
        beep = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        //If the target is hit by a projectile, destroys the hinge joint between the target and the "rope" and adds user score
        if (collision.collider.CompareTag("Projectile") && !isHit)
        {
            Destroy(joint);
            UIManager.score += 20;
            isHit = true;
            beep.Play();
        }
    }
}

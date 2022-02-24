using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyGet : MonoBehaviour
{

    private bool taken = false;
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

        //If player collides with trophy object, adds score and removes object
        if (collision.collider.CompareTag("Player") && !taken)
        {
            taken = true;
            UIManager.score += 50;
            Destroy(this.gameObject);
            beep.Play();
        }
    }
}

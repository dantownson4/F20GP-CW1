using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballScore : MonoBehaviour
{

    private bool hasScored = false;
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

        //If a basketball object hits the hoop object, adds score
        if (!hasScored && collision.collider.CompareTag("Basketball"))
        {
            UIManager.score += 25;
            hasScored = true;
            beep.Play();
        }
    }
}

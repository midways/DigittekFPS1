using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fysiskverden {
public class Enemy : MonoBehaviour
{

    private Gamecontrol script;


    void Start()
    {
        script = FindObjectOfType<Gamecontrol>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Tjekker om et skud rammer keglen. Hvis den rammer bliver keglen destrueret.
    void OnCollisionEnter(Collision other){
            print ("Noget ramte mig: " + other.gameObject.tag);
            if (other.gameObject.CompareTag ("skud")) {
                script.AddScore();
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}
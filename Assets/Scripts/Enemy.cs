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

                //Konvertering af rgb værdi til xy værdi 
                Color keglefarve =gameObject.transform.GetComponent<Renderer>().material.color;
                //Tal kommer fra https://developers.meethue.com/develop/application-design-guidance/color-conversion-formulas-rgb-to-xy-and-back/#Color-rgb-to-xy
                float red = (keglefarve.r > 0.04045f) ? Mathf.Pow((keglefarve.r + 0.055f) / (1.0f + 0.055f), 2.4f) : (keglefarve.r / 12.92f);
                float green = (keglefarve.g > 0.04045f) ? Mathf.Pow((keglefarve.g + 0.055f) / (1.0f + 0.055f), 2.4f) : (keglefarve.g / 12.92f);
                float blue = (keglefarve.b > 0.04045f) ? Mathf.Pow((keglefarve.b + 0.055f) / (1.0f + 0.055f), 2.4f) : (keglefarve.b / 12.92f);
                float X = red * 0.4124f + green * 0.3576f + blue * 0.1805f;
                float Y = red * 0.2126f + green * 0.7152f + blue * 0.0722f;
                float Z = red * 0.0193f + green * 0.1192f + blue * 0.9505f;
                float x = X / (X + Y + Z);
                float y = Y / (X + Y + Z);
                script.AddScore(x, y);
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
        }
    }
}
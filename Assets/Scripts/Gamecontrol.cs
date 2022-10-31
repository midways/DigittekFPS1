using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Fysiskverden {
public class Gamecontrol : MonoBehaviour
{
    public GameObject kegle;

    private int score = 0;
    public TMP_Text scoreText;
    private ApiHandler apiScript;
    public uint lampe;

 

    // Start is called before the first frame update
    void Start()
    {
        apiScript = GetComponent<ApiHandler>();

    }

    // Update is called once per frame
    void Update()
    {
   
    }

    //Tilføjer point til den totale score, Blinker vores lampe og spawner en ny kegle.
    public void AddScore() {
        score += 1;
        scoreText.text = "Score: " + score;
        apiScript.Blink(lampe);
        StartCoroutine(nykegle());
    }

    //Spawner en ny kegle.
    IEnumerator nykegle(){
        yield return new WaitForSeconds(1);
        Vector3 position = new Vector3(Random.Range(39.19F, -1.06F), 0.75F, Random.Range(40.16F, 5.26F));
        Instantiate(kegle, position, transform.rotation);
    }
}
}

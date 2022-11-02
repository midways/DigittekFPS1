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
    public void AddScore(float x, float y) {
        Color randomfarve = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1f);
        score += 1;
        scoreText.text = "Score: " + score;
        apiScript.ColorSetup(lampe, x, y);
        StartCoroutine(nykegle(randomfarve));
    }

    //Spawner en ny kegle.
    IEnumerator nykegle(Color32 nyfarve){
        
        yield return new WaitForSeconds(1);
        Vector3 position = new Vector3(Random.Range(39.19F, -1.06F), 0.75F, Random.Range(40.16F, 5.26F));
        var nykegle = (GameObject)Instantiate(kegle, position, transform.rotation);
        nykegle.transform.GetComponent<Renderer>().material.color = nyfarve;
    }
}
}

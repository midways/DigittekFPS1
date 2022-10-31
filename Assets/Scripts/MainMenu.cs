using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fysiskverden {
public class MainMenu : MonoBehaviour
{
    //Skifter scenen til selve spillet
    public void StartGame() {
        SceneManager.LoadScene("Spil");
    }

    //Lukker spillet
    public void LukSpil() {
        Application.Quit();
    }
}
}

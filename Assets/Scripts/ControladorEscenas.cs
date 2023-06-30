using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorEscenas : MonoBehaviour
{
    public void CargarEscena()
    {
        SceneManager.LoadScene("Juego");
    }
}

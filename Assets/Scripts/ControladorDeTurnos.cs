using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorDeTurnos : MonoBehaviour
{
    public int turno = 0;
    public Animator transicion;
    public GameObject pantallaFinal;
    public Image mensajeFinal;
    public Sprite[] mensajes;
    public GameObject partidaCerradaGO;
    public GameObject[] sumasFichasGO;
    public ElegirFichaJugador jugador;
    public ElegirFichaMaquina[] maquinas;
    public AudioSource tambores;
    public int turnosPasados = 0;
    public bool finPartida = false;

    // Asigna como primer turno el parametro que le llegue y llama a "SiguienteTurno" que lo inicia
    public void EstablecerInicio(int t)
    {
        turno = t;
        if (turno == 0)
        {
            jugador.turno = true;
            jugador.primerTurno = true;
        }
        else
            maquinas[turno - 1].primerTurno = true;
        SiguienteTurno(false);
    }

    // llama al metodo de comenzar turno del jugador actual, si el turno se pasó lo suma a una variable con la que se estará revisando si el juego se ahoga
    public void SiguienteTurno(bool pasoTurno)
    {

        if (pasoTurno)
        {
            turnosPasados++;
            if (turnosPasados >= 4)
            {
                Debug.Log("Juego Ahogado");
                PartidaCerrada();
            }
        }
        else
        {
            turnosPasados = 0;
        }

        if (finPartida == false)
        {
            if (turno == 4)
                turno = 0;

            if (turno == 0)
                jugador.ComenzarTurno();
            else
                maquinas[turno - 1].ComenzarTurno();

            turno++;
        }

    }

    // Se llamará cuando ninguno de los jugadores pueda mover (4 saltos de turno seguidos)
    public void PartidaCerrada()
    {
        finPartida = true;
        int indexGanador = 0;
        int valorGanador = maquinas[0].ContarValorFichas();

        // Revisa las sumas de las fichas de cada jugador y encuentra al que tenga menor valor
        for (int i = 0; i < 3; i++)
        {
            int s = maquinas[i].ContarValorFichas();
            if (s < valorGanador)
            {
                indexGanador = i;
                valorGanador = s;
            }
            sumasFichasGO[i].GetComponent<TMP_Text>().text = s.ToString();
        }

        int sJugador = jugador.ContarValorFichas();
        if (sJugador <= valorGanador)
        {
            indexGanador = 4;
            valorGanador = sJugador;
        }

        sumasFichasGO[3].GetComponent<TMP_Text>().text = jugador.ContarValorFichas().ToString();

        partidaCerradaGO.SetActive(true);

        tambores.Play();

        StartCoroutine(DelayMostrarSumas(indexGanador));
    }

    private IEnumerator DelayMostrarSumas(int iGanador)
    {
        for (int i = 0; i < sumasFichasGO.Length; i++)
        {
            yield return new WaitForSeconds(1);
            sumasFichasGO[i].SetActive(true);
        }

        partidaCerradaGO.SetActive(false);
        if (iGanador == 4)
            jugador.Victoria();
        else
        {
            maquinas[iGanador].VictoriaMaquina();
        }
    }

    public void FinDelJuego(int resultado)  // 0: Victoria, 1: Derrota
    {
        finPartida = true;
        mensajeFinal.sprite = mensajes[resultado];
        pantallaFinal.SetActive(true);
    }
}

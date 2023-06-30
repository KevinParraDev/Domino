using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElegirFichaMaquina : MonoBehaviour
{
    public Domino domino;
    public ControladorDeTurnos controladorDeTurnos;
    public int numeroDeMaquina;
    public GameObject[] _GOFichasMaquina;
    public AudioSource sonidoDerrota;

    public SpriteRenderer spriteReaccion;
    public Sprite[] reacciones;
    private int[] fichaInicial = new int[2] { 6, 6 };
    public bool primerTurno = false;
    private bool pasoTurno = false;


    public void ComenzarTurno()
    {
        pasoTurno = false;
        StartCoroutine(DelayTurno());
    }

    // Espera 1 segundo y luego llama a elegir ficha
    public IEnumerator DelayTurno()
    {
        spriteReaccion.sprite = reacciones[1];
        yield return new WaitForSeconds(1);

        if (primerTurno)
        {
            primerTurno = false;
            SeleccionarPrimeraFicha();
        }
        else
            ElegirFicha();
    }

    // Se llama cuando acaba el turno, sirve para cambiar el emogi y llamar al siguiente turno
    public IEnumerator ResetReaccion()
    {
        yield return new WaitForSeconds(1f);
        spriteReaccion.sprite = reacciones[0];
        controladorDeTurnos.SiguienteTurno(pasoTurno);
    }

    // Este metodo selecciona una ficha y la coloca en juego
    private void ElegirFicha()
    {
        Debug.Log("Seleccionar ficha normal");
        List<int> fichasEnJuego = domino.GetFichasEnJuego();

        bool encontroFicha = false;

        // Busca cuales son las ultimas fichas fuestas agregadas al domino en juego
        int cabeza = fichasEnJuego[0];
        int cola = fichasEnJuego[^1];

        // Poner 2 pares

        List<int> iPares = new List<int>();
        int cantidadPares = 0;

        // Revisa si tiene pares que pueda poner en el juego
        for (int i = 0; i < domino._fichasMaquina1.Length; i++)
        {
            int a = _GOFichasMaquina[i].GetComponent<Ficha>().valorFicha[0];
            int b = _GOFichasMaquina[i].GetComponent<Ficha>().valorFicha[1];

            if ((a == b && (b == cabeza || b == cola)) && _GOFichasMaquina[i].GetComponent<Ficha>().usada == false)
            {
                cantidadPares++;
                iPares.Add(i);
            }
        }

        // Si tiene 2 pares los va a colocar
        if (cantidadPares == 2)
        {
            encontroFicha = true;
            _GOFichasMaquina[iPares[0]].GetComponent<Ficha>().MoverFicha();
            _GOFichasMaquina[iPares[1]].GetComponent<Ficha>().MoverFicha();

            spriteReaccion.sprite = reacciones[2];
        }
        else    // Si no tiene 2 pares colocará simplemente una, para eso revisará cual puede poner
        {
            //Poner una sola ficha

            for (int i = 0; i < domino._fichasMaquina1.Length; i++)
            {
                int a = _GOFichasMaquina[i].GetComponent<Ficha>().valorFicha[0];
                int b = _GOFichasMaquina[i].GetComponent<Ficha>().valorFicha[1];

                if ((a == cabeza || b == cabeza || a == cola || b == cola) && _GOFichasMaquina[i].GetComponent<Ficha>().usada == false)
                {
                    encontroFicha = true;
                    _GOFichasMaquina[i].GetComponent<Ficha>().MoverFicha();
                    spriteReaccion.sprite = reacciones[2];
                    break;
                }
            }
        }

        // Si no tiene fichas para mover saltará el turno

        if (!encontroFicha)
        {
            spriteReaccion.sprite = reacciones[3];
            pasoTurno = true;
            Debug.Log("La maquina no tiene fichas para colocar");
        }

        // Verifica si con el ultimo movimiento que realizó se quedó sin fichas
        if (VerificarGanador())
        {
            VictoriaMaquina();
        }
        else
            StartCoroutine(ResetReaccion());
    }

    // Se detiene el juego y saltará las correspondientes animaciones en unity
    public void VictoriaMaquina()
    {
        Debug.Log("La maquina " + numeroDeMaquina + " gano la partida");
        sonidoDerrota.Play();
        spriteReaccion.gameObject.GetComponent<Animator>().enabled = true;
        controladorDeTurnos.FinDelJuego(1);
    }

    // Revisa si todas las fichas de la maquina fueron usadas
    bool VerificarGanador()
    {
        bool gano = true;
        for (int i = 0; i < _GOFichasMaquina.Length; i++)
        {
            if (_GOFichasMaquina[i].GetComponent<Ficha>().usada == false)
            {
                gano = false;
                break;
            }
        }

        return gano;
    }

    // Solo se llama cuando tiene el [6,6], aqui se revisa en que posicion la tiene y lo coloca en el juego
    public void SeleccionarPrimeraFicha()
    {
        Debug.Log("Seleccionar primera ficha");
        for (int i = 0; i < _GOFichasMaquina.Length; i++)
        {
            if (_GOFichasMaquina[i].GetComponent<Ficha>().valorFicha[0] == 6 && _GOFichasMaquina[i].GetComponent<Ficha>().valorFicha[1] == 6)
            {
                Debug.Log("Primera ficha");
                _GOFichasMaquina[i].GetComponent<Ficha>().MoverFicha(1);
                spriteReaccion.sprite = reacciones[2];
            }
        }
        StartCoroutine(ResetReaccion());
    }

    // Sirve para preguntar la suma de los puntos de la ficha y declarar al ganador cuando la partida se cierra, es decir, ningun jugador puede mover
    public int ContarValorFichas()
    {
        int suma = 0;

        // Revisa cada valor de las fichas que no se han usado y los suma
        for (int i = 0; i < domino._fichasMaquina1.Length; i++)
        {
            int a = _GOFichasMaquina[i].GetComponent<Ficha>().valorFicha[0];
            int b = _GOFichasMaquina[i].GetComponent<Ficha>().valorFicha[1];

            if (_GOFichasMaquina[i].GetComponent<Ficha>().usada == false)
            {
                suma += a + b;
            }
        }

        return suma;
    }
}

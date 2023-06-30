using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElegirFichaJugador : MonoBehaviour
{
    public Domino domino;
    public ControladorDeTurnos controladorDeTurnos;
    public bool turno;
    public bool primerTurno;
    public AudioSource sonidoVictoria;
    public SpriteRenderer spriteReaccion;
    public Sprite[] reacciones;
    public GameObject[] _GOFichasJugador;
    private bool pasoTurno = false;

    public void ComenzarTurno()
    {
        spriteReaccion.sprite = reacciones[1];
        turno = true;
        pasoTurno = false;
    }

    // Se llama cuando acaba el turno, sirve para cambiar el emogi y llamar al siguiente turno
    public IEnumerator ResetReaccion()
    {
        yield return new WaitForSeconds(1f);
        spriteReaccion.sprite = reacciones[0];
        controladorDeTurnos.SiguienteTurno(pasoTurno);
    }

    // Este metodo se llama despues de que el jugador suelta la ficha en alguno de los lados del dominó, verifica si el movimiento es valido y si no lo es, retorna un false que hará que se resetee la posicion de la ficha
    public bool VerificarValidezDeMovimiento(GameObject ficha, string dir)
    {
        List<int> fichasEnJuego = domino.GetFichasEnJuego();

        int cabeza = fichasEnJuego[0];
        int cola = fichasEnJuego[^1];

        int a = ficha.GetComponent<Ficha>().valorFicha[0];
        int b = ficha.GetComponent<Ficha>().valorFicha[1];

        Debug.Log("Direccion: " + dir);

        // Revisa si la ficha que colocó tiene un numero igual a alguno de los dos extremos del dominó en juego (cola y cabeza)
        if ((dir == "Cabeza" && (a == cabeza || b == cabeza)) || (dir == "Cola" && (a == cola || b == cola)))
        {
            //Mueve la ficha al lado que se le haya dicho al llamar el metodo
            if (dir == "Cabeza")
                ficha.GetComponent<Ficha>().MoverFicha("Cabeza");
            else
                ficha.GetComponent<Ficha>().MoverFicha("Cola");

            spriteReaccion.sprite = reacciones[2];

            // Revisa si en el ultimo movimiento se quedó sin fichas
            if (VerificarGanador())
            {
                Victoria();
            }
            else
            {
                // Si colocó un doble (revisa que los dos numeros de la ficha colocada sean iguales)
                if (a == b)
                {
                    //Revisa si tiene otro par que pueda poner y lo coloca
                    for (int i = 0; i < domino._fichasMaquina1.Length; i++)
                    {
                        int x = _GOFichasJugador[i].GetComponent<Ficha>().valorFicha[0];
                        int y = _GOFichasJugador[i].GetComponent<Ficha>().valorFicha[1];

                        if ((x == y && (y == cabeza || y == cola)) && _GOFichasJugador[i].GetComponent<Ficha>().usada == false)
                        {
                            _GOFichasJugador[i].GetComponent<Ficha>().MoverFicha();
                        }
                    }
                }

                StartCoroutine(ResetReaccion());
            }

            turno = false;
            return true;
        }
        else
            return false;
    }

    // Se detiene el juego y saltará las correspondientes animaciones en unity
    public void Victoria()
    {
        Debug.Log("Has ganado la partida");
        sonidoVictoria.Play();
        spriteReaccion.gameObject.GetComponent<Animator>().enabled = true;
        controladorDeTurnos.FinDelJuego(0);
    }

    // Mismo metodo de arriba sobrecargado para el caso de que el jugador tenga el [6,6]
    public bool VerificarValidezDeMovimiento(GameObject ficha)
    {
        int a = ficha.GetComponent<Ficha>().valorFicha[0];
        int b = ficha.GetComponent<Ficha>().valorFicha[1];
        if (a == 6 && b == 6)
        {
            ficha.GetComponent<Ficha>().MoverFicha(1);
            spriteReaccion.sprite = reacciones[2];
            StartCoroutine(ResetReaccion());
            turno = false;
            return true;
        }
        else
            return false;
    }

    // Revisa si todas las fichas de la maquina fueron usadas
    bool VerificarGanador()
    {
        bool gano = true;
        for (int i = 0; i < _GOFichasJugador.Length; i++)
        {
            if (_GOFichasJugador[i].GetComponent<Ficha>().usada == false)
            {
                gano = false;
                break;
            }
        }

        return gano;
    }

    // Se llama al clickear el boton en unity, llamará al siguiente turno
    public void PasarTurno()
    {
        spriteReaccion.sprite = reacciones[3];
        pasoTurno = true;
        StartCoroutine(ResetReaccion());
    }

    // Sirve para preguntar la suma de los puntos de la ficha y declarar al ganador cuando la partida se cierra, es decir, ningun jugador puede mover
    public int ContarValorFichas()
    {
        int suma = 0;
        for (int i = 0; i < domino._GOFichasJugador.Length; i++)
        {
            int a = _GOFichasJugador[i].GetComponent<Ficha>().valorFicha[0];
            int b = _GOFichasJugador[i].GetComponent<Ficha>().valorFicha[1];

            if (_GOFichasJugador[i].GetComponent<Ficha>().usada == false)
            {
                suma += (a + b);
            }
        }

        return suma;
    }
}

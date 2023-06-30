using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domino : MonoBehaviour
{
    public ControladorDeTurnos controladorDeTurnos;
    public Camera _cam;
    public AudioSource knockAudio;
    public Sprite[] _spritesFichasH;
    public Sprite[] _spritesFichasV;
    public int[] ordenFichas = new int[28] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 };
    int[][] _todasLasFichas = new int[28][];
    public List<int> _fichasEnJuego = new List<int>() { 6, 6 };


    int[][] _fichasJugador = new int[7][];
    public GameObject[] _GOFichasJugador;

    public int[][] _fichasMaquina1 = new int[7][];
    public GameObject[] _GOFichasMaquina1;

    public int[][] _fichasMaquina2 = new int[7][];
    public GameObject[] _GOFichasMaquina2;

    public int[][] _fichasMaquina3 = new int[7][];
    public GameObject[] _GOFichasMaquina3;

    public GameObject _fichaCola;
    public GameObject _fichaCabeza;

    public int contadorCabeza = 0;
    public int contadorCola = 0;


    // Es lo primero que se ejecuta al iniciar el juego, primero se revuelven las cartas, se construyen las fichas y luego se reparten
    void Start()
    {
        _cam = Camera.main;

        RevolverJuego(ordenFichas);
        ConstruirFichas();
        RepartirFichas();
    }

    //Crea un arreglo de enteros de dos posiciones (int[2]) y le asigna los valores de la ficha
    void ConstruirFichas()
    {
        int aux = 0;
        int contador = 0;
        for (int i = 0; i <= 6; i++)
        {
            for (int e = aux; e <= 6; e++)
            {
                _todasLasFichas[contador] = new int[2] { i, e };

                //-------------- Para pruebas ----------------------

                // _todasLasFichas[contador] = new int[2] { 0, 0 };

                //--------------------------------------------------

                print("[" + _todasLasFichas[contador][0] + " - " + _todasLasFichas[contador][1] + "]");
                contador++;
            }
            aux++;
        }
    }

    // Desordena el  arreglo de enteros que contiene el orden de cartas para repartir
    void RevolverJuego(int[] arreglo)
    {
        int n = arreglo.Length;
        int randomValue;
        int temp;
        for (int i = 0; i < n; i++)
        {
            randomValue = Random.Range(0, n);
            temp = arreglo[randomValue];
            arreglo[randomValue] = arreglo[i];
            arreglo[i] = temp;
        }
    }

    // Le asigna a cada jugador 7 fichas y se modifican los valores de la ficha del objeto y el sprite que se usará en unity
    void RepartirFichas()
    {

        for (int i = 0; i < 7; i++)
        {
            _fichasMaquina1[i] = new int[2];
            _fichasMaquina1[i] = _todasLasFichas[ordenFichas[i + 7]];
            _GOFichasMaquina1[i].GetComponent<Ficha>().valorFicha = _todasLasFichas[ordenFichas[i + 7]];
            _GOFichasMaquina1[i].GetComponent<Ficha>().spriteFicha = _spritesFichasV[ordenFichas[i + 7]];
            _GOFichasMaquina1[i].GetComponent<Ficha>().knockSound = knockAudio;

            _fichasMaquina2[i] = new int[2];
            _fichasMaquina2[i] = _todasLasFichas[ordenFichas[i + 14]];
            _GOFichasMaquina2[i].GetComponent<Ficha>().valorFicha = _todasLasFichas[ordenFichas[i + 14]];
            _GOFichasMaquina2[i].GetComponent<Ficha>().spriteFicha = _spritesFichasV[ordenFichas[i + 14]];
            _GOFichasMaquina2[i].GetComponent<Ficha>().knockSound = knockAudio;

            _fichasMaquina3[i] = new int[2];
            _fichasMaquina3[i] = _todasLasFichas[ordenFichas[i + 21]];
            _GOFichasMaquina3[i].GetComponent<Ficha>().valorFicha = _todasLasFichas[ordenFichas[i + 21]];
            _GOFichasMaquina3[i].GetComponent<Ficha>().spriteFicha = _spritesFichasV[ordenFichas[i + 21]];
            _GOFichasMaquina3[i].GetComponent<Ficha>().knockSound = knockAudio;

            _fichasJugador[i] = new int[2];
            _fichasJugador[i] = _todasLasFichas[ordenFichas[i]];
            _GOFichasJugador[i].GetComponent<Ficha>().valorFicha = _todasLasFichas[ordenFichas[i]];
            _GOFichasJugador[i].GetComponent<Ficha>().spriteFicha = _spritesFichasV[ordenFichas[i]];
            _GOFichasJugador[i].GetComponent<SpriteRenderer>().sprite = _spritesFichasV[ordenFichas[i]];
            _GOFichasJugador[i].GetComponent<Ficha>().knockSound = knockAudio;
        }

        SeleccionarPrimerTurno();
    }

    // Busca en donde se encuentra la ficha 27 [6,6] para decidir quien tendrá el primer turno
    void SeleccionarPrimerTurno()
    {
        // controladorDeTurnos.EstablecerInicio(0);
        for (int i = 0; i < ordenFichas.Length; i++)
        {
            if (ordenFichas[i] == 27 && i < 7)
                controladorDeTurnos.EstablecerInicio(0);
            else if (ordenFichas[i] == 27 && i < 14)
                controladorDeTurnos.EstablecerInicio(1);
            else if (ordenFichas[i] == 27 && i < 21)
                controladorDeTurnos.EstablecerInicio(2);
            else if (ordenFichas[i] == 27 && i < 28)
                controladorDeTurnos.EstablecerInicio(3);
        }
    }

    // Coloca la ficha del domino en el lado que le llega como parametro y la ficha se agrega a la lista de fichas en juego
    public string AgregarFicha(int[] ficha, string posicion)
    {
        int cabeza = _fichasEnJuego[0];
        int cola = _fichasEnJuego[^1];

        Debug.Log("Cabeza: " + cabeza + " Cola: " + cola);

        int a = ficha[0];
        int b = ficha[1];

        if (posicion == "inicio")
        {
            if (a == cabeza)
            {
                _fichasEnJuego.Insert(0, a);
                _fichasEnJuego.Insert(0, b);

                return "a";
            }
            else if (b == cabeza)
            {
                _fichasEnJuego.Insert(0, b);
                _fichasEnJuego.Insert(0, a);
                return "b";
            }
            else
            {
                Debug.Log("Movimiento invalido");
                return "Movimiento invalido";
            }
        }
        else if (posicion == "final")
        {
            if (a == cola)
            {
                _fichasEnJuego.Add(a);
                _fichasEnJuego.Add(b);
                return "a";
            }
            else if (b == cola)
            {
                _fichasEnJuego.Add(b);
                _fichasEnJuego.Add(a);
                return "b";
            }
            else
            {
                Debug.Log("Movimiento invalido");
                return "Movimiento invalido";
            }
        }
        else
        {
            Debug.Log("Movimiento invalido");
            return "Movimiento invalido";
        }
    }

    // Para pruevas, imprime las fichas en consola
    private void ImprimirJuego(List<int> fichas)
    {
        Debug.Log("------------------------------");
        for (int i = 0; i < fichas.Count; i++)
        {
            Debug.Log(fichas[i]);
        }
        Debug.Log("------------------------------");
    }

    public GameObject GetFicha(string dir)
    {
        if (dir == "Cabeza")
            return _fichaCabeza;
        else if (dir == "Cola")
            return _fichaCola;
        else
            return null;
    }

    public void SetFicha(string dir, GameObject ficha)
    {
        if (dir == "Cabeza")
        {
            _fichaCabeza = ficha;
            contadorCabeza++;
        }
        else if (dir == "Cola")
        {
            _fichaCola = ficha;
            contadorCola++;
        }
        else
            Debug.Log("Error Seteando la ficha");
    }

    public List<int> GetFichasEnJuego()
    {
        return _fichasEnJuego;
    }
}

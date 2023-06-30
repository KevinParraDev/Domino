using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverFicha : MonoBehaviour
{
    public Domino domino;
    public ElegirFichaJugador elegirFichaJugador;

    private bool _moviento;
    private Vector3 _posicionInicial;
    private Vector2 _resetPosition;

    void Start()
    {
        _resetPosition = transform.position;
    }

    void Update()
    {
        if (_moviento)
        {
            transform.position = PosicionDelMouse() - _posicionInicial;
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && elegirFichaJugador.turno)
        {
            _posicionInicial = PosicionDelMouse() - transform.position;
            _moviento = true;
        }
    }

    private Vector3 PosicionDelMouse()
    {
        Vector3 mousePos;
        mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseUp()
    {
        if (elegirFichaJugador.primerTurno)
        {
            _moviento = false;
            if (Mathf.Abs(transform.position.x) <= 1f && Mathf.Abs(transform.position.y) <= 1f)
            {
                Debug.Log("Primer movimiento jugador");
                if (!elegirFichaJugador.VerificarValidezDeMovimiento(this.gameObject))
                    transform.position = _resetPosition;
                else
                    elegirFichaJugador.primerTurno = false;
            }
            else
                transform.position = _resetPosition;
        }
        else
        {
            _moviento = false;
            GameObject _fichaCabeza = domino.GetFicha("Cabeza");
            GameObject _fichaCola = domino.GetFicha("Cola");

            if (Mathf.Abs(transform.position.x - _fichaCabeza.transform.position.x) <= 1f &&
                Mathf.Abs(transform.position.y - _fichaCabeza.transform.position.y) <= 1f)
            {
                Debug.Log("Se movio a cabeza");
                if (!elegirFichaJugador.VerificarValidezDeMovimiento(this.gameObject, "Cabeza"))
                    transform.position = _resetPosition;
            }
            else if (Mathf.Abs(transform.position.x - _fichaCola.transform.position.x) <= 1f &&
                Mathf.Abs(transform.position.y - _fichaCola.transform.position.y) <= 1f)
            {
                Debug.Log("Se movio a cola");
                if (!elegirFichaJugador.VerificarValidezDeMovimiento(this.gameObject, "Cola"))
                    transform.position = _resetPosition;
            }
            else
                transform.position = _resetPosition;
        }
    }
}

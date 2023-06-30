using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour
{
    public Domino domino;
    public Sprite spriteFicha;
    public AudioSource knockSound;
    public bool usada = false;
    public int[] valorFicha = new int[2] { 0, 0 };

    public void MoverFicha()
    {
        Debug.Log("Mover ficha maquina");
        List<int> fichasEnJuego = domino.GetFichasEnJuego();

        GameObject _fichaCabeza = domino.GetFicha("Cabeza");
        GameObject _fichaCola = domino.GetFicha("Cola");

        int cabeza = fichasEnJuego[0];
        int cola = fichasEnJuego[^1];

        if (valorFicha[0] == cabeza || valorFicha[1] == cabeza)
        {
            string r = domino.AgregarFicha(valorFicha, "inicio");

            if (domino.contadorCabeza <= 5)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, 90);

                transform.position = _fichaCabeza.transform.position + new Vector3(-1, 0, 0);
            }
            else if (domino.contadorCabeza > 5 && domino.contadorCabeza < 8)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                Debug.Log("Voltear: " + domino.contadorCola);
                if (domino.contadorCabeza == 6)
                    transform.position = _fichaCabeza.transform.position + new Vector3(-0.25f, 0.75f, 0);
                else
                    transform.position = _fichaCabeza.transform.position + new Vector3(0, 1, 0);
            }
            else if (domino.contadorCabeza >= 8)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, -90);

                if (domino.contadorCabeza == 8)
                    transform.position = _fichaCabeza.transform.position + new Vector3(0.25f, 0.75f, 0);
                else
                    transform.position = _fichaCabeza.transform.position + new Vector3(1, 0, 0);
            }
            domino.SetFicha("Cabeza", this.gameObject);
        }
        else if (valorFicha[0] == cola || valorFicha[1] == cola)
        {
            string r = domino.AgregarFicha(valorFicha, "final");

            if (domino.contadorCola <= 5)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, -90);

                transform.position = _fichaCola.transform.position + new Vector3(1, 0, 0);
            }
            else if (domino.contadorCola > 5 && domino.contadorCola < 8)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, 180);

                Debug.Log("Voltear: " + domino.contadorCola);
                if (domino.contadorCola == 6)
                    transform.position = _fichaCola.transform.position + new Vector3(0.25f, -0.75f, 0);
                else
                    transform.position = _fichaCola.transform.position + new Vector3(0, -1, 0);
            }
            else if (domino.contadorCola >= 8)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, 90);

                if (domino.contadorCola == 8)
                    transform.position = _fichaCola.transform.position + new Vector3(-0.25f, -0.75f, 0);
                else
                    transform.position = _fichaCola.transform.position + new Vector3(-1, 0, 0);
            }
            domino.SetFicha("Cola", this.gameObject);

        }
        GetComponent<SpriteRenderer>().sprite = spriteFicha;
        knockSound.Play();
        usada = true;
    }

    public void MoverFicha(string dir)
    {
        Debug.Log("Mover ficha jugador");
        List<int> fichasEnJuego = domino.GetFichasEnJuego();

        GameObject _fichaCabeza = domino.GetFicha("Cabeza");
        GameObject _fichaCola = domino.GetFicha("Cola");

        if (dir == "Cabeza")
        {
            string r = domino.AgregarFicha(valorFicha, "inicio");

            if (domino.contadorCabeza <= 5)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, 90);

                transform.position = _fichaCabeza.transform.position + new Vector3(-1, 0, 0);
            }
            else if (domino.contadorCabeza > 5 && domino.contadorCabeza < 8)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                if (domino.contadorCabeza == 6)
                    transform.position = _fichaCabeza.transform.position + new Vector3(-0.25f, 0.75f, 0);
                else
                    transform.position = _fichaCabeza.transform.position + new Vector3(0, 1, 0);
            }
            else if (domino.contadorCabeza >= 8)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, -90);

                if (domino.contadorCabeza == 8)
                    transform.position = _fichaCabeza.transform.position + new Vector3(0.25f, 0.75f, 0);
                else
                    transform.position = _fichaCabeza.transform.position + new Vector3(1, 0, 0);
            }

            domino.SetFicha("Cabeza", this.gameObject);
            GetComponent<SpriteRenderer>().sprite = spriteFicha;
            knockSound.Play();
            usada = true;
        }
        else if (dir == "Cola")
        {
            string r = domino.AgregarFicha(valorFicha, "final");

            if (domino.contadorCola <= 5)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, -90);

                transform.position = _fichaCola.transform.position + new Vector3(1, 0, 0);
            }
            else if (domino.contadorCola > 5 && domino.contadorCola < 8)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, 180);

                if (domino.contadorCola == 6)
                    transform.position = _fichaCola.transform.position + new Vector3(0.25f, -0.75f, 0);
                else
                    transform.position = _fichaCola.transform.position + new Vector3(0, -1, 0);
            }
            else if (domino.contadorCola >= 8)
            {
                if (r == "a")
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                else if (r == "b")
                    transform.rotation = Quaternion.Euler(0, 0, 90);

                if (domino.contadorCola == 8)
                    transform.position = _fichaCola.transform.position + new Vector3(-0.25f, -0.75f, 0);
                else
                    transform.position = _fichaCola.transform.position + new Vector3(-1, 0, 0);
            }

            domino.SetFicha("Cola", this.gameObject);
            GetComponent<SpriteRenderer>().sprite = spriteFicha;
            knockSound.Play();
            usada = true;
        }
    }

    public void MoverFicha(int r)
    {
        Debug.Log("Mover primera ficha");
        transform.position = new Vector3(0, 0, 0);
        domino.SetFicha("Cabeza", this.gameObject);
        domino.SetFicha("Cola", this.gameObject);
        transform.rotation = Quaternion.Euler(0, 0, 90);
        GetComponent<SpriteRenderer>().sprite = spriteFicha;
        knockSound.Play();
        usada = true;
    }
}

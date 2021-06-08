///<modifed>
/// Notification text allowed to be changed at runtime
/// @Author: Lautaro
///</modifed>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class NotificationToggle : MonoBehaviour
{
    //Autores: Juan Martin Tripodi - Herrajeria
    //Ultima modificacion: 5-6-2021
    //Script encargado de activar y desactivar la notificacion de nueva reunion creada

    public GameObject canvasNotification;
    public PhotonView photonView;

    [SerializeField] public Text text;
    
    // Start is called before the first frame update
    void Start()
    {   
        
        //Comienzo con la notificacion apagada
        if (photonView.IsMine)
        {
            canvasNotification.SetActive(false);
        }
        else
        {
            canvasNotification.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //A modo de testeo, la notificacion se activara al presionar F3
        if (Input.GetKeyDown(KeyCode.F3))
        {
            turnOnNotification();
        }
    }

    //Prendo la notificacion
    public void turnOnNotification()
    {
        //Si es mia, la prendo
        if (photonView.IsMine)
        {
            canvasNotification.SetActive(true);
        }
        else
        {
            canvasNotification.SetActive(false);
        }
    }

    //Apago la notificacion (el boton X se encarga de llamar a este metodo al ser clickeado)
    public void turnOffNotification()
    {  
        //Si es mia, la apago
        if (photonView.IsMine)
        {
            canvasNotification.SetActive(false);
        }
        else
        {
            canvasNotification.SetActive(false);
        }
    }

    public void setText(string text){
        this.text.text = text;
    }
}

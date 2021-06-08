using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CymaticLabs.Unity3D.Amqp;
using System;
using Newtonsoft.Json;
using ManejoEventos;
using System.Threading;


///<sumary>
/// This class is the abstract controller from which concrete controllers such as the teamBot controller and the player Controller inherit
///@Author: Lautaro
///</sumary>
public abstract class IController : MonoBehaviour, ManejoEventos.Accion, ManejoEventos.AccionUnity
{   
    public string nickName;
    public Queue<AmqpExchangeReceivedMessage> messageQueue;

    public abstract IEnumerator handleMessageReceived(AmqpExchangeReceivedMessage msg); //abstracta
    public abstract void suscribirse();

    protected GameObject avatar;

    protected abstract void findAvatar();

    public abstract void identificarNombre();
    // Start is called before the first frame update
    public void Start()
    {   
        messageQueue = new Queue<AmqpExchangeReceivedMessage> ();
        StartCoroutine (this.consumer()); 
        suscribirse();
        identificarNombre();
        findAvatar();
    }



    ///<sumary>
    ///@Author: Benjamin
    ///</sumary>
    IEnumerator consumer ()
    {
        while (true)
        {
            yield return new WaitForSeconds (0.1f); 
            if (messageQueue.Count > 0)
            {
               // ManageEvent eventToconsume = ;
                yield return StartCoroutine(this.handleMessageReceived(messageQueue.Dequeue()));
            }
        }
    }

    public void AccionUnity(AmqpExchangeSubscription subscripcion, IAmqpReceivedMessage mensaje)
    {
        Debug.Log("--");
    }

    public void Accion(AmqpExchangeReceivedMessage msgRecibido)
    {       
        messageQueue.Enqueue (msgRecibido);
    }
}

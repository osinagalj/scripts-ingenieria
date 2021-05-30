using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CymaticLabs.Unity3D.Amqp;

using System;
using Newtonsoft.Json;

using ManejoEventos;


using System.Threading;

/*Controllador visual del agile_bot. Se encarga de subscribirse a los eventos generados por el agileBot del mundo sintetico*/
/*@autor: Lautaro*/
public class AgileBot : MonoBehaviour,ManejoEventos.Accion, ManejoEventos.AccionUnity
{   
    Actor teamMeamber = new Actor();
    string id="";
    string nombre="";
   


    public AgileBot(string nickName) {

         Debug.Log("----------------------------- AGILE_BOT start -------------");

        nombre=nickName;
        teamMeamber.setNombre(nickName);
        id=nombre+"_bot";

        ManejadorEventos.Conectar("localhost");

        ManejadorEventos.Subscribirse("example", this, this);
        ManejadorEventos.Subscribirse("CambioEstadoUserStory", this, this);
        ManejadorEventos.Subscribirse("ArtefactoCambio", this, this);
        ManejadorEventos.Subscribirse("FasePorIniciar", this, this);
        
        Debug.Log("----------------------------- AgileBot publicando evento tarea -------------");
        //Codigo para serializar un objeto
        Tarea tarea = new Tarea();
        tarea.setNombre("tarea1");
        tarea.addActor(teamMeamber);
        string jsonData = JsonConvert.SerializeObject(tarea); 
        ManejadorEventos.Publicar("CambioEstadoUserStory", jsonData);

        
        //Codigo para publicar un evento
        var exchangeName = "log_eventos";
        var routingKey = "CambioEstadoUserStory";
        var message = jsonData;
        AmqpClient.Publish(exchangeName, routingKey, message);
        

    }

    public void Accion(AmqpExchangeReceivedMessage msgRecibido)
    {   
        Debug.Log("---------------------AGILE_BOT escucho una SUSCRIPCION----------------------");

        //Thread.Sleep(5000);
        Debug.Log("----------------------------- Moviendo agil bot -------------");
        var bot = GameObject.FindGameObjectWithTag("Player"); 
        bot.GetComponent<movementAgent>().runAgent("kanban");

        //var topic= subscription;
        //var receivedJson = System.Text.Encoding.UTF8.GetString(message.Body);
        //Tarea newtarea = JsonConvert.DeserializeObject<Tarea>(receivedJson);  
        Debug.Log("-----------Mensaje de la accion = " + 
                System.Text.Encoding.UTF8.GetString(msgRecibido.Message.Body));

        switch (subscription.RoutingKey){
             case "CambioEstadoUserStory" :
                   // Debug.Log("-- CambioEstadoUserStory RoutingKey --");
                    //ResponseFinTarea(msg);
                    //ResponseFinTarea(newtarea);
                     break;
             case "FasePorIniciar" :
                    //Debug.Log("-- FasePorIniciar RoutingKey --");
                    //ResponseReunionComienzaPronto(msg);
                    break;
              default:
                 Debug.Log("-- Default RoutingKey --");
                break;
        }        

    }

    public void AccionUnity(AmqpExchangeSubscription subscripcion, IAmqpReceivedMessage mensaje)
    {
       //Interfaz implementada para el correcto funcionamieno de ManejoEventos
    }

 
    /*Handler de eventos, se activa al publicarse un evento al cual esta suscripto el AgileBot y ejecuta la accion correspondiente al mismo.
    Deserializa el mensaje recibido */
    void HandleExchangeMessageReceived(AmqpExchangeSubscription subscription, IAmqpReceivedMessage message)
    {           
    }

    /*Ejecuta la accion correspondiente al evento "Finalizo Tarea". En caso de estar asignado a dicha tarea, ejecuta el pedido de animacion para
    mover la tarea en el kanban 
      @autor: Lautaro
      @params: tarea es la tarea que finalizo*/
    void ResponseFinTarea(Tarea tarea){

        //var bot = GameObject.Find("Opcion 4(Clone)");

        
        if((tarea.contieneActor(teamMeamber.getIdentificador()))){
            //enviar evento para que el avatar cambie la tarea a done
            //Debug.Log("-- Tarea asociada mi bot --");

            //Esta funcion la tendria que implementar Hexejeria
            //Jugador.moveTarea(usuario, tarea)
        }
    }


    void ResponseCambioArtefacto(string json){
        //Debug.Log(json);
    }

    void ResponseTareaAsignada(string json){
    }

    void ResponseReunionComienzaPronto(string json){}

}


        //Suscripcion a los eventos necesarios
       // var subscriptionActor = new UnityAmqpExchangeSubscription("topic_logs", AmqpExchangeTypes.Topic, "CambioEstadoUserStory",null, HandleExchangeMessageReceived);
       //     AmqpClient.Subscribe(subscriptionActor);

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CymaticLabs.Unity3D.Amqp;

using System;
using Newtonsoft.Json;

public class AgileBot
{   // Clase AgileBot Team Mignon

    Actor teamMeamber = new Actor();
    string id="";
    string nombre="";
   

/*estructura del AgileBot, guardado de datos del Team Member y suscripcion a eventos
 @autor: Sofia
 @params: string nickName es el nombre del Actor asociado al AgileBot*/
    public AgileBot(string nickName){

        nombre=nickName;
        teamMeamber.setNombre(nickName);
        id=nombre+"_bot";

        //Debug.Log("----------------------------- AGILE_BOT -------------");
        //Suscripcion a los eventos necesarios
        var subscriptionActor = new UnityAmqpExchangeSubscription("topic_logs", AmqpExchangeTypes.Topic, "CambioEstadoUserStory",null, HandleExchangeMessageReceived);
            AmqpClient.Subscribe(subscriptionActor);

        var subscriptionArtefacto = new UnityAmqpExchangeSubscription("topic_logs", AmqpExchangeTypes.Topic, "Artefacto.Cambio",null, HandleExchangeMessageReceived);
            AmqpClient.Subscribe(subscriptionArtefacto);

        var subscriptionReunion = new UnityAmqpExchangeSubscription("topic_logs", AmqpExchangeTypes.Topic, "FasePorIniciar",null, HandleExchangeMessageReceived);
            AmqpClient.Subscribe(subscriptionReunion);

        //Codigo para serializar un objeto
        Tarea tarea = new Tarea();
        tarea.setNombre("tarea1");
        tarea.addActor(teamMeamber);
        string jsonData = JsonConvert.SerializeObject(tarea); 
        
        //Codigo para publicar un evento
        var exchangeName = "topic_logs";
        var routingKey = "CambioEstadoUserStory";
        var message = jsonData;
        AmqpClient.Publish(exchangeName, routingKey, message);

    }


    /*Handler de eventos, se activa al publicarse un evento al cual esta suscripto el AgileBot y ejecuta la accion correspondiente al mismo.
    Deserializa el mensaje recibido 
     @autor: Lautaro*/
    void HandleExchangeMessageReceived(AmqpExchangeSubscription subscription, IAmqpReceivedMessage message)
    {   
        
        //Debug.Log("---------------------AGILE_BOT escucho una SUSCRIPCION----------------------");
        var topic= subscription;
        var receivedJson = System.Text.Encoding.UTF8.GetString(message.Body);
        Tarea newtarea = JsonConvert.DeserializeObject<Tarea>(receivedJson);  
        //Debug.Log("-----------Mensaje de message = " + receivedJson);

        switch (subscription.RoutingKey){
             case "CambioEstadoUserStory" :
                   // Debug.Log("-- CambioEstadoUserStory RoutingKey --");
                    //ResponseFinTarea(msg);
                    ResponseFinTarea(newtarea);
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

    /*Ejecuta la accion correspondiente al evento "Finalizo Tarea". En caso de estar asignado a dicha tarea, ejecuta el pedido de animacion para
    mover la tarea en el kanban 
      @autor: Lautaro
      @params: tarea es la tarea que finalizo*/
    void ResponseFinTarea(Tarea tarea){
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

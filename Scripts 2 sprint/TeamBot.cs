using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CymaticLabs.Unity3D.Amqp;
using System;
using Newtonsoft.Json;
using ManejoEventos;
using System.Threading;


   ///<sumary>
    ///This class is a component that controls every action in the teambot. 
    ///when a event is captured, there is a filter for identify queue of the event, and another filter for the action to make. 
    ///@Author: Lautaro
    ///</sumary>

public class TeamBot : MonoBehaviour
{

    public string nickName = "undefined";
    private string gender = "M";
    private GameObject avatar = null;
    void Start()
    {   
        this.nickName = PlayerPrefs.GetString("nickname");
        findAvatar();
        getGender();
        //this.gender = PlayerPrefs.GetString("gender");

        //Event subscription
        Listener.setTeamBot (this); 

    }

    ///<sumary>
    // Search for the avatar associated by the nickname
    ///</sumary>
    private void findAvatar(){
        foreach(var pv in GameObject.FindGameObjectsWithTag("teamBot")){
            string avatar_name = pv.GetComponent<PlayerNameTag>().getName();
            if(avatar_name == this.nickName){
                this.avatar = pv;
                return;
            }
        }
    }

    ///<sumary>
    // Set the gender of the teamBot depending on the avatar selecting by the teamMember
    ///</sumary>
    private void getGender(){
        string avatar = PlayerPrefs.GetString("avatar");
        switch (avatar)
        {
            case "Opcion 4":
                this.gender = "F";
                break;
            case "Opcion 5":
                this.gender = "F";
                break;
            case "Opcion 6":
                this.gender = "F";
                break;
            default:    
                this.gender = "M";
                break;
            
        }
    }
    ///<sumary>
    /// this method handle the teamBot actions. 
    ///    Parameter ManageEvent: event to handle. 
    /// For execute actions to make movements, you has to call coroutines. 
    /// For execute actions to pass messages, you can call any function. 
    ///</sumary>
    public IEnumerator handleMessageReceived (ManageEvent ManageEvent)
    {
        string jsonBody = System.Text.Encoding.UTF8.GetString(ManageEvent.exchangeMessage.Message.Body);
        string routingKey = ManageEvent.exchangeMessage.Subscription.RoutingKey;
        Event _event = Event.deserializeJson(jsonBody);  
        
        Debug.Log (_event.getTo()); 
        Debug.Log(routingKey); 
        if (this.nickName == _event.getTo())
        {
            switch (routingKey)
            {
                case "CambioEstadoUserStory":
                    yield return StartCoroutine (moveTask(_event));
                    break;
                case "Speak":
                    yield return StartCoroutine (speak(_event));
                    break;
                case "FasePorIniciar":
                    this.FasePorIniciar();
                    break; 
                case "message":
                    switch (_event.getAction())
                    {
                        case "go_to_meeting":
                            yield return StartCoroutine (this.goToMeeting(_event)); // En este caso se toma la sala del jefe como sala de reuniones. 
                            break;
                        case "ended_meeting":
                            yield return StartCoroutine (this.ended_meeting(_event)); // en este caso cuando termina la reunion se va a la sala de desarrollo. 
                            break;
                        default:
                            yield return StartCoroutine (this.speak(_event)); // en este caso cuando termina la reunion se va a la sala de desarrollo. 
                            break;        
                    }
                    break; 
            }
        }
        yield return null ; 
    }

                
    
    public string GetNickName ()
    {
        return nickName; 
    }

    ///<sumary>
    /// this method implements the action of movement of teambot. 
    ///</sumary>
    public IEnumerator moveTeamBot(string location){
        var bot = avatar.GetComponent<TeamBotMovement>();
        bot.runAgent(location);
        while (bot.isMoving)
        {
            yield return null; 
        }   
        
    }

    ///<sumary>
    ///This method is responsible for reproducing the event text using the voice component.
    ///This runs a different speaker depending on the gender of the teamMember
    ///</sumary>
    public IEnumerator speak(Event _event){
        string text = _event.message;
         Debug.Log ("---------------------------------------SPEAK-----------------------------"); 
         Debug.Log (text);
        //Get the component of speek
        var bot_speaker = avatar.GetComponent<TTSUnityWin>();
        if(this.gender == "F"){
            bot_speaker.FemaleVoiceSpeech(text);
        }else {
            bot_speaker.MaleVoiceSpeech(text);
        }

        //@Here verify that you have finished speaking
        yield return null; 
    }

    public void FasePorIniciar()
    {
        Debug.Log ("evento en formato texto"); 
    }

    public IEnumerator goToMeeting(Event ev){
        //obtener la ubicacion de la reunion 
        string location = "jefe";
        yield return this.moveTeamBot(location);
        //yield return null; 
    }

    public IEnumerator ended_meeting(Event ev){
        //obtener la ubicacion 
        string location = "desarrollo";
        yield return this.moveTeamBot (location);
        //yield return null; 
    }
    

    public IEnumerator moveTask(Event ev){
        //obtener la task
        string location = "kanban";
        string teamBot = "Lautaro";
        yield return this.moveTeamBot (location);
        //yield return null; 
    }
    

    ///<sumary>
    /// this method implements the action of movement of player. 
    /// @author: Benjamin 
    ///</sumary>
    public IEnumerator moverse (string lugar)
    {
        var bot = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<movementAgent>();
        bot.runAgent(lugar);
        while (bot.isMoving)
        {
            yield return null; 
        }
    }
}

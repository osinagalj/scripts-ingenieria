using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CymaticLabs.Unity3D.Amqp;
using System;
using Newtonsoft.Json;
using ManejoEventos;
using System.Threading;


///<sumary>
///@Author: Lautaro
///</sumary>
public class TeamBot :  IController
{   
    private string gender = "M";

    protected override void findAvatar(){
        foreach(var pv in GameObject.FindGameObjectsWithTag("teamBot")){
                string avatar_name = pv.GetComponent<PlayerNameTag>().getName();
                if(avatar_name == this.nickName){
                    base.avatar = pv;
                    return;
                }
        }
    }
    // Start is called before the first frame update
    void Start()
    {       
        base.Start();
    }

    public override void suscribirse(){
        ManejadorEventos.Subscribirse("speak", this, this);
        ManejadorEventos.Subscribirse("movement", this, this);
        ManejadorEventos.Subscribirse("task", this, this);
        ManejadorEventos.Subscribirse("message", this, this);

    }

    public override void identificarNombre(){
        //this.nickName = PlayerPrefs.GetString("nickname");
    }
    
    ///<sumary>
    /// this method handle the teamBot actions. 
    ///    Parameter ManageEvent: event to handle. 
    /// For execute actions to make movements, you has to call coroutines. 
    /// For execute actions to pass messages, you can call any function.
    ///@Author: Lautaro 
    ///</sumary>
    public override IEnumerator handleMessageReceived (AmqpExchangeReceivedMessage msg)
    {
        Dictionary <string, string> _event = JsonConvert.DeserializeObject<Dictionary <string, string>>(System.Text.Encoding.UTF8.GetString(msg.Message.Body));
        
        if (this.nickName == _event["to"])
        {
            switch (msg.Subscription.RoutingKey.ToLower())
            {
                case "task":
                    yield return StartCoroutine (this.moveTeamBot("kanban"));
                    switch (_event["action"].ToLower())
                    {
                        case "create":
                            string name_task = _event ["name_task"];
                            string description_task = _event ["description_task"];
                            string priority_task = _event["priority_task"]; // hacer checkeo de existencia. 
                            // crear tarea con todos los valores del diccionario tareaInfo, herrajeria. 
                            break;
                        case "change_state":
                            string new_state = _event["new_state"];
                            string id_artefacto = _event["id_artefacto"];
                            // herrajeria
                            break;
                        case "change_color":
                            string new_color = _event["new_color"]; 
                            string id_artefacto2 = _event["id_artefacto"]; 
                            // herrajeria
                            break;
                        case "delete_task":
                            string name_task_to_delete = _event ["task_to_delete"]; 
                            // herrajeria
                            break; 
                        default:
                            yield return null;
                            break;  
                    }
                    break;
                case "speak":
                    yield return StartCoroutine (speak(_event ["message"]));
                    break;

                case "message":
                    switch (_event["action"].ToLower())
                    {
                        case "go_to_meeting":
                            yield return StartCoroutine (this.moveTeamBot("reunion")); // En este caso se toma la sala del jefe como sala de reuniones. 
                            break;
                        case "ended_meeting":
                            yield return StartCoroutine (this.moveTeamBot("desarrollo")); // en este caso cuando termina la reunion se va a la sala de desarrollo. 
                            break;
                        default:
                            yield return StartCoroutine (this.speak(_event ["message"])); 
                            break;        
                    }
                    break; 
                default:
                    yield return null; 
                    break; 
            }
        }
        yield return null ; 
        
    }


    
    public void setName2(string name){
        this.nickName = name;
    }

        ///<sumary>
    // Set the gender of the teamBot depending on the avatar selecting by the teamMember
    ///</sumary>
    private void getGender(){
        string avatar = "Opcion 4";
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
    /// this method implements the action of movement of teambot.
    ///@Author: Lautaro 
    ///</sumary>
    public IEnumerator moveTeamBot(string location){
        var bot = base.avatar.GetComponent<TeamBotMovement>();
        bot.runAgent(location);
        while (bot.isMoving)
        {
            yield return null; 
        }   
        
    }


    ///<sumary>
    ///This method is responsible for reproducing the event text using the voice component.
    ///This runs a different speaker depending on the gender of the teamMember
    ///@Author: Lautaro
    ///</sumary>
    public IEnumerator speak(string messageToSay){
        //Get the component of speek
        Debug.Log("--------------SPEAK ----------------");
        Debug.Log(messageToSay);
        var bot_speaker = avatar.GetComponent<TTSUnityWin>();
        string gender = "F";
        if(gender == "F"){
            //bot_speaker.FemaleVoiceSpeech(messageToSay);
        }else {
            //bot_speaker.MaleVoiceSpeech(messageToSay);
        }

        //@Here verify that you have finished speaking
        yield return null; 
    }


  
}




/*
    ///<sumary>
    /// this method handle the teamMember actions. 
    /// Parameter ManageEvent: event to handle. 
    ///@Author: Lautaro 
    ///</sumary>
    public override IEnumerator handleMessageReceived (AmqpExchangeReceivedMessage msg)
    {
        Dictionary <string, string> _event = JsonConvert.DeserializeObject<Dictionary <string, string>>(System.Text.Encoding.UTF8.GetString(msg.Message.Body));
        
        if (this.nickName == _event["to"])
        {   
            switch (msg.Subscription.RoutingKey.ToLower())
            {   
                case "speak":
                    yield return StartCoroutine (this.speak(_event ["message"]));
                    break;
                case "movement":
                    yield return StartCoroutine (this.moveTeamBot(_event["location"].ToLower())); 
                    break; 

                case "task":
                    yield return StartCoroutine (this.moveTeamBot("kanban"));
                    switch (_event["action"].ToLower())
                    {
                        case "create":
                            string name_task = _event ["name_task"];
                            string description_task = _event ["description_task"];
                            string priority_task = _event["priority_task"]; // hacer checkeo de existencia. 
                            // crear tarea con todos los valores del diccionario tareaInfo, herrajeria. 
                            break;
                        case "change_state":
                            string new_state = _event["new_state"];
                            string id_artefacto = _event["id_artefacto"];
                            // herrajeria
                            break;
                        case "change_color":
                            string new_color = _event["new_color"]; 
                            string id_artefacto2 = _event["id_artefacto"]; 
                            // herrajeria
                            break;
                        case "delete_task":
                            string name_task_to_delete = _event ["task_to_delete"]; 
                            // herrajeria
                            break; 
                        default:
                            yield return null;
                            break;  
                    }
                    break;

                default:
                    yield return null; 
                    break; 
            }
        }
        yield return null ; 
        
    }



*/
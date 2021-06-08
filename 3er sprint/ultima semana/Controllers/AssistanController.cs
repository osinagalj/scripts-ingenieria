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
public class AssistanController :  IController
{   
    protected override void findAvatar(){        
        base.avatar = GameObject.FindGameObjectWithTag("assistant");
    }
    // Start is called before the first frame update
    void Start()
    {       
        base.Start();
    }

    public override void suscribirse(){
        ManejadorEventos.Subscribirse("speak", this, this);
        ManejadorEventos.Subscribirse("movement", this, this);
    }

    public override void identificarNombre(){
        this.nickName = "Cristina"; //todo
    }
    

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
                    yield return StartCoroutine (this.moveTeamBot(_event["location"].ToLower())); // En este caso se toma la sala del jefe como sala de reuniones. 
                    break; 
                default:
                    yield return null; 
                    break; 
            }
        }
        yield return null ;       
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
        Debug.Log("--------------SPEAK ----------------");
        Debug.Log(messageToSay);
        //Get the component of speek
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



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
/// Controller used by teamMembers, with functionalities to show notifications and move only if the user wishes.
///</sumary>
public class TeamMemberController :  IController
{   


    protected override void findAvatar(){
        base.avatar = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {       
        base.Start();
    }

    public override void suscribirse(){
        ManejadorEventos.Subscribirse("movement", this, this);
        ManejadorEventos.Subscribirse("movement_now", this, this);
        ManejadorEventos.Subscribirse("notification", this, this);
    }

    public override void identificarNombre(){
        this.nickName = PlayerPrefs.GetString("nickname");
    }
    

    ///<sumary>
    /// this method handle the teamMember actions. 
    /// Parameter ManageEvent: event to handle. 
    ///@Author: Lautaro 
    ///</sumary>
    public override IEnumerator handleMessageReceived (AmqpExchangeReceivedMessage msg)
    {   
        Debug.Log("Handle del TeamMember");
        Dictionary <string, string> _event = JsonConvert.DeserializeObject<Dictionary <string, string>>(System.Text.Encoding.UTF8.GetString(msg.Message.Body));
        
        if (this.nickName == _event["to"])
        {
            switch (msg.Subscription.RoutingKey.ToLower())
            {
 
                case "movement":
                    yield return StartCoroutine (this.goToMeeting());  
                    break;
                case "movement_now":
                    yield return StartCoroutine (this.moveTeamMember(_event["location"])); 
                    break;
                case "notification":
                    yield return StartCoroutine (this.notification(_event["message"])); 
                    break;
                
                    
                default:
                    yield return null; 
                    break;        
            }
        }        
    }

    ///<sumary>
    ///@Author: Lautaro 
    ///</sumary>
    public IEnumerator notification(string message){
        Debug.Log(" -------- notificacion ---------");
        var noti = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<NotificationToggle>();
        noti.setText(message);
        noti.turnOnNotification();
        yield return null; 
    }


    ///<sumary>
    /// this method implements the action of movement of teamMember.
    ///@Author: Lautaro 
    ///</sumary>
    public IEnumerator moveTeamMember(string location){
        var mov_ag = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<movementAgent>();
        mov_ag.runAgent(location);
        while (mov_ag.isMoving)
        {
            yield return null; 
        }
        
    }

    ///<sumary>
    /// this method implements the action of movement of teamMember.
    ///@Author: Lautaro 
    ///</sumary>
    public IEnumerator goToMeeting(){
        Debug.Log("Entro en go to meeting");
        var btn = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<P_AskButton>();
        btn.activateAsk();
        //moveTeamMember("kanban");
        yield return null;
    }



  
}


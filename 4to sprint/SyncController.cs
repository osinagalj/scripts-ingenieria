using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CymaticLabs.Unity3D.Amqp;
using System;
using Newtonsoft.Json;
using ManejoEventos;
using System.Threading;
using UnityEngine;
using Photon.Pun;
using System.Globalization;
using System;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;

///<sumary>
///  This class is in charge of receiving the events for the elimination and creation of the teamBots avatars through the event
///  @Author: Lautaro
///</sumary>
public class SyncController : MonoBehaviour, ManejoEventos.Accion, ManejoEventos.AccionUnity
{   
    public PhotonView photonView;
        public Queue<AmqpExchangeReceivedMessage> messageQueue;

    ///<sumary>
    ///  It subscribes to 2 event queues, one for creation and one for destruction.
    ///  @Author: Lautaro
    ///</sumary>
    public void Start()
    {   
        messageQueue = new Queue<AmqpExchangeReceivedMessage> ();
        ManejadorEventos.Subscribirse("delete_teamBot", this, this);
        ManejadorEventos.Subscribirse("create_teamBot", this, this);
        StartCoroutine (this.consumer()); 
    }

    ///<sumary>
    ///  This method does the magic, it asks if I am the master, then I can create or delete avatars. The important thing is that each client must have this component, 
    //   so they all receive these types of events and only the current master is in charge of eliminating / creating the avatars.
    ///  @Author: Lautaro
    ///</sumary>
    public IEnumerator handleMessageReceived (AmqpExchangeReceivedMessage msg)
    {
        Dictionary <string, string> _event = JsonConvert.DeserializeObject<Dictionary <string, string>>(System.Text.Encoding.UTF8.GetString(msg.Message.Body));
        
        if (photonView.IsMine)
        {
            switch (msg.Subscription.RoutingKey.ToLower())
            {
                case "delete_teambot":
                    if (PhotonNetwork.IsMasterClient)
                        yield return StartCoroutine (this.deleteTeamBot(_event["nickName"]));
                    break;
                case "create_teambot":
                    if (PhotonNetwork.IsMasterClient)
                        yield return StartCoroutine (this.createTeamBot(_event));
                    break;

                default:
                    yield return null; 
                    break; 
            }
        }

    
    }

    ///<sumary>
    ///  Post a teamBot creation event
    ///  @Author: Lautaro
    ///</sumary>
    public static void pulishCreate(string nickName, string avatar, float x, float y, float z){
        ManejadorEventos.Publicar ("create_teamBot",
                                 "{\"nickName\":\"" + nickName +
                                  "\", \"avatar\":\""+ avatar +
                                  "\", \"x\":\""+ x +
                                  "\", \"y\":\""+ y +
                                  "\", \"z\":\""+ z +"\"}"
                                  );
    }

    ///<sumary>
    ///  Post a teamBot delete event
    ///  @Author: Lautaro
    ///</sumary>
    public static void publishDelete(string nickName){
        ManejadorEventos.Publicar ("delete_teamBot",
                                 "{\"nickName\":\"" + nickName + "\"}"
                                 );
    }

    ///<sumary>
    ///  create one teamBot
    ///  @Author: Lautaro
    ///</sumary>
    public IEnumerator createTeamBot(Dictionary <string, string> _event){
        //CreateAgileBot c = new CreateAgileBot();
        //c.crearBot();
        Debug.Log("CREATE TEAM BOT - - - - - - - -- - - - - - - - -");
        string nickName = _event["nickName"];
        string avatar = _event["avatar"];

        Debug.Log("vector pos x = " + _event["x"]);
        Debug.Log("vector pos y = " + _event["y"]);
        Debug.Log("vector pos z = " + _event["z"]);

        float x = float.Parse(_event["x"]);
        float y = float.Parse(_event["y"]);
        float z = float.Parse(_event["z"]);

        //Vector3 vectorPosicion = new Vector3(x,y,z);
        Vector3 vectorPosicion = new Vector3(11.0f,0,0);

        Debug.Log("vector = " + vectorPosicion);

        GameObject g = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs/Avatars", avatar), vectorPosicion, Quaternion.identity);
            
        PhotonView p = g.GetComponent<PhotonView>();

        g.GetComponent<TeamBot>().setName2(nickName);
        g.GetComponent<TeamBot>().setName2(nickName);
        g.GetComponent<PlayerNameTag>().SetName(nickName);

        TeamBotId a = new TeamBotId(nickName, p.ViewID);
        DBAgileBot.Instance.Save(a);

        string json = "{\"from\": \"" + nickName + "\", \"date\": \"" + DateTime.Now.ToString() + "\", \"action\": \"" + "disconnect" + "\", \"id_teambot_created\": \"" + p.ViewID + "\"}";
        ManejadorEventos.Publicar("team_member", json);
               
        yield return null; 
         
        
    }


    ///<sumary>
    ///  delete one teamBot
    ///  @Author: Lautaro
    ///</sumary>
    public IEnumerator deleteTeamBot(string nickname){
           
        Debug.Log("Entra a destruir BOT -  - - - -- - - - - - - - ");
        List<TeamBotId> agiles = DBAgileBot.Instance.GetAll();
        bool reconnect = false; //indica si el usuario se esta reconectando al room

            foreach(var pv in GameObject.FindGameObjectsWithTag("teamBot")){
                PhotonView p = pv.GetComponent<PhotonView>();
                //Debug.Log(p.ViewID); 
                //Debug.Log("--- DESTRUYE A: " + nickname);
                foreach(var ag in agiles){
                    //Debug.Log("-----bot ----");
                    //Debug.Log(ag.getNickName()); 
                    //Debug.Log(ag.getPhotonViewID()); 
                    if (nickname == ag.getNickName() && p.ViewID == ag.getPhotonViewID()){
                        //Debug.Log("---DESTRUYE UN BOT ----");
                        //Debug.Log(p.ViewID); 
                        PhotonNetwork.Destroy(pv);
                        DBAgileBot.Instance.Delete(ag);
                        reconnect = true;
                        string json = "{\"from\": \"" + nickname + "\", \"date\": \"" + DateTime.Now.ToString() + "\", \"action\": \"" + "reconnect" + "\", \"id_teambot_removed\": \"" + p.ViewID + "\"}";
                        ManejadorEventos.Publicar("team_member", json);
                        //Debug.Log(json);
                    }

                }
                
            }

            if (!reconnect) {//no elimino ningun bot entonces el usuario entra por primera vez al room
                string json = "{\"from\": \"" + nickname + "\", \"date\": \"" + DateTime.Now.ToString() + "\", \"action\": \"" + "connect" + "\"}";
                ManejadorEventos.Publicar("team_member", json);
                //Debug.Log(json);
            }
        yield return null; 
          
    }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

///</sumary> 
/// Esta clase se encarga de crear un TeamBot identico al player.
///</sumary>     
/// @Author: Lautaro
public class CreateAgileBot 
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ///</sumary> 
    /// Este metodo se encarga de instanciar un npc identico al teamMember 
    ///</sumary>     
    /// @Author: Lautaro
    [PunRPC]
    public bool crearBot()
    {   
        if(!PhotonNetwork.IsMasterClient){
           PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
           return false;
        }else{

            Debug.Log("Crendo el TeamBot");


            //Obteniendo Avatar
            string avatar = PlayerPrefs.GetString("avatar");
            avatar = avatar + " - bot";
            
            //Posicion
            var bot = GameObject.FindGameObjectWithTag("Player"); 
            Transform t = bot.GetComponent<Transform>();
                
            //Vector3 vectorPosicion = new Vector3(11.66974f,-0.2294269f,-0.5654888f);
            Vector3 vectorPosicion = new Vector3(t.position.x,t.position.y,t.position.z);

            //Instanciado bot con photon
            GameObject g = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs/Avatars", avatar), vectorPosicion, Quaternion.identity);
            
            PhotonView p = g.GetComponent<PhotonView>();
            
            //setear id
            //g.GetComponent<AgileBotId>().nickname = PlayerPrefs.GetString("nickname");

            g.GetComponent<PlayerNameTag>().SetName(PlayerPrefs.GetString("nickname"));
            Debug.Log("---- termino de crear el TeamBot -----");
            Debug.Log(GameObject.FindObjectsOfType<PhotonView>().Length);

            TeamBotId a = new TeamBotId(PlayerPrefs.GetString("nickname"),p.ViewID);
            DBAgileBot.Instance.Save(a);

            return true;
        }    
    }

    [PunRPC]
    public void destroyTeamBot(string nickname)
    {   
        askMaster();
        //Debug.Log("--- Eliminando TeamBot ----");
        destroyBot(); 
    }    

    public static async void askMaster(){
        var result = await MasterAsync();
    }

    public static async Task<bool> MasterAsync()  
    {   
        //Debug.Log("--------------- Solicitando Master");
        bool result = false;
        await Task.Run(() =>  
        {  
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            //Thread.Sleep(5000);
            result = true;
        });
        return result;
        
    } 
    
    
    public static async void destroyBot()
    {    
        
        await Task.Run(() =>  
        {   
            string nickname = PlayerPrefs.GetString("nickname");
            if(PhotonNetwork.IsMasterClient){
                    foreach(var pv in GameObject.FindGameObjectsWithTag("teamBot")){
                    Debug.Log(pv.GetComponent<PlayerNameTag>().getName()); 
                    if (pv.GetComponent<PlayerNameTag>().getName() == "4"){
                            PhotonNetwork.Destroy(pv);
                        }
                    }
            }
        });                
   }
}

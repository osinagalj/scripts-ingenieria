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
/// Esta clase es un componente de cada player que se encarga de eliminar su 
/// teamBot asociado una vez que el TeamMember se reconecta
///</sumary>     
/// @Author: Lautaro

public class DeleteBot : MonoBehaviour
{   
    private bool finish = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake(){

    }

    // Update is called once per frame
    void Update()
    {
        if(finish == false){
            destroyTeamBot();
        }
            
    }

    ///</sumary> 
    /// EL teamMember solicita ser el MasterCliente para poder eliminar a su bot asociado,
    /// ya que solo el MasterCLient es capaz de destruir objetos InstantiateRoomObject.
    /// Luego se buscan las relaciones teamMember-teamBot en la DB y se eliminan los teamBots asociados al TeamMember
    ///</sumary>     
    /// @Author: Lautaro
    
    public void destroyTeamBot() 
    {
        
        if(!PhotonNetwork.IsMasterClient){
            Debug.Log("--------------- NO SOY EL MASTER ----------");
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }
        else{
            Debug.Log("-- SOY EL MASTER -");
            
            finish = true;
            string nickname = PlayerPrefs.GetString("nickname");

            //var filter = Builders<BsonDocument>.Filter.Eq(PlayerPrefs.GetString("nickname"),p.ViewID );
    
            List<TeamBotId> agiles = DBAgileBot.Instance.GetAll();
            foreach(var ag in agiles){
                Debug.Log(ag.getNickName()); 
                Debug.Log(ag.getPhotonViewID()); 
            }

            //Debug.Log("------ Eliminando los bots----");

            foreach(var pv in GameObject.FindGameObjectsWithTag("teamBot")){
                PhotonView p = pv.GetComponent<PhotonView>();
                Debug.Log(p.ViewID); 
                //Debug.Log("--- DESTRUYE A: " + nickname);
                foreach(var ag in agiles){
                    //Debug.Log("-----bot ----");
                    Debug.Log(ag.getNickName()); 
                    Debug.Log(ag.getPhotonViewID()); 
                    if (nickname == ag.getNickName() && p.ViewID == ag.getPhotonViewID()){
                        //Debug.Log("---DESTRUYE UN BOT ----");
                        Debug.Log(p.ViewID); 
                        PhotonNetwork.Destroy(pv);
                        DBAgileBot.Instance.Delete(ag);
                        
                    }
                    
                }
                
            }
            return;
            //yield return new WaitForSeconds(.1f);
             //Debug.Log(pv.GetComponent<PlayerNameTag>().getName()); 
        }        
    }



}

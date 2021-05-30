using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///</sumary>   
///Esta entidad modela la relacion entre un teamMember identificado por su nickname y 
///la identificacion del teamBot utilizando el ID generado por PhotonView
///</sumary>     
/// @Author: Lautaro

public class TeamBotId : MongoDBEntity
{   
    public string nickName = "unknow";
    public int photonViewID = 0;
        
    public TeamBotId(string nickname, int pvID){

        _id = DBAgileBot.Instance.Count();
        this.nickName = nickname;
        this.photonViewID = pvID;
    }

    public string getNickName(){
        return this.nickName;
    }

    public int getPhotonViewID(){
        return this.photonViewID;
    }
    
    public void setNickName(string nickname){
        this.nickName = nickName;
    }

    public void setPhotonViewID(int photonViewID){
        this.photonViewID = photonViewID;
    }

}

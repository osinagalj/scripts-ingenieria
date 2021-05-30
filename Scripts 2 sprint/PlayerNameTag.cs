using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

    ///<sumary>
    ///This class is in charge of setting the name of the TeamBot avatar, 
    //identifying them correctly through the DB
    ///</sumary>
    ///@Author: Lautaro
public class PlayerNameTag : MonoBehaviourPun
{
    public Transform nameUserMapTransform;
    [SerializeField] private TextMeshProUGUI nametag;
    [SerializeField] private TextMeshProUGUI nameUserMap;
    // Start is called before the first frame update
    void Start()
    {
        SetName();
    }

    // Update is called once per frame
    void Update()
    {
        nameUserMapTransform.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
    }

    private void SetName()
    {
        //Debug.Log("--------- SETEANDO NOMBRE --------------");
        if (photonView.Owner != null)
        {
            nametag.text = photonView.Owner.NickName;
            nameUserMap.text = photonView.Owner.NickName;
        }
        else
        {
            //Debug.Log("-------------- ES UN BOT ---------------");
            //nametag.text = photonView.ViewID.ToString();
            string nickname = PlayerPrefs.GetString("nickname");
            List<TeamBotId> agiles = DBAgileBot.Instance.GetAll();
            foreach(var ag in agiles)
            {
                //Debug.Log("--------------------seteando nombre bot de la db -----------");
                //Debug.Log(ag.getNickName()); 
                //Debug.Log(ag.getPhotonViewID()); 

                if(ag.getPhotonViewID() == photonView.ViewID)
                {
                    nametag.text = ag.getNickName();
                    nameUserMap.text = ag.getNickName();
                }
            }
        }
    }

    public void SetName(string nickname)
    {
        //nametag.text = photonView.Owner.NickName;
        nametag.text = nickname;
    }

    public string getName(){
        return nametag.text; 
    }

}

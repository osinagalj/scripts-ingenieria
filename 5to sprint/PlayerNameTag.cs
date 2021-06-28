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
    public IDMapper idm;
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
        if (photonView.Owner != null)
        {
            nametag.text = photonView.Owner.NickName;
            nameUserMap.text = photonView.Owner.NickName;
        }
        else
        {
            string nickname = PlayerPrefs.GetString("nickname");
            List<TeamBotId> agiles = DBAgileBot.Instance.GetAll();
            foreach(var ag in agiles)
            {
                if(ag.getPhotonViewID() == photonView.ViewID)
                {
                    nametag.text = ag.getNickName();
                    nameUserMap.text = ag.getNickName();
                    //obtener el teamBot y setearle el nickname
                    foreach(var pv in GameObject.FindGameObjectsWithTag("teamBot")){
                        PhotonView p = pv.GetComponent<PhotonView>();
                        if(p.ViewID == ag.getPhotonViewID()){
                            pv.GetComponent<TeamBot>().setName2(ag.getNickName());
                        }

                    }
                }
            }

        }
    }


    
    public void SetNameP(string nickname)
    {
        //nametag.text = photonView.Owner.NickName;
        nametag.text = nickname;
        
    }
    ///<summary>
    ///The method sets the Synthetic World's name of the AgileBot to vinculate it with the Photon ID
    ///</summary>
    //Author: Ezequiel Carbajo
    ///<return>
    ///void
    ///</return>
    ///<param name="nickname">
    ///The Synthetic World's name of the AgileBot
    ///</param>
    public void SetName(string nickname)
    {
        //nametag.text = photonView.Owner.NickName;
        nametag.text = nickname;
        idm.setSyntheticID(nickname);
    }

    public string getName(){
        return nametag.text; 
    }

}

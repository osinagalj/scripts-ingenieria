using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameTag_Assistant : MonoBehaviour
{   
    public Transform nameUserMapTransform;
    [SerializeField] private TextMeshProUGUI nametag;
    [SerializeField] private TextMeshProUGUI nameUserMap;
    // Start is called before the first frame update
    void Start()
    {   
        //Text timeText = "Marcelita";
        //nametag = new TextMeshProUGUI();
        //nametag.text = "Marcelita";
    }
    void Update()
    {
       
    }

    public string getName(){
        return nametag.text; 
    }

}

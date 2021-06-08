using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class P_AskButton : MonoBehaviour
{   
    [SerializeField]
    public GameObject askMovement;
    // Start is called before the first frame update
    void Start()
    {   
        
        askMovement.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateAsk()
    {
        //Debug.log("Entro en active");
        askMovement.SetActive(true);
       // askMovement.enabled=true;
    }


    public void desactivateAsk()
    {
        Debug.Log("Entro en desactive");
        askMovement.SetActive(false);
       // askMovement.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAskMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void answerYes(){

        var btn = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<P_AskButton>();
        var mv_agent = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<movementAgent>();
        btn.desactivateAsk();
        mv_agent.runAgent("reunion");
    
        Debug.Log("---- Moviendo YESSSSS  -----");
    
        //btn.activateAsk();
    }

    public void answerNo(){
        var btn = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<P_AskButton>();
        btn.desactivateAsk();
    }
}

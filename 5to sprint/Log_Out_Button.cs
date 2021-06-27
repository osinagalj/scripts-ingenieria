using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;
using System;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using ManejoEventos;
using Photon.Realtime;
using Debug = UnityEngine.Debug;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Log_Out_Button : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject bot;
    public Button button;
    public static bool log_out = false; 
    //SCRIPT QUE SE ENCARGA DE PRODUCIR LOS DISTINTOS LOG OUT POSIBLES.

    void Awake()
    {
        button.onClick.AddListener(() => StartCoroutine(closeApplication()));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator closeApplication()
    {
        string avatar = PlayerPrefs.GetString("avatar");
        avatar = avatar + " - bot";
        var bot = GameObject.FindGameObjectWithTag("Player"); 
        Transform t = bot.GetComponent<Transform>();
        string nickName = PlayerPrefs.GetString("nickname");

        if (PhotonNetwork.IsMasterClient)
        {
            yield return StartCoroutine (this.createTeamBot(avatar,nickName,Convert.ToString( t.position.x),Convert.ToString( t.position.y),Convert.ToString( t.position.z)));
        }
        else
        {
            yield return StartCoroutine (ejecutarEnOrden(avatar,nickName,Convert.ToString( t.position.x),Convert.ToString( t.position.y),Convert.ToString( t.position.z)));
        }
        Application.Quit ();
    }

    IEnumerator publicarEventos (string avatar, string nickName, string x, string y, string z)
    {
         ManejadorEventos.Publicar ("create_teamBot",
                                 "{\"nickName\":\"" + nickName +
                                  "\", \"avatar\":\""+ avatar +
                                  "\", \"x\":\""+ x  +
                                  "\", \"y\":\""+ y +
                                  "\", \"z\":\""+ z +"\"}"
                                  );
         yield return new WaitForSeconds (0.5f); 
    }

    IEnumerator ejecutarEnOrden (string avatar, string nickName, string x, string y, string z)
    {

        yield return StartCoroutine(publicarEventos(avatar,nickName,x,y,z));

        while (!log_out)
        {
            yield return null; 
        }
        
    }

        ///<sumary>
    ///  create one teamBot
    ///  @Author: Lautaro
    ///</sumary>
    public IEnumerator createTeamBot(string avatar, string nickName, string x, string y, string z){
        
        Vector3 vectorPosicion = new Vector3(11.0f,0,0);

        GameObject g = PhotonNetwork.InstantiateRoomObject(Path.Combine("PhotonPrefabs/Avatars", avatar), vectorPosicion, Quaternion.identity);
            
        PhotonView p = g.GetComponent<PhotonView>();
        g.GetComponent<TeamBot>().setName2(nickName);
        g.GetComponent<PlayerNameTag>().SetNameP(nickName);
        
        Debug.Log("----------------------------- save en- create teambot------------");
        
        TeamBotId a = new TeamBotId(nickName, p.ViewID);
        DBAgileBot.Instance.Save(a);
        yield return null; 
         
    }

    public static void setLogOut (bool logout)
    {
        log_out = logout;
    }

    public void cerrarAplicacion()
    {
        string avatar = PlayerPrefs.GetString("avatar");
        avatar = avatar + " - bot";
            
        //Posicion
        var bot = GameObject.FindGameObjectWithTag("Player"); 
        Transform t = bot.GetComponent<Transform>();
        string nickName = PlayerPrefs.GetString("nickname");

        if(PhotonNetwork.IsMasterClient){
            StartCoroutine(this.createTeamBot(avatar,nickName,Convert.ToString( t.position.x),Convert.ToString( t.position.y),Convert.ToString( t.position.z)));
        }else{
        StartCoroutine (ejecutarEnOrden(avatar,nickName,Convert.ToString( t.position.x),Convert.ToString( t.position.y),Convert.ToString( t.position.z)));
/*         ManejadorEventos.Publicar ("create_teamBot",
                                 "{\"nickName\":\"" + nickName +
                                  "\", \"avatar\":\""+ avatar +
                                  "\", \"x\":\""+ Convert.ToString( t.position.x)  +
                                  "\", \"y\":\""+ Convert.ToString( t.position.y) +
                                  "\", \"z\":\""+ Convert.ToString( t.position.z) +"\"}"
                                  ); */
        }




        //CreateAgileBot c = new CreateAgileBot();
        //var botCreado = c.crearBot();
        //if(botCreado)
        //Application.Quit();
    }
    /*
    public void cargarBacklog(ArtefactoContenedor columna)
    {
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        GameObject Backlog = Instantiate(PrefabPanelBacklog, PrefabPanelBacklog.transform.position,PrefabPanelBacklog.transform.rotation);
        Backlog.transform.SetParent(this.transform,false);
        columnas.Add(Backlog);
        ColumnController auxcc = Backlog.transform.Find("PanelBacklog").GetComponent<ColumnController>();
        auxcc.setArtefactoContenedor(columna);
        auxcc.setPadre(ArtefactoTablero);
        auxcc.setTitulo(columna.getNombre());
        
    }
*/
    public void volverMenu()
    {/*
        Destroy(GameObject.Find("Photon Voice Network"));
        //Destroy(GameObject.Find("PhotonMono"));
        Destroy(GameObject.Find("RoomManager"));
        Destroy(GameObject.Find("AmqpClient"));
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.UnloadSceneAsync("Menu");
        //Resources.UnloadUnusedAssets();
        SceneManager.LoadScene("Menu");
        // (IN MENU) PhotonMono, RoomManager,AmqpClient, (ONLY ON GAME) Photon Voice Network
        */
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;



using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Debug = UnityEngine.Debug;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class Log_Out_Button : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject bot;
    //SCRIPT QUE SE ENCARGA DE PRODUCIR LOS DISTINTOS LOG OUT POSIBLES.

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void cerrarAplicacion()
    {
        
        CreateAgileBot c = new CreateAgileBot();
        var botCreado = c.crearBot();
        if(botCreado)
            Application.Quit();
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

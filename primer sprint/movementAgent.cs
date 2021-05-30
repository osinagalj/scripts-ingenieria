using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class movementAgent : MonoBehaviour
{
    public DestionationController destinations; //El controlador de destinos.
    public AgentController controller; //El controlador de agentes.
    public NavMeshAgent agent; //El agente mesh del avatar.
    public Animator animator; //el animador del avatar.
    public string name;
    public static int globalIndex; //ultimo identificador global de los agentes navMesh dentro de la oficina.
    public int localIndex; //identificador univoco del agente navMesh particular.
    public Transform target; //El lugar donde debe quedar observando el agente una vez llega a destino.
    private Quaternion _lookRotation; //Auxiliar quaternion para calcular el lugar a donde tiene que rotar el avatar una vez llega a destino.
    private Vector3 _direction; //IDEM que lookRotation.
    public float RotationSpeed=1000; 
    public Transform camTransform; //La posicion espacial de la camara del TeamMember.

    bool isMoving; //indica si el agente se esta movi

    // Start is called before the first frame update
    //Inicializacion de los componentes.
    void Start()
    {
        isMoving = false;
        agent.updatePosition = true;
        destinations = FindObjectOfType<DestionationController>(); //Se encuentra el destination controller
        name = "user" + globalIndex;     //Se obtiene el nombre del agente, que resulta de una combinaci�n de "User<id_global>", siendo la id_global la actual en ese momento de creaci�n.
        localIndex = globalIndex; //Se captura el indice local del agente. 
        globalIndex++;
        controller = FindObjectOfType<AgentController>(); 
        //controller.addAgent(this);
        //camTransform = this.transform.Find("Camera").transform;
    }


    public string getName()
    {
        return name;
     }


    //metodo encargado de la rotaci�n del avatar.
    public void rotate()
    {
        //transform.LookAt(target);      
        Vector3 targetPostition = new Vector3(target.position.x, this.transform.position.y, target.position.z); //se calcula la distancia a el target.
        this.transform.LookAt(targetPostition); //se lo observa.
       // camTransform.rotation = new Quaternion(0.5f,camTransform.rotation.y,camTransform.rotation.z,camTransform.rotation.w); //se corrije la rotaci�n.
    
    }

    // Update is called once per frame
    void Update()
    {
        if ((isMoving) && (agent.remainingDistance != 0)) //Si el agente se esta movimiendo.
            if (agent.remainingDistance > agent.stoppingDistance) //Si el agente aun no llego a destino 
            {
                this.GetComponent<Personaje>().movimientoAutomatico = true; //Se anima automaticamente.
                this.GetComponent<Personaje>().automaticMovement();
                //SE DEBE ESTAR MOVIENDO CON EL MATI CONTROLLER.
                Debug.Log("CC: MOVING");
            }
            else
            {
                Debug.Log("CC: NOT MOVING remaining distance = " + agent.remainingDistance + " , stoppingDistance = " + agent.stoppingDistance);
                this.GetComponent<Personaje>().movimientoAutomatico = false; //Si el agente no se esta moviendo, no se debe mover.
                //NO SE DEBE ESTAR MOVIENDO EL MATI CONTROLLER. 
                Debug.Log("CC: NOT MOVING");
                rotate();
                isMoving = false;
                agent.enabled = false;
            }

        if (Input.GetKeyDown(KeyCode.Alpha5))    //Al ingresar el input 5, se indica el movimiento del agente. 
        {
            agent.enabled = true; //probablemente esto se setea cuando se llama a runAgent automaticamente, pero por las dudas vio
            this.runAgent("kanban");
            isMoving = true;
        }
    }

    public void runAgent(string locationName) //Metodo que se invoca para poner a correr un agente. 
    {	
        
    	agent.enabled = true; //probablemente esto se setea cuando se llama a runAgent automaticamente, pero por las dudas vio
 	
 	    Debug.Log("----------------------------- debug destinarion -------------" + locationName);
        
        Vector3 destination = destinations.getPositionDestination(locationName); //destinations
        target = destinations.getToLook();
        agent.SetDestination(destination);
        isMoving = true;
        

    }
}

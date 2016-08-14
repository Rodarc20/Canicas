using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour {
    public int m_NumeroCanicas = 5;
    public int m_LanzamientoNumero = 0;
    //deberia tener algunos delay
    public CameraControl m_CameraControl;
    public Text m_Score;
    public Text m_WinText;
    public Slider m_ForceSlider;

    public GameObject m_ObjetivoPrefab;
    public GameObject m_PlayerPrefab;//esta deberia ser una referencia al prefab, y un jugador manager, para que cunete los que entran y salen, este es una bola
    //este es un prefab jugaddor
    public GameObject m_Player;//esta es la instancia de una bola//este es el jugador, no la pelota
    public Rigidbody m_CanicaPlayer;//instancia de la canica del jugador
    public Collider m_GameZone;
    public Transform m_SpawnPosition;
    private int m_Puntos = 0;
    public Transform[] m_Objetivos;
    public void Awake(){
        SetCameraInitial();
        //SetCameraTarget();//quiza esto deberia estar dentro de spawnplayer();
    }

    public void Start(){
        SpawnPlayer();
        SpawnObjectives();
    }
    public void SpawnPlayer(){//instancio a todos los jugadores, pero solo al que le toca turano esta con SetActive() true;
        m_Player = Instantiate(m_PlayerPrefab, m_SpawnPosition.position, m_SpawnPosition.rotation) as GameObject;
        m_Player.GetComponent<PlayerAim>().m_CenterGameZone = m_GameZone.GetComponent<Transform>();//estas referencias, pebere establecerlas cada ve que cambio de jugador, no se si sea conveniete, o intentar trabajar conel PlayerManager para que las administre, leer anotacion del playaer manager
        m_Player.GetComponent<PlayerAim>().m_SpawnPoint = m_SpawnPosition;
        m_Player.GetComponent<PlayerThrow>().m_Fuerza = m_ForceSlider;
        NuevoLanzamiento();
        /*m_Player.GetComponent<PlayerThrow>().Setup();//quiza al instanciarse, solo deberia geenrar su propia bola
        m_CanicaPlayer = m_Player.GetComponent<PlayerThrow>().m_CanicaPlayer.GetComponent<Rigidbody>();//debieria haber una mejor forma de acceder a esta canica, quiza obtener la referencia a travez de una funcion de playerthrow
        */
        SetCameraTarget();//quiza esto deberia estar dentro de spaenplayer();
    }

    public void SpawnObjectives(){//almacenados
        m_Objetivos = new Transform [m_NumeroCanicas];//o quiza geerar esto al comienxo
        //for(int i = 0; i < m_NumeroCanicas; i++){//deberi usar m_Obejtivos.Length
        for(int i = 0; i < m_Objetivos.Length; i++){//deberi usar m_Obejtivos.Length
            GameObject obj = Instantiate(m_ObjetivoPrefab, posicionValida(), Quaternion.identity) as GameObject;//

            //lo que hare es crear un transform, y posicionarlo segun los calculos, luego ese trasnform lo verifica si es valido, hago la isntancia usasnod ese transform como posicion
        }        
    }

    //deberia tener una funcion que me de una posicion valida
    private Vector3 posicionValida(){
        Transform posicion = Instantiate(m_SpawnPosition, new Vector3 (0f, 0.5f, 0f), Quaternion.identity) as Transform;//donde probare las posiciion generada, este es una clon del objeto trasnsform
        //no es aconsejable usar el transform de este gamobject, falla
        posicion.position = new Vector3 (0f, 0.5f, Random.Range(0f, 8f));//podira mezclasr la anterior
        posicion.RotateAround(transform.position, Vector3.up, Random.Range(0f, 360f));//obtener defrente la rotacion*/
        while(!EsValido(posicion)){
            posicion.position = new Vector3 (0f, 0.5f, Random.Range(0f, 8f));//podira mezclasr la anterior
            posicion.RotateAround(transform.position, Vector3.up, Random.Range(0f, 360f));//obtener defrente la rotacion*/
        }
        return posicion.position;
    }

    private bool EsValido(Transform posicion){
        bool result = true;
        for(int i = 0; i < m_Objetivos.Length /*&& result*/; i++){//odira reducir un if aqui a dentro, si recibiera el i desde el ques se llamo en el spawnobjectives
            // si result se hace falso, eso quiere decir que con alguien colisisona, por lo tanto el for debe terminar, asi evito el if de alli comentado abajo
            if(m_Objetivos[i]){
                result = result && Vector3.Distance(posicion.position, m_Objetivos[i].position) >= 1f;
                /*if(Vector3.Distance(posicion.position, m_Objetivos[i].position) >= 1f){//aun que si tuviera un if para que retorne defrente la funcion no pasaria por todas siempre, no se de que forma es mejor
                }*/
            }
        }
        return result;
    }
    public void SetCameraTarget(){
        m_CameraControl.m_Player = m_Player;
        m_CameraControl.SetToPlayer();
    }
    public void SetCameraInitial(){
        m_CameraControl.SetStartPosition();
    }

    private void NuevoLanzamiento(){
        m_Player.GetComponent<PlayerThrow>().Setup();//quiza al instanciarse, solo deberia geenrar su propia bola
        m_CanicaPlayer = m_Player.GetComponent<PlayerThrow>().m_CanicaPlayer.GetComponent<Rigidbody>();//debieria haber una mejor forma de acceder a esta canica, quiza obtener la referencia a travez de una funcion de playerthrow
    }
    void OnTriggerExit(Collider other){
        //cuando todas las canicas se detengan, el turno finalizo
        GameObject m_canica = other.gameObject; //GameObject m_canica = other.GetComponent<GameObject>();
        if(m_canica.layer == LayerMask.NameToLayer("Objetivo")){//tengo que revisar que sea un objetivo, para sumar, y ver si es un jugador para no sumar, en ambos casos la bola se elimna}
            m_Puntos++;//esto esta bien para los objetivos
            Destroy(other.gameObject, 1f);//para que desaparezcan dos segundo despues, ahora el problema es que cuando destruyo una, no la he quitado del array
            SetTextScore();//otr opcion es solo llamar cuando haya modicificacion
        }
        if(m_Puntos == m_NumeroCanicas)
            m_WinText.color = Color.white;//aqui debo finalizar el juego
    }
    public void SetTextScore(){
        string s = "Puntos: " + m_Puntos + "\nLanzamientos: " + m_LanzamientoNumero;
        m_Score.text = s;
    }
    //debo almacenar todos los rigidbody de todas las canicas, cuando en
    public void FixedUpdate(){//las preguntas deberian estar encapsuladas con respecto al jugador, quiza 
        //aqui debo verificar que todas las pelotas esten quietas para dar por finalizado el turno
        //tambien deberia comprobar que mi cnica haya sido disparada para incrementar el lnuemro lanzamiento
        bool finalizoLanzamiento = true;
        finalizoLanzamiento = finalizoLanzamiento && (m_CanicaPlayer.IsSleeping() && m_CanicaPlayer.GetComponent<CanicaPlayer>().m_Fired);//di la calinca no se mueve, y ya fue disparada,entoces debe finalizar el alnzamineto
        //print("Jugador: " + finalizoLanzamiento);

        for(int i = 0; i < m_Objetivos.Length; i++){
            if(m_Objetivos[i]){//este IsSleeping, por que creo que nunca la la velocidad e la poelota entra en el rango minimo que estableci, para la canica funciona bien, pero para los objtivos aprece que no
                finalizoLanzamiento = finalizoLanzamiento && m_Objetivos[i].GetComponent<Rigidbody>().IsSleeping();//si esta quito, retorna verdadero, si se mueve falso,
                //print("Objetivo " + i + ": " + m_Objetivos[i].GetComponent<Rigidbody>().IsSleeping());
            //laidea es ir comprobanto que todo este quieto, si alguno no esta quieto, finalizoLanzamineto deberia terminar en falso
            }
        }
        if(finalizoLanzamiento){
            print("Finalizo Lanzamiento");
            Destroy(m_CanicaPlayer.gameObject, 1f);//para que desaparezcan dos segundo despues//esto funciona en Colliders no en Rigidbody por lo visto

            /*m_Player.GetComponent<PlayerThrow>().Setup();//quiza no deberia reinicar la camara, o tener dos funcines, una para reinicar balon, y otra para reiniciar posicion
            m_CanicaPlayer = m_Player.GetComponent<PlayerThrow>().m_CanicaPlayer.GetComponent<Rigidbody>();//debieria haber una mejor forma de acceder a esta canica, quiza obtener la referencia a travez de una funcion de playerthrow
            */
            NuevoLanzamiento();
            //estas dos lineas siempre van juntas, deberia ponerlas dentro d una funcion
            m_LanzamientoNumero++;//este incremento no ha funcionado
            SetTextScore();
            //como ya finalizo el lanzamiento, toca un cambio de turno, pero or ahora solo le dare una nueva pelota al jugadro
        }//revisar las logicas, a veces no entra en esta cosa
    }
}   
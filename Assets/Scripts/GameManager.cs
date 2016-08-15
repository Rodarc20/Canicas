using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour {
    public int m_NumeroCanicas = 5;
    public int m_LanzamientoNumero = 0;//esto podria se un contador de turnos
    //deberia tener algunos delay
    public CameraControl m_CameraControl;
    public Text m_Score;
    public Text m_WinText;
    public Slider m_ForceSlider;

    public GameObject m_ObjetivoPrefab;
    public GameObject m_PlayerPrefab;//esta deberia ser una referencia al prefab, y un jugador manager, para que cunete los que entran y salen, este es una bola
    //este es un prefab jugaddor
    //public GameObject m_Player;//esta es la instancia de una bola//este es el jugador, no la pelota
    //public Rigidbody m_CanicaPlayer;//instancia de la canica del jugador
    public Collider m_GameZone;
    public Transform m_SpawnPosition;
    //private int m_Puntos = 0;
    public Transform[] m_Objetivos;

    //si no establezco el tamaño de esto en el inspector y lo dejo en 0 esto fallara, intentar decorregir o preveer este caso desde el codigo
    public PlayerManager[] m_Jugadores;//array de script PlayerManager, el tamaño se establece en el inspector, junto con algunos de sus parametros, como el color
    public PlayerManager m_GameWinner;//para conservar al ganador de la ronda..si fuera necesario, podria eliminarse
    private int m_CurrentPlayer = 0;
    private bool m_FinalizoLanzamiento = false;

    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    public void Awake(){
        SetCameraInitial();
        //SetCameraTarget();//quiza esto deberia estar dentro de spawnplayer();
        SpawnPlayers();
        SpawnObjectives();

    }

    public void Start(){
        //SpawnPlayer();
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        StartCoroutine(GameLoop());//o solo game
    }
     public void SpawnPlayers(){
         for(int i = 0; i < m_Jugadores.Length; i++){
             m_Jugadores[i].m_Player = Instantiate(m_PlayerPrefab, m_SpawnPosition.position, m_SpawnPosition.rotation) as GameObject;
             m_Jugadores[i].m_PlayerNumber = i+1;
             m_Jugadores[i].Setup();
             //intenar encapsular esto en una funcion de playermager, quiza estos parametro que estoy usando aqui los deba recibir tambien playermanagera o quiza ocmo parametros de funcion
             m_Jugadores[i].m_Aim.m_CenterGameZone = m_GameZone.GetComponent<Transform>();
             m_Jugadores[i].m_Aim.m_SpawnPoint = m_SpawnPosition;
             m_Jugadores[i].m_Throw.m_Fuerza = m_ForceSlider;
         }
     }

    public void SpawnObjectives(){//almacenados
        m_Objetivos = new Transform [m_NumeroCanicas];//o quiza geerar esto al comienxo
        for(int i = 0; i < m_Objetivos.Length; i++){//deberi usar m_Obejtivos.Length
            //GameObject obj = Instantiate(m_ObjetivoPrefab, posicionValida(), Quaternion.identity) as GameObject;//esta es la forma correcta de instancion para en la siguiente linea usar todos sus componentes
            GameObject obj = Instantiate(m_ObjetivoPrefab, posicionValida(), Quaternion.identity) as GameObject;//
            //en teoria falta asignar a m_Onjetivos, los transform correctos, no se por que esta funcionanado
        }        
    }

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

    private bool EsValido(Transform posicion){//mejorar esta funcion
        bool result = true;
        for(int i = 0; i < m_Objetivos.Length && result; i++){//mejorar esto
            if(m_Objetivos[i]){//si existe verifico, si no existe me los salto, aun que no es deberia hacer esot, ya que compuero con los anterioro,es decir si agrego la cuarta canica, eso quiere decir que las 3 anteriores esta y solo con esas debo comparar
                result = result && Vector3.Distance(posicion.position, m_Objetivos[i].position) >= 1f;
            }
        }
        return result;
    }
    public void SetCameraTarget(){
        m_CameraControl.m_Player = m_Jugadores[m_CurrentPlayer].m_Player;
        m_CameraControl.SetToPlayer();
    }
    public void SetCameraInitial(){//no se si esto siempre es necesario, en lugar de llamar donde necesito a la linea de abajo
        m_CameraControl.SetStartPosition();
    }

    private void NuevoTurno(){//lla llamare en turn startign, aun que no se si es necesario tenrela aparte o dejarla dentro
        //al final de cada turno, se incrementa la variable del current
        //la psociion de aquin le toca la debo calcular con modulo
        //todos los jugadores debe iniciar deshabilitados, en esta funcion los habilitare uno a uno
        m_Jugadores[m_CurrentPlayer].m_Player.SetActive(true);//y deberia estar listo para un nuevo lanzamiento o llamar a esa funcion
        SetCameraTarget();//esta funcion deberia leer al m_CurrentPlayer/
        m_Jugadores[m_CurrentPlayer].NewThrow();
        m_FinalizoLanzamiento = false;
    }
    private void FinalizarTurno(){
        //SetCameraInitial();
        m_Jugadores[m_CurrentPlayer].m_Player.SetActive(false);
        Destroy(m_Jugadores[m_CurrentPlayer].m_CanicaPlayer.gameObject);
        //aui debo comprabar que hya ganador
        //creo que solo debo llamar a endmessage
        SetEndMessage();
        //if(m_NumeroCanicas == 0){
            //bool win = GetWinner();//
        //}
        //m_CurrentPlayer = (m_CurrentPlayer + 1) % m_Jugadores.Length;//esto prvoca errores,p por que si bien ya finalizo el turno, estoy esperndo 3 segudno antes de empezar el siguiente, sin embargo yo ya actualice, m_CurrentPlayer a la siguiente posicion, que no tiene una canicaplayer provocan un nullreference
    }

    private bool GetWinner(){
        //esta funcion buscara si hay un ganador, o si hubo empate, debo lamacenar un booleano para el empate, por ejemplo que lo retorne lafuncion, treu si el ganador es absoluto, falso si hay empate
        //m_GameWinner almacena al gaador, podria ser que esta funcion solo conserve el indice, en lugar del script, se usa es script, por qeu si o hay ganador, este se queda con valor null
        bool win = true;
        int max = 0;
        for(int i = 0; i < m_Jugadores.Length; i++){
            if(m_Jugadores[i].m_ObjetivosObtenidos > max){//no usao >= por uqe esto hara que si tiene todos 0, m_gamevinner deje de ser null
                m_GameWinner = m_Jugadores[i];
                max = m_Jugadores[i].m_ObjetivosObtenidos;
                win = true;//puede que encuentre un maximo al cominezo, y luego otro jugdor lo empate, y vuelva a encontrar un max, por eso debo volver a ponerlo true
            }
            else if (m_Jugadores[i].m_ObjetivosObtenidos == max){
                win = false;
            }
        }
        //si max, se qued en si m_gamenanger se queda en null, entonces no hay ganador, por que todoshicieron 0 puntos, lo cual es imposible
        return win;
        //quiza pdoira aprovechar este recorrido para la tablad e posiciones
    }
    void OnTriggerExit(Collider other){
        //cuando todas las canicas se detengan, el turno finalizo
        GameObject canica = other.gameObject; //GameObject m_canica = other.GetComponent<GameObject>();
        if(canica.layer == LayerMask.NameToLayer("Objetivo")){//tengo que revisar que sea un objetivo, para sumar, y ver si es un jugador para no sumar, en ambos casos la bola se elimna}
            m_Jugadores[m_CurrentPlayer].m_ObjetivosObtenidos++;
            Destroy(other.gameObject, 1f);//para que desaparezcan dos segundo despues, ahora el problema es que cuando destruyo una, no la he quitado del array
            //tal vez no sea necesario destruirlos// o llamar al destructor en turn ending
            SetTextScore();//otr opcion es solo llamar cuando haya modicificacion//quiz esto se deba modificar de acuero al puntaje del jugador en turno
            //deberia tener un nuemro de canicas restantes, cuando sea 0, el turno continue, ejemp while(numeroobejtos)
            //aui deberia estar restando un valor de alguna variable
            m_NumeroCanicas--;
        }
        //el siguiente if no deberia estar aui, sol
        //if(m-puntos ++ numero de canicas)
    }
    public void SetTextScore(){
        string s = "Jugador " + (m_CurrentPlayer + 1) + "\nPuntos: " + m_Jugadores[m_CurrentPlayer].m_ObjetivosObtenidos + "\nLanzamientos: " + m_Jugadores[m_CurrentPlayer].m_LanzamientosRealizados;
        m_Score.text = s;
    }
    public void SetEndMessage(){//se musetra al final de cada turno
        //por ahora no mostrara de forma ordenada las posiciones de los jugadores, 
        m_Score.color = Color.clear;
        string message = "";//quiza solo deberia llamar a get winner en caso de que tenga las cnicas sean 0, en este caso la funcion get wiiner, que se ejecuta despues de esta, agregara al cominezo de mensaje
        if(m_NumeroCanicas == 0){
            if(GetWinner()){//hubo ganador absoluto
                message = "Jugador " + m_GameWinner.m_PlayerNumber + " gana!!!";//esta linea deberia tener un tamañomas grande
            }
            else{
                message = "Empate";
            }
            message += "\n\n";
        }
        message += "Jugador\tPuntos\n";
        //ahora solo agrego la tabla de posiciones
        for(int i = 0; i < m_Jugadores.Length; i++){
            message += "Jugador " + m_Jugadores[i].m_PlayerNumber/*i+1*/ + "\t" + m_Jugadores[i].m_ObjetivosObtenidos + "\n";
        }
        m_WinText.text = message;
        m_WinText.color = Color.white;
    }
    public void FixedUpdate(){//las preguntas deberian estar encapsuladas con respecto al jugador, quiza 
        bool finalizoLanzamiento = true;//esto ahora deberia ser finalizoTurno chequear si alguien gano el juego, si ya temrino el juego, por mas que la ultima canica haya salido antes de que la pelota del jugador se haya detenedo, es alli debe terminar el turno y comprobar quein gano
        //m_FinalizoLanzamiento = m_FinalizoLanzamiento && (m_CanicaPlayer.IsSleeping() && m_CanicaPlayer.GetComponent<CanicaPlayer>().m_Fired);//di la calinca no se mueve, y ya fue disparada,entoces debe finalizar el alnzamineto
        //print(m_CurrentPlayer);
        finalizoLanzamiento = finalizoLanzamiento && m_Jugadores[m_CurrentPlayer].FinalizoLanzamiento();//di la calinca no se mueve, y ya fue disparada,entoces debe finalizar el alnzamineto

        for(int i = 0; i < m_Objetivos.Length; i++){
            if(m_Objetivos[i]){//este IsSleeping, por que creo que nunca la la velocidad e la poelota entra en el rango minimo que estableci, para la canica funciona bien, pero para los objtivos aprece que no
                finalizoLanzamiento = finalizoLanzamiento && m_Objetivos[i].GetComponent<Rigidbody>().IsSleeping();//si esta quito, retorna verdadero, si se mueve falso,
            }
        }
        if(finalizoLanzamiento){//cuando entro en este if//debo llamar a finalizar lanzamiento y comprobar si hubo ganador
            //deberia actualizar la variable
            m_FinalizoLanzamiento = finalizoLanzamiento;
            //como ya finlizo, dberia actualizar los valores
            //Destroy(m_CanicaPlayer.gameObject, 1f);//para que desaparezcan dos segundo despues//esto funciona en Colliders no en Rigidbody por lo visto
            SetTextScore();
            //como ya finalizo el lanzamiento, toca un cambio de turno, pero or ahora solo le dare una nueva pelota al jugadro
        }//revisar las logicas, a veces no entra en esta cosa
    }
    private IEnumerator TurnStarting(){//mostrara un mensaje de a que jugador le toca jugar, por ahora todos usaran las mismas teclas, pero esto es opcinal, se puede modificar
        //esta funcion demorara unos 2 o 3 segundo, solo en mostrar este mensaje, los ocmtroles deberian estar deshabilitados, tener cuidado con esto, no queremos destruircanicas inecesariamente
        //a qui se llama a los setup del jugaodr de turno, o a la fucnion nuevo turno, que hace eso
        NuevoTurno();
        //mensajito en win texte, turno jugador x
        m_Score.color = Color.clear;
        m_WinText.text = "Turno Jugador " + (m_CurrentPlayer + 1);
        m_WinText.color = Color.white;//esto por ahora, mas adelante el dare color d otra fomra para que sea diferente
        m_Jugadores[m_CurrentPlayer].DisableControl();//espero que no cause poroblemas
        yield return m_StartWait;

    }

//tambien debo actualizar el numero de lanzamientos, al lanzar, no al terminar, pero de cada jugador
    private IEnumerator TurnPlay(){//aqui el jugaro podra moverse, y lanzar, aqui se establencen los calculos e cuantas pelotas salieron
        SetTextScore();
        m_Score.color = Color.white;
        m_WinText.color = Color.clear;
        m_Jugadores[m_CurrentPlayer].EnableControl();//estas funciones podria omitirlas para probar e
        //para determinar que un turno acabo, debo verificar que todas la pelotas se hayan detenido, sino, sigo esperando
        //cuando se detuvieron todas las canicas esta funcion terminara
        while(!m_FinalizoLanzamiento){//o podira ocmprobarun booleando de temrino lanzamiento, que tendria que sacarlo de la funcion en la que esta, y ponerlo en falso cada nuevo turno
            yield return null;
        }
    }

    private IEnumerator TurnEnd(){//aqui se deshabilita el control del jjugador en turno, se hace un balance de los puntajes, y se va determinando un ganador, quiza establecer una tabal de posiciones el mayor es el que gana
        //si todas las pelotas fueron retiradas, se establece al ganador del juego, se menada un mensaje de que el juego termino,y en otro linea posteriora la llamada de esta funcion, se llama a la recarga de la escena
        //aqui se debe incremntar el valor de m_CurrentPlayer, para dejar listo para el siguinete jugador
        //auiq debo verificr si hubo algun ganador, y mostrar el mensaje respectivo, para esto debo recoorer en caso de que ya no queden pelotitas, quien fue el que gano, todo el array de jugadores
        //buscando el que tenga mas puntso, tener cuidado con los empatese,  podria desempatar algunos casos con  el numero de lanzamientos pero no se podra siempre
        FinalizarTurno();//los jugadores se quedan en ese posicin y al siguiente turno empiezan donde se quedaron
        m_Jugadores[m_CurrentPlayer].DisableControl();//estas cosas si funcion odiran estar dentro de finalizar turno
        //quiza disable control siinhabilita los rigidbody, lo cual hari fallar algunas cosas, podria solo desactivar aim, pero podria aun presionar fire1
        //aqui deberia mostrar un score general, pero por ahora solo pondre que finalizo el turno en caso de que no haya ganador, aqui debo verificar si lohubo
        //destruir la canica que estoy usado
        yield return m_EndWait;//mientras espera, lo que se ejecutan son los updates y los fixed update
    }

    private IEnumerator GameLoop(){
        while(m_NumeroCanicas > 0){
            yield return StartCoroutine(TurnStarting());
            yield return StartCoroutine(TurnPlay());
            yield return StartCoroutine(TurnEnd());
            Destroy(m_Jugadores[m_CurrentPlayer].m_CanicaPlayer);//puedo destruir de algo que esta inhabilitado, y si esta inhabilitado, por que sigue calculdand su fixed update
            m_CurrentPlayer = (m_CurrentPlayer + 1) % m_Jugadores.Length;
        }
        SceneManager.LoadScene(0);//no ha cargado bien las luces de esta cosa
    }
}   
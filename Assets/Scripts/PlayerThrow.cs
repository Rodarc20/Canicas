using UnityEngine;
using UnityEngine.UI;//aqui hare modificaciones al ui, por ejemplo la barra de fuerza o marcador de llegada

public class PlayerThrow : MonoBehaviour {
    public int m_PlayerNumber = 1;
    public Slider m_Fuerza;
    public float m_MinForce = 0f;
    public float m_MaxForce = 100f;
    public float m_MaxChargeTime = 2f;//un segudno en cargar toda la barra de fuerza
    public GameObject m_CanicaPlayerPrefab;
    public GameObject m_CanicaPlayer;//instancia de una canica de jugador
    private CanicaPlayer m_ScriptCP;

    private string m_ThrowButton;
    private float m_CurrentThrowForce;
    private float m_ChargeSpeed;
    private bool m_Throwed;

    void Start(){
        m_ThrowButton = "Fire1";
        m_ChargeSpeed = (m_MaxForce - m_MinForce) / m_MaxChargeTime;
        //Setup()//por ahora lo llamo dentro de spawnPlayer en el gamemanager
    }
    private void OnEnable(){
        m_CurrentThrowForce = m_MinForce;
    }

    public void Setup(){
        m_Throwed = false;
        m_CurrentThrowForce = m_MinForce;
        m_CanicaPlayer = Instantiate(m_CanicaPlayerPrefab, transform.position, transform.rotation) as GameObject;
        if(m_CanicaPlayer){
            m_ScriptCP = m_CanicaPlayer.GetComponent<CanicaPlayer>();
            m_ScriptCP.m_Player = transform; //m_CanicaPlayer.GetComponent<CanicaPlayer>().m_Player = transform;
        }
    }
    private void Update(){
        //si me paso del maximo de la barra no debo lanzar la canica, por que puede que el jugador aun quiera modificar la direccion, por ello podra aun moverse, solo se disparara cuando el jugador suelte la tecla de deisparo
        if(m_CurrentThrowForce >= m_MaxForce && !m_Throwed){//si la fuerza esa mayor que el maximo, y aun no he disparado, entonces solo establesco el current en el max
            m_CurrentThrowForce = m_MaxForce;//se dispara solo cuando el jugador suslete la tecla
            m_Fuerza.value = m_CurrentThrowForce;//hay problemas con este if,buscar solucion
        }
        else if(Input.GetButtonDown(m_ThrowButton)){//cuando presioo por primera vez el boton,, su bi com pu
            m_Throwed = false;
            m_CurrentThrowForce = m_MinForce;
            m_Fuerza.value = m_CurrentThrowForce;
        }
        else if(Input.GetButton(m_ThrowButton) && !m_Throwed){//cuando mantendo presionado el boton pero aun no he disparado
            m_CurrentThrowForce += m_ChargeSpeed * Time.deltaTime;
            m_Fuerza.value = m_CurrentThrowForce;
            //aqui tambien van modificaciones la slider de la fuerz de lanzamiento
        }
        if(Input.GetButtonUp(m_ThrowButton) && !m_Throwed){//cuadno suelto el boton y aun no he disparado, eliminado el elseif
            //m_Throwed = true;
            Fire();
        }
        //hay un erro que impide que dispare, por ejemplo si mntego presionado espacio antes de que me habiliten el disparo
    }
    private void Fire(){
        m_Throwed = true;
        m_ScriptCP.Fire(transform.forward * m_CurrentThrowForce);//ninguna de las dos funciona, el proble es que en cada update lo regresa a la posicion del jugador, cuando no este disparando
    }//una ve z que se ha disparado, debo deshabilitar los controles, la pelota sigue por su cuenta
}
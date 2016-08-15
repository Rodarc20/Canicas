using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerManager {//no estoy usando esta cosa, seria util si usara varios jugadores
    public Color m_PlayerColor;
    [HideInInspector] public string m_ColoredPlayerText;
    public Transform SpawnPosition;//quiza no sea necesario, darle uso a esto, para no sltarme tantas clases en una sola linea
    public Slider m_ForceSlider;//ni este tampoco
    [HideInInspector] public int m_PlayerNumber = 1;

    //public GameObject m_PlayerPrefab;//para la forma2, el prefab del jugador
    [HideInInspector] public GameObject m_Player;//esta sera la instancia de un objeto jugadora, hay dos formas de instanciar este objeto, a travez del gamemanager, y otra a travez de este script, probar ambas
    //el anterior parametro puede que es publico para poder acceder a el desde afuera
    public Rigidbody m_CanicaPlayer;//referencia a la canica del jugador
    private bool m_FinLanzamiento;
    public int m_LanzamientosRealizados = 0;
    public int m_ObjetivosObtenidos = 0;//cada jugaro contara sus puntajes, en el gamenayer cuando salgan todos solo vera quien obtuvo el mayor de los puntajes

    [HideInInspector] public PlayerAim m_Aim;//referencia a los scripts de m_Player
    [HideInInspector] public PlayerThrow m_Throw;//esto son para poder habilitar y deshabilitar el control una vez que se realizo un lanzamiento, aun que dberia hacerlo de forma iterna

    public void Setup(){ //para establecer las referencias y valores inicales
        //m_Player = Instantiate(m_PlayerPrefab, m_SpawnPosition.position, m_SpawnPosition.rotation) as GameObject;//esta es para la forma 2//sin embargo este escript no es hernecia de MonoBehavior, por lo tanto no tengo la funcion Instantiate, por eso la forma 2 no funciona
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">Jugador " + m_PlayerNumber + "</color>";//parecidoa  html 
        m_FinLanzamiento = false;
        m_Aim = m_Player.GetComponent<PlayerAim>();
        m_Throw = m_Player.GetComponent<PlayerThrow>();
        m_Player.SetActive(false);//todos los jugadores deben iniciar inactivos, los cativara y desactivara el gamemanager cuadno seasu turno
    }
    public void NewThrow(){//esta se llamara al inicio de cada turno, al igual quiza que enable control, la camara tambien se debe asiganar a cada jugador correspondiento
        m_FinLanzamiento = false;
        m_Throw.Setup();//talvez no sea necesario, ademas podria hacer que retorne la referencia al rigidbbody de la canica para quepueda ser util, si la quisiera conservar
        m_CanicaPlayer = m_Throw.m_CanicaPlayer.GetComponent<Rigidbody>();
        //aun que tal vez no sea necesario
    }
    public void EnableControl(){
        m_Aim.enabled = true;
        m_Throw.enabled = true;
    }
    public void DisableControl(){
        m_Aim.enabled = false;
        m_Throw.enabled = false;
    }
    public bool FinalizoLanzamiento(){//esta funcion debe haberse asegurado de haber contado todo, para que desde aqui se desactive el gameobjet jugador(m_Player.SetActive(false)), o hacerlo desde el gamemanager
        //if(!m_FinLanzamiento)//este if no es necesaio, solo erapor el error anterior
        if(!m_FinLanzamiento && m_CanicaPlayer != null)//este if no es necesaio, solo erapor el error anterior
            m_FinLanzamiento =  m_CanicaPlayer.IsSleeping() && m_CanicaPlayer.GetComponent<CanicaPlayer>().m_Fired;
        return m_FinLanzamiento;//no era esto
        //hay un poroblema con esta funcion, por alguna razon se llaa, pero cuadno m_CanicaPlayer no existe, provicando errores
    }
}
//por ejemplo la barra de fuerza, todos tendran que tenerla referenciada, pero como solo habra un jugador activo, solo ese poodra modificarla

//por ahora trabajar con que genera una nueva canica, pero luego intentar que sea solo una, es decir que esta se quede en la scene y solo se mueva a donde necesito
//este escript tambien debe tener una funcion boleana, para preguntar si finalizo el turno, o smplmente una variable, que pueda consultar, depende de como lo quiera hacer, seria mejor la funcion
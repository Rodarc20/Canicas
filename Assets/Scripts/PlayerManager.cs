using System;
using UnityEngine;

[Serializable]
public class PlayerManager {//no estoy usando esta cosa, seria util si usara varios jugadores
    public Color m_PlayerColor;
    public Transform SpawnPosition;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public GameObject m_Player;//esta sera la instancia de un objeto jugador
    public int m_LanzamientosRealizados;
    public int m_ObjetivosObtenidos;//cada jugaro contara sus puntajes, en el gamenayer cuando salgan todos solo vera quien obtuvo el mayor de los puntajes

    private PlayerAim m_Aim;
    private PlayerThrow m_Throw;//esto son para poder habilitar y deshabilitar el control una vez que se realizo un lanzamiento, aun que dberia hacerlo de forma iterna

    
    public void NewThrow(){
        m_Throw.Setup();
    }
    public void EnableControl(){
        m_Aim.enabled = true;
        m_Throw.enabled = true;
    }
    public void DisableControl(){
        m_Aim.enabled = false;
        m_Throw.enabled = false;
    }
}
//por ejemplo la barra de fuerza, todos tendran que tenerla referenciada, pero como solo habra un jugador activo, solo ese poodra modificarla
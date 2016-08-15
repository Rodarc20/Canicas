using UnityEngine;

public class CameraControl : MonoBehaviour {//esta cosa deberia esar en dentro de un obejto adicional, no en la camara
    public GameObject m_Player;//quiza deba ser solo un Transform, o array de transforma para varios jugadores
    private Vector3 m_Offset;
    private Vector3 m_InicialPosition;
    private Quaternion m_InicialRotation;
    public Transform m_InitialParent;

    public void Start(){
        m_InicialRotation = transform.rotation;
        m_InicialPosition = transform.position;
    }
    public void Update(){//era Last
        transform.position = m_Player.transform.position;//pero esto deberia ser relativo
        Quaternion rotate = m_Player.transform.rotation * Quaternion.AngleAxis(45f, Vector3.right);//para corregir la rotacion, por haber tomado el spawnpint como inical
        transform.rotation = rotate;//pero esto deberia ser relativo
    }
    public void SetStartPosition(){
        transform.position = m_InicialPosition;
        transform.rotation = m_InicialRotation;
    }
    public void SetToPlayer(){//seria necesaria si utilizo varios jugadores
        SetStartPosition();//para que sigan al jugador y no el 
    }//mejorar el contol de la camara, para que no salte d jugadro a jugadro, si no que se traslade alrededor de la zona de juego
}
using UnityEngine;
//using UnityEngine.UI;

public class CanicaObjetivo : MonoBehaviour {
    //public bool m_Move;
    public Rigidbody m_Rigidbody;
    public float m_Desaceleracion = 0f;
    public void Awake(){//con este script puedo controlar lo de los puntos, para que no aparezcan dos cen el mismo lugar y se toquen al cominenzo
        m_Rigidbody = GetComponent<Rigidbody>();
        //m_Move = false;
    }
    
    public void FixedUpdate(){
        Vector3 direccion = m_Rigidbody.velocity.normalized;//direccion antes de aplicar la desaceleracion
        if(m_Desaceleracion != 0f){
            m_Rigidbody.AddForce(m_Rigidbody.velocity.normalized * -1 * m_Desaceleracion, ForceMode.Acceleration);//esta desaceleracion funciona
        }
        //print(m_Rigidbody.velocity.magnitude);
        if((m_Rigidbody.velocity.magnitude <= 0.01f || m_Rigidbody.velocity.normalized == direccion*-1f) && m_Rigidbody.velocity != Vector3.zero){//este evita que entre constante menete a reemplazar por vector zero
            //quiza el problema es que el vector de velocidad cambia antes de llegar aqui, y por eso nunca tiene la misma direccion
            m_Rigidbody.isKinematic = true;
            m_Rigidbody.isKinematic = false;
        }
    }
}
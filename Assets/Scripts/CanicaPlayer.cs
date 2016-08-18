using UnityEngine;

public class CanicaPlayer : MonoBehaviour {
    public bool m_Fired;//si ha sido disparado
    [HideInInspector] public int m_PlayerNumber;
    public Rigidbody m_Rigidbody;
    public Transform m_Player;//este es la posicion del Jugador
    public PlayerThrow m_PlayerThrow;
    public float m_Desaceleracion = 0f;
    public void Awake(){
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Fired = false;
    }
    public void Update(){
        if(!m_Fired){
            transform.position = m_Player.position;//con esto las canicas disparadas, no siguen la traslacion del jugador
        }
    }
    public void FixedUpdate(){
        if(m_Fired && m_Rigidbody != null){
            Vector3 direccion = m_Rigidbody.velocity.normalized;
            if(m_Desaceleracion != 0f){
                m_Rigidbody.AddForce(m_Rigidbody.velocity.normalized * -1 * m_Desaceleracion, ForceMode.Acceleration);//esta desaceleracion funciona
            }
            if((m_Rigidbody.velocity.magnitude <= 0.01f || m_Rigidbody.velocity.normalized == direccion*-1f) && m_Rigidbody.velocity != Vector3.zero){//este evita que entre constante menete a reemplazar por vector zero
                //en este momento el movimiento ya es muy pequeño, puedo cambia r los valores de volocity y angleVelocity a 0, para detener los calculos corespondientes, y en el caso de canica player terminar el turno
                m_Rigidbody.isKinematic = true;//esto deteiene el movimieitno, evita que le afecten fuerzas fisicas
                m_Rigidbody.isKinematic = false;//esto lo vuelve a poner modificable por fuerzas fisicas
                //m_Rigidbody.velocity = Vector3.zero;
                //m_Rigidbody.angularVelocity = Vector3.zero;
                //hay un pequeño error al comienzo del lanzamiento, me deja entrara a esta funcion uan vez antes de que entre en contacto con el piso despues de lanzarla
            }
        }
        else{
            if(m_PlayerThrow.m_Throwed && m_Rigidbody.velocity != Vector3.zero){//aqui compruebo el sript player trow, ya no es necesario hacerlo e el player manager, mejorar la velocidad de esto
                m_Fired = true;
            }
        }
    }

    public void Fire(Vector3 fuerza){
        if(!m_Fired){
            m_Rigidbody.AddForce(fuerza, ForceMode.Impulse);
            //m_Fired = true;// para esta apicacion que usa fuerza fisicas, seria coveniete esta variabe comprobar recien cuadno este en movimineto, no antes, es decir comprobar la canica si ya se movio
            //deberia descativar el script PlayerThrow, y activarse denuevo cuando se cree una nuva canica
        }
    }

    /*public void OnTriggerEnter(Collider other){//esto es para mejorar un poco el sistema de colisiones

    }*/
}
//basicamente es para que la canica siga la psicion del jugador cuand este se traslada, tambien podria hacer que controle la fuerza a tra vez de este script, y ya solo player throw, se encarga de transmitir la informacion necesaria
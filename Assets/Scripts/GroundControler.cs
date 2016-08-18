using UnityEngine;

public class GroundControler : MonoBehaviour {//para agregar desaceleracion
    public void OnTriggerEnter(Collider other){//cambiar esto a Stay hace que sea muy pesado
        GameObject canica = other.gameObject;
        if(canica.layer == LayerMask.NameToLayer("Jugador")){
            CanicaPlayer canicaPlayer = canica.GetComponent<CanicaPlayer>();
            if(canicaPlayer.m_Fired){
                //Rigidbody canicaRigidbody = canica.GetComponent<Rigidbody>();//no funciona llamar a addforce del rigid body por que esto no esta en un update
                canicaPlayer.m_Desaceleracion = 1f;
                //los objetivos tambien deben tener un script para poider agregar alguna desaceleracion
            }
        } 
        if(canica.layer == LayerMask.NameToLayer("Objetivo")){
            CanicaObjetivo canicaObjetivo = canica.GetComponent<CanicaObjetivo>();
            canicaObjetivo.m_Desaceleracion = 1f;
        }
    }
    public void OnTriggerExit(Collider other){//para devolver las desaceleraciones a su lugar
        GameObject canica = other.gameObject;
        if(canica.layer == LayerMask.NameToLayer("Jugador")){
            CanicaPlayer canicaPlayer = canica.GetComponent<CanicaPlayer>();
            if(canicaPlayer.m_Fired){
                canicaPlayer.m_Desaceleracion = 0f;
            }
        } 
        if(canica.layer == LayerMask.NameToLayer("Objetivo")){
            CanicaObjetivo canicaObjetivo = canica.GetComponent<CanicaObjetivo>();
            canicaObjetivo.m_Desaceleracion = 0f;
        }
    }
}
//otra alternativa es que esta calse tenga su funcion fixed update, y todos las canicas que entren se les aplicadesaceleracion en su moviemineto, el problema es qperderia control en la condicion para poner a 0 el movimiento
//o tal vez no
//cuadno sale una canica, si es de un jugador, se la debo entregar al jugador que la hya sacado, es decir, del que sea el turno, de esta fomra tenria que elminar la canica que el jugadro usara
//del array de canicas del jugadro, entonces todas las canicas qe salgan de la zona, que sean de tipo jugador, se que las agrego al array, del jugador, esto podria ser costo dependiendo de la funion, pero solo es leiminar de la lista, mas no destruccion del objeto,
//las unicas canicas que deben detruirse son las obejietivo

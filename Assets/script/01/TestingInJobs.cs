using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;

public class TestingInJobs : MonoBehaviour {

    [SerializeField] private bool useJobs;


    // Usando Jobs System (Multi-thread)

    // Mesmo usando Job System não há benefício
    // por que estamos fazendo uso de uma thread apenas
    private void Update () {

        float inicioTempo = Time.realtimeSinceStartup;

        if (useJobs) {

            // Job System call
            JobHandle jobHandle = TarefaPesadaTaskJob ();

            // Pausa a Thread principal até completar
            jobHandle.Complete ();

        }
        else {

            //Metodo tradicional
            TarefaPesada ();


        }



        Debug.Log (((Time.realtimeSinceStartup - inicioTempo) * 1000f) + "ms");
          

        
    }


    private void TarefaPesada () {

        float valor = 0f;
        for (int i = 0 ; i < 50000 ; i++) {
            valor = math.exp10 (math.sqrt (valor));
        }
    }

    private JobHandle TarefaPesadaTaskJob () {

        //Declara e Instancia o Job
        TarefaPesadaJob job = new TarefaPesadaJob ();

        //Agenda o Job. O agendamento retorna o job handle
        return job.Schedule ();


    }


    // Para maior velocidade na alocaçao da memória
    // o Job System usa strutct.
    // É necessário usar IJob interface

    public struct TarefaPesadaJob : IJob {

        public void Execute () {

            float valor = 0f;
            for (int i = 0 ; i < 50000 ; i++) {
                valor = math.exp10 (math.sqrt (valor));
            }

        }
    }

}

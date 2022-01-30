using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;


public class TestingInJobsWorks : MonoBehaviour {

    [SerializeField] private bool useJobs;


    // Usando Jobs System (Multi-thread)
    // Aqui vamos aumetar as tarefas de uma maneira
    // que o Jobs System moste mais a sua eficiencia
 
    private void Update () {

        float inicioTempo = Time.realtimeSinceStartup;

        if (useJobs) {

            // Aqui vamos especificar que todos as 10 operacoes
            // sejam lancadas ao mesmo tempo.
            // Se não, continuaremos usando sigle Thread

            NativeArray<JobHandle> jobHandlesList = new NativeArray<JobHandle> (10,Allocator.Temp);


            for (int i = 0 ; i < 10 ; i++) {
                // Job System call
                JobHandle jobHandle = TarefaPesadaTaskJob ();
                jobHandlesList[0] = jobHandle;
                
                
            }
            // Completa a lista de Jobs
            JobHandle.CompleteAll (jobHandlesList);

            //Depois de resolvido, limpa a memoria
            jobHandlesList.Dispose ();

        }
        else {
            for (int i = 0 ; i < 10 ; i++) {
                //Metodo tradicional
                TarefaPesada ();

            }
            


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

    [BurstCompile]
    public struct TarefaPesadaJob : IJob {

        public void Execute () {

            float valor = 0f;
            for (int i = 0 ; i < 50000 ; i++) {
                valor = math.exp10 (math.sqrt (valor));
            }

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;



public class TestingInJobsParallelFor : MonoBehaviour {

    [SerializeField] private bool useJobs;
    [SerializeField] private Transform pfZombie;
    private List<Zombie> zombieList;


    public class Zombie {


        public Transform transform;
        public float moveY;
    }

    private void Start () {
        zombieList = new List<Zombie> ();

        for (int i = 0 ; i < 10000 ; i++) {

            Transform zombieTranform = Instantiate (pfZombie,new Vector3 (UnityEngine.Random.RandomRange (-6f,6f),UnityEngine.Random.RandomRange (-4f,4f)),Quaternion.identity);
            zombieList.Add (new Zombie { transform = zombieTranform, moveY = UnityEngine.Random.Range(1f,2f)});
        }


    }

    private void Update () {

   
        if (useJobs) {

        
            NativeArray<float3> postionArr = new NativeArray<float3> (zombieList.Count,Allocator.TempJob);
            NativeArray<float> moveYArr = new NativeArray<float> (zombieList.Count,Allocator.TempJob);


            for (int i = 0 ; i < zombieList.Count; i++) {

                postionArr[i] = zombieList[i].transform.position;
                moveYArr[i] = zombieList[i].moveY;

            }


            ZombieParallelJob zombieParalleJob = new ZombieParallelJob {

                deltaTime = Time.deltaTime,
                positionArray = postionArr,
                moveYArray = moveYArr               
                

            };


            // PAssar a quantidade de Zombis e quantos Jobs vão dar conta dos 1000 Zombis
            JobHandle jobHandle = zombieParalleJob.Schedule (zombieList.Count,10);
            jobHandle.Complete ();

            //Depois que as posicoes foram calculadas pelas threas, é preciso passar para os objetos

            for (int i = 0 ; i < zombieList.Count ; i++) {

                zombieList[i].transform.position = postionArr[i];
                zombieList[i].moveY = moveYArr[i];

            }

            postionArr.Dispose ();
            moveYArr.Dispose ();
           

        }
        else {

            foreach (Zombie zombie in zombieList) {

                zombie.transform.position += new Vector3 (0,zombie.moveY * Time.deltaTime);
                if(zombie.transform.position.y > 5f) {

                    zombie.moveY = -math.abs (zombie.moveY);

                }
                if(zombie.transform.position.y < -5f) {
                    zombie.moveY = +math.abs (zombie.moveY);
                }

            }


        }



    }



   


    // IJobParallelFor Permite executar elementos dento da lista


    [BurstCompile (CompileSynchronously = true)]
    public struct ZombieParallelJob : IJobParallelFor {

        public NativeArray<float3> positionArray;
        public NativeArray<float> moveYArray;
        public float deltaTime;

        public void Execute (int index) {

            positionArray[index] += new float3 (0,moveYArray[index] * deltaTime,0f);

            if(positionArray[index].y > 5f) {

                moveYArray[index] = -math.abs (moveYArray[index]);
            }

            if(positionArray[index].y < -5f) {
                moveYArray[index] = +math.abs (moveYArray[index]);
            }


        }
    }

}

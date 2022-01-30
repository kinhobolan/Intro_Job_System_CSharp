using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Testing : MonoBehaviour {
   

    // Usando apenas uma thread
    private void Update() {

        float inicioTempo = Time.realtimeSinceStartup;

        TarefaPesada ();
        Debug.Log (((Time.realtimeSinceStartup - inicioTempo) * 1000f) + "ms");
    }

    private void TarefaPesada () {

        float valor = 0f;
        for (int i = 0 ; i < 50000 ; i++) {
            valor = math.exp10 (math.sqrt (valor));
        }
    }
}

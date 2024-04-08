using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Grunden til vi overhovedet har det her script er fordi vi er nødt til at bruge et interface til at handle at vi skal calle den samme metode fra talent scripts, mens scriptsne kommer til at have forskellige navne
public interface ITalentEffect
{
    void ApplyEffect(GameObject player);
}

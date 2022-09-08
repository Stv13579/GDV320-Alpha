using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ElementStats : MonoBehaviour
{
    public List<Multiplier> crystaldamageMultis = new List<Multiplier>();
    public List<Multiplier> fireDamageMultis = new List<Multiplier>();
    public List<Multiplier> waterDamageMultis = new List<Multiplier>();

    public float crystalDamageMultiplier = 1, fireDamageMultiplier = 1, waterDamageMultiplier = 1;
}

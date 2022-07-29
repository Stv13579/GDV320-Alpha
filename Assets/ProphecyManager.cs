using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProphecyManager : MonoBehaviour
{
    /// <summary>
    /// A list of the attached prophecy components.
    /// When adding a prophecy to the prophecy manager, remember to add it to this list as well.
    /// </summary>
    [SerializeField]
    public List<Prophecy> allProphecies;


}

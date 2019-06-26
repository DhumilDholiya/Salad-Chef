using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{
    //To store items on chopping board.
    [HideInInspector]public List<string> Container = new List<string>();
    [HideInInspector]public int currChopped = 0;
}

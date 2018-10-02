using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class UnitFactory : MonoBehaviour {
    public List<UnitType> CanBuild;
}

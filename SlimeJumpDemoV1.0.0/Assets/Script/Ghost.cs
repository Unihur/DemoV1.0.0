using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    void DestroyGhost()
    {
        Destroy(gameObject);
    }
}

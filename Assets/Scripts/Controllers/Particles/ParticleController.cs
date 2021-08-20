using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController :
    /// @cond SKIP_THIS
        MonoBehaviour
    /// @endcond
{
    private void FinishAnim()
    {
        Destroy(gameObject);
    }
}

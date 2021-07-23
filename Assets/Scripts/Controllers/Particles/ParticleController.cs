using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     <para>Clase utilizada por las particulas de un hit-attack del PJ</para>
/// </summary>
/// <remarks>
///     \emoji :clock4: Ultima actualizacion: v0.0.9 - 22/7/2021 - Nicolas Cabrera
/// </remarks>
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

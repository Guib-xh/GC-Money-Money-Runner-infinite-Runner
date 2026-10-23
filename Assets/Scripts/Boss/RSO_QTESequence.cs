using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    [CreateAssetMenu(fileName = "RSO_QTESequence", menuName = "RSO/Boss/RSO_QTESequence")]
    public class RSO_QTESequence : RuntimeScriptableObject<List<QTEKey>>
    {
    }
}
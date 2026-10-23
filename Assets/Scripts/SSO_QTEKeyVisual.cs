using System.Collections.Generic;
using Boss;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SSO_QTEKeyVisual", menuName="SSO/SSO_QTEKeyVisual")]
public class SSO_QTEKeyVisual: ScriptableObject
{
    [SerializeField] private Dictionary<QTEKey, Sprite> _values;

    public Sprite GetSprite(QTEKey key)
    {
        //Debug.Log("QTEKey " + key + " : " + _values[key].name);
        return _values[key];
    } 
}

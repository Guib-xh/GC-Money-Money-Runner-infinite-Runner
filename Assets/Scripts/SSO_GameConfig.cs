using UnityEngine;

[CreateAssetMenu(fileName = "SSO_GameConfig", menuName="SSO/SSO_GameConfig")]
public class SSO_GameConfig: ScriptableObject
{
    public int lineCount = 5;
    public float lineWidthLv = 1f;
    public float jumpHeight = 3f;
    public float jumpDuration = 1.2f;
    public float slideDuration = 1.2f;
    public float slideHeightScale = 0.5f;
}

using UnityEngine;

public struct QTEStageConfig
{
    public int stage;
    public int sequenceLength;
    public float keyPressWindowDuration;
}

[CreateAssetMenu(fileName = "SSO_BossConfig", menuName="SSO/Boss/SSO_BossConfig")]
public class SSO_BossConfig: ScriptableObject
{
    public float qteFailPauseSeconds = 2f;
    public int maxKeyPoolSize = 5;
    public QTEStageConfig[]  qteConfigs = {
        new QTEStageConfig{stage=1,sequenceLength = 3, keyPressWindowDuration = 1f},
        new QTEStageConfig{stage=2,sequenceLength = 3, keyPressWindowDuration = 1f},
        new QTEStageConfig{stage=3,sequenceLength = 4, keyPressWindowDuration = 0.8f},
        new QTEStageConfig{stage=4,sequenceLength = 4, keyPressWindowDuration = 0.8f},
        new QTEStageConfig{stage=5,sequenceLength = 5, keyPressWindowDuration = 0.7f},
        new QTEStageConfig{stage=6,sequenceLength = 5, keyPressWindowDuration = 0.7f},
        new QTEStageConfig{stage=7,sequenceLength = 6, keyPressWindowDuration = 0.6f},
        new QTEStageConfig{stage=8,sequenceLength = 6, keyPressWindowDuration = 0.6f},
        new QTEStageConfig{stage=9,sequenceLength = 7, keyPressWindowDuration = 0.5f},
        new QTEStageConfig{stage=10,sequenceLength = 7, keyPressWindowDuration = 0.5f},
        new QTEStageConfig{stage=11,sequenceLength = 8, keyPressWindowDuration = 0.4f},
    };

}

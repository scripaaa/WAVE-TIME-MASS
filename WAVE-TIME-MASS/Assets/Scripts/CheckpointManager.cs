using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Vector2? lastCheckpointPosition = null;
    private Checkpoint bossCheckpoint; // —сылка на чекпоинт босса

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector2 position)
    {
        lastCheckpointPosition = position;
    }

    public void SetBossCheckpoint(Checkpoint checkpoint)
    {
        bossCheckpoint = checkpoint;
    }

    public Vector2? GetLastCheckpoint()
    {
        return lastCheckpointPosition;
    }

    public bool HasCheckpoint()
    {
        return lastCheckpointPosition != null;
    }

    public bool IsLastCheckpointBoss()
    {
        return bossCheckpoint != null &&
               lastCheckpointPosition.HasValue &&
               (Vector2)bossCheckpoint.transform.position == lastCheckpointPosition.Value;
    }

    public void ResetCheckpoints()
    {
        lastCheckpointPosition = null;
        bossCheckpoint = null;
    }
}
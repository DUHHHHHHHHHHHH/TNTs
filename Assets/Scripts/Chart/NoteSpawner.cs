using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform spawnPoint;
    public Transform hitPositionTransform; // Aggiunto per linea di hit

    public float baseScrollSpeed = 5f;
    public float userScrollSpeed = 1f;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager non trovato nella scena!");
        }
    }

    public void SpawnAllNotes(List<int> noteTimes, List<Note.NoteType> noteTypes)
    {
        float effectiveSpeed = baseScrollSpeed * userScrollSpeed;

        for (int i = 0; i < noteTimes.Count; i++)
        {
            float distance = noteTimes[i] * effectiveSpeed * 0.001f; // ms -> s
            Vector3 pos = spawnPoint.position + Vector3.right * distance;

            GameObject noteObj = Instantiate(notePrefab, pos, Quaternion.identity);
            Note noteScript = noteObj.GetComponent<Note>();

            if (noteScript != null)
            {
                noteScript.noteType = noteTypes[i];
                noteScript.noteTime = noteTimes[i];
                noteScript.speed = effectiveSpeed;

                if (hitPositionTransform != null)
                {
                    noteScript.hitPositionX = hitPositionTransform.position.x;
                }
                else
                {
                    Debug.LogWarning("hitPositionTransform non assegnato, uso spawnPoint come fallback.");
                    noteScript.hitPositionX = spawnPoint.position.x;
                }

                if (gameManager != null)
                {
                    gameManager.RegisterNote(noteScript);
                }
                else
                {
                    Debug.LogWarning("GameManager non trovato, nota non registrata.");
                }
            }
        }
    }

    // Metodo per disegnare la linea di hit nel Scene View
    void OnDrawGizmos()
    {
        if (hitPositionTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(hitPositionTransform.position.x, -10f, 0f),
                new Vector3(hitPositionTransform.position.x, 10f, 0f)
            );
        }
    }
}

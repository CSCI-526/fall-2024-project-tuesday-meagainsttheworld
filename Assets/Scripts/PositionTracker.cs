using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    private struct PlayerTracker
    {
        public PlayerTracker(GameObject inputPlayer)
        {
            transform = inputPlayer.transform;
            name = inputPlayer.name;
            lastPosition = Vector3.one * 100;
            waitTime = 0;
        }

        public Transform transform;
        public string name;
        public Vector3 lastPosition;
        public float waitTime;
    }

    private PlayerTracker player1Tracker;
    private PlayerTracker player2Tracker;
    private readonly float waitLimit = 5;

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players[0].name == "Player1")
        {
            player1Tracker = new PlayerTracker(players[0]);
            player2Tracker = new PlayerTracker(players[1]);
        }
        else
        {
            player1Tracker = new PlayerTracker(players[1]);
            player2Tracker = new PlayerTracker(players[0]);
        }
    }

    void Update()
    {
        // Check Player 1's position
        if (player1Tracker.transform.position == player1Tracker.lastPosition) player1Tracker.waitTime += Time.deltaTime;
        else
        {
            if (player1Tracker.waitTime > waitLimit) TrackWaitTime(player1Tracker);
            player1Tracker.waitTime = 0;
            player1Tracker.lastPosition = player1Tracker.transform.position;
        }

        // Check Player 2's position
        if (player2Tracker.transform.position == player2Tracker.lastPosition) player2Tracker.waitTime += Time.deltaTime;
        else
        {
            if (player2Tracker.waitTime > waitLimit) TrackWaitTime(player2Tracker);
            player2Tracker.waitTime = 0;
            player2Tracker.lastPosition = player2Tracker.transform.position;
        }
    }

    void OnDestroy()
    {
        if (player1Tracker.waitTime > waitLimit) TrackWaitTime(player1Tracker);
        if (player2Tracker.waitTime > waitLimit) TrackWaitTime(player2Tracker);
    }

    private void TrackWaitTime(PlayerTracker inputTracker)
    {
        Debug.Log(inputTracker.name + " waited at position " + inputTracker.lastPosition + " for " + inputTracker.waitTime + " seconds");
    }
}
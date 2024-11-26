using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PillInteraction : MonoBehaviour
{
    public RectTransform player1;
    public RectTransform player2;
    public Image pill1;
    public Image pill2;
    public float effectDuration = 5f;
    public float sizeMultiplier = 2f;
    public float moveSpeed = 100f;
    public float blinkInterval = 0.1f;
    public float extraMoveDistance = 50f; // Distance to move past the pill

    private Vector3 originalScalePlayer1;
    private Vector3 originalScalePlayer2;
    private Vector3 originalPosPlayer1;
    private Vector3 originalPosPlayer2;
    private Color originalColorPill1;
    private Color originalColorPill2;
    private Color grayColor = Color.gray;

    private void Start()
    {
        originalScalePlayer1 = player1.localScale;
        originalScalePlayer2 = player2.localScale;
        originalPosPlayer1 = player1.position;
        originalPosPlayer2 = player2.position;
        originalColorPill1 = pill1.color;
        originalColorPill2 = pill2.color;
        StartCoroutine(InteractionSequence());
    }

    private IEnumerator InteractionSequence()
    {
        yield return MovePlayerToPillAndGrow(player1, pill1.rectTransform, true);
        yield return new WaitForSeconds(effectDuration);
        yield return BlinkPlayers();
        ResetPlayerSizes();
        SetPillActive(pill1);

        yield return MovePlayerToPillAndGrow(player2, pill2.rectTransform, false);
        yield return new WaitForSeconds(effectDuration);
        yield return BlinkPlayers();
        ResetPlayerSizes();
        SetPillActive(pill2);
    }

    private IEnumerator MovePlayerToPillAndGrow(RectTransform player, RectTransform pill, bool isGrowing)
    {
        Vector3 originalPos = player.position;

        // Move to the pill
        while (Vector3.Distance(player.position, pill.position) > 1f)
        {
            player.position = Vector3.MoveTowards(player.position, pill.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Player has reached the pill
        SetPillInactive(pill.GetComponent<Image>());

        // Instant scaling
        if (isGrowing)
        {
            player.localScale = originalScalePlayer1 * sizeMultiplier;
            AdjustVerticalPosition(player, originalPos, true);
            player2.localScale = originalScalePlayer2 / sizeMultiplier; // Shrink other player
            AdjustVerticalPosition(player2, originalPosPlayer2, false);
        }
        else
        {
            player.localScale = originalScalePlayer2 / sizeMultiplier;
            AdjustVerticalPosition(player, originalPos, false);
            player1.localScale = originalScalePlayer1 * sizeMultiplier; // Grow other player
            AdjustVerticalPosition(player1, originalPosPlayer1, true);
        }

        // Continue moving past the pill
        Vector3 extraMovePosition = player.position + Vector3.right * extraMoveDistance;
        while (Vector3.Distance(player.position, extraMovePosition) > 1f)
        {
            player.position = Vector3.MoveTowards(player.position, extraMovePosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void AdjustVerticalPosition(RectTransform player, Vector3 originalPosition, bool isGrowing)
    {
        float originalHeight = player.rect.height * originalScalePlayer1.y;
        float newHeight = player.rect.height * player.localScale.y;
        float heightDifference = newHeight - originalHeight;

        if (isGrowing)
        {
            player.position = new Vector3(player.position.x, originalPosition.y + heightDifference / 2, player.position.z);
        }
        else
        {
            player.position = new Vector3(player.position.x, originalPosition.y - heightDifference / 2, player.position.z);
        }
    }

    private IEnumerator BlinkPlayers()
    {
        IEnumerator blinkPlayer1 = BlinkPlayer(player1.GetComponent<Image>());
        IEnumerator blinkPlayer2 = BlinkPlayer(player2.GetComponent<Image>());
        
        yield return StartCoroutine(RunParallelCoroutines(blinkPlayer1, blinkPlayer2));
    }

    private IEnumerator BlinkPlayer(Image playerImage)
    {
        if (playerImage == null) yield break;

        Color originalColor = playerImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            playerImage.color = (playerImage.color == originalColor) ? grayColor : originalColor;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        playerImage.color = originalColor;
    }

    private IEnumerator RunParallelCoroutines(IEnumerator coroutine1, IEnumerator coroutine2)
    {
        bool coroutine1Finished = false;
        bool coroutine2Finished = false;

        StartCoroutine(RunCoroutineAndSignalCompletion(coroutine1, () => coroutine1Finished = true));
        StartCoroutine(RunCoroutineAndSignalCompletion(coroutine2, () => coroutine2Finished = true));

        yield return new WaitUntil(() => coroutine1Finished && coroutine2Finished);
    }

    private IEnumerator RunCoroutineAndSignalCompletion(IEnumerator coroutine, System.Action onComplete)
    {
        yield return StartCoroutine(coroutine);
        onComplete?.Invoke();
    }

    private void ResetPlayerSizes()
    {
       // Reset both players to their original scales and positions
       player1.localScale = originalScalePlayer1;
       AdjustVerticalPosition(player1, originalPosPlayer1, false);

       player2.localScale = originalScalePlayer2;
       AdjustVerticalPosition(player2, originalPosPlayer2, false);
   }

   private void SetPillInactive(Image pill)
   {
       pill.color = grayColor;
   }

   private void SetPillActive(Image pill)
   {
       pill.color = (pill == pill1) ? originalColorPill1 : originalColorPill2; 
   }
}
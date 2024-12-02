using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerupAnimation : MonoBehaviour
{
    public RectTransform player1;
    public RectTransform player2;
    public Image pill1;
    public Image pill2;
    public GameObject powerupAnimationPanel;
    public Image whiteBackground;
    public Image blackBackground;
    public Button closeButton;
    public float effectDuration = 5f;
    public float sizeMultiplier = 2f;
    public float moveSpeed = 100f;
    public float blinkInterval = 0.1f;
    public float extraMoveDistance = 50f;
    public float panelZoomDuration = 1f;

    private Vector3 originalScalePlayer1;
    private Vector3 originalScalePlayer2;
    private Vector3 originalPosPlayer1;
    private Vector3 originalPosPlayer2;
    private Color originalColorPill1;
    private Color originalColorPill2;
    private Color grayColor = Color.gray;
    private bool isAnimationPlaying = false;
    public static bool hasAnimationPlayed = false;

    private void Awake()
    {
        originalScalePlayer1 = player1.localScale;
        originalScalePlayer2 = player2.localScale;
        originalPosPlayer1 = player1.anchoredPosition;
        originalPosPlayer2 = player2.anchoredPosition;
        originalColorPill1 = pill1.color;
        originalColorPill2 = pill2.color;

        powerupAnimationPanel.SetActive(false);
        SetVisibility(false);
        closeButton.gameObject.SetActive(false);
        closeButton.onClick.AddListener(CloseAnimation);
    }

    public void StartAnimation()
    {
        if (!isAnimationPlaying && !hasAnimationPlayed)
        {
            StartCoroutine(AnimationSequence());
        }
    }

    private IEnumerator AnimationSequence()
    {
        isAnimationPlaying = true;
        hasAnimationPlayed = true;
        Time.timeScale = 0f; // Freeze gameplay

        powerupAnimationPanel.SetActive(true);
        closeButton.gameObject.SetActive(true);
        yield return StartCoroutine(ZoomInPanel());

        SetVisibility(true);
        yield return null;

        yield return StartCoroutine(InteractionSequence());

        // Wait for close button to be clicked
        yield return new WaitUntil(() => !isAnimationPlaying);
    }

    private void CloseAnimation()
    {
        StopAllCoroutines();
        powerupAnimationPanel.SetActive(false);
        closeButton.gameObject.SetActive(false);
        SetVisibility(false);
        Time.timeScale = 1f; // Unfreeze gameplay
        isAnimationPlaying = false;
    }

    private IEnumerator ZoomInPanel()
    {
        RectTransform panelTransform = powerupAnimationPanel.GetComponent<RectTransform>();
        Vector3 startScale = Vector3.one * 0.1f;
        Vector3 endScale = Vector3.one;

        panelTransform.localScale = startScale;

        float elapsed = 0f;
        while (elapsed < panelZoomDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / panelZoomDuration;
            panelTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        panelTransform.localScale = endScale;
    }

    private void SetVisibility(bool isVisible)
    {
        player1.gameObject.SetActive(isVisible);
        player2.gameObject.SetActive(isVisible);
        pill1.gameObject.SetActive(isVisible);
        pill2.gameObject.SetActive(isVisible);
    }


    private IEnumerator InteractionSequence()
    {
        yield return MovePlayerToPillAndGrow(player1, pill1.rectTransform, true);
        yield return new WaitForSecondsRealtime(4f);
        yield return BlinkPlayers();
        ResetPlayerSizesAndPills();
        yield return new WaitForSecondsRealtime(1f);

        yield return MovePlayerToPillAndGrow(player2, pill2.rectTransform, false);
        yield return new WaitForSecondsRealtime(4f);
        yield return BlinkPlayers();
        ResetPlayerSizesAndPills();
    }

    private IEnumerator MovePlayerToPillAndGrow(RectTransform player, RectTransform pill, bool isGrowing)
    {
        Vector2 originalPos = player.anchoredPosition;
        Vector2 pillPos = pill.anchoredPosition;

        while (Vector2.Distance(player.anchoredPosition, pillPos) > 1f)
        {
            player.anchoredPosition = Vector2.MoveTowards(player.anchoredPosition, pillPos, moveSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        SetPillInactive(pill.GetComponent<Image>());

        if (isGrowing)
        {
            player.localScale = originalScalePlayer1 * sizeMultiplier;
            AdjustVerticalPosition(player, originalPos, true);

            player2.localScale = originalScalePlayer2 / sizeMultiplier;
            AdjustVerticalPosition(player2, originalPosPlayer2, false);
        }
        else
        {
            player1.localScale = originalScalePlayer1 * sizeMultiplier;
            AdjustVerticalPosition(player1, originalPosPlayer1, true);

            player.localScale = originalScalePlayer2 / sizeMultiplier;
            AdjustVerticalPosition(player, originalPos, false);
        }

        Vector2 extraMovePosition = player.anchoredPosition + Vector2.right * extraMoveDistance;
        while (Vector2.Distance(player.anchoredPosition, extraMovePosition) > 1f)
        {
            player.anchoredPosition = Vector2.MoveTowards(player.anchoredPosition, extraMovePosition, moveSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
    }

    private void AdjustVerticalPosition(RectTransform player, Vector2 originalPosition, bool isGrowing)
    {
        float originalHeight = player.rect.height * originalScalePlayer1.y;
        float newHeight = player.rect.height * player.localScale.y;
        float heightDifference = newHeight - originalHeight;

        if (isGrowing)
        {
            player.anchoredPosition = new Vector2(player.anchoredPosition.x, originalPosition.y + heightDifference / 2);
        }
        else
        {
            player.anchoredPosition = new Vector2(player.anchoredPosition.x, originalPosition.y - heightDifference / 2);
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
            yield return new WaitForSecondsRealtime(blinkInterval);
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

    private void ResetPlayerSizesAndPills()
    {
        player1.localScale = originalScalePlayer1;
        AdjustVerticalPosition(player1, originalPosPlayer1, false);

        player2.localScale = originalScalePlayer2;
        AdjustVerticalPosition(player2, originalPosPlayer2, false);

        SetPillActive(pill1);
        SetPillActive(pill2);
    }

    private void SetPillInactive(Image pill)
    {
        pill.color = grayColor;
    }

    private void SetPillActive(Image pill)
    {
        pill.color = (pill == pill1) ? originalColorPill1 : originalColorPill2;
    }
    public static void ResetAnimationFlag()
    {
        hasAnimationPlayed = false;
    }
}


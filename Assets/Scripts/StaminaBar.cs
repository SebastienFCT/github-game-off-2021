using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private int maxStamina = 100;
    private int currentStamina;
    private bool isShaking = false;
    public float shakeDuration = 1f;

    public static StaminaBar instance;
    public GameEngine gameEngine;
    private Slider slider;

    private GameObject SliderFill;
    private Color greenFill = new Color32(113, 255, 32, 100);

    private IEnumerator coroutine;

    public GameObject uiBar;
    public GameObject exhaustedBanner;
    Vector2 targetPosition;

    private void Awake()
    {
        instance = this;
        slider = instance.GetComponent<Slider>();
    }

    void Start()
    {
        targetPosition = new Vector2(0,0);
        currentStamina = 0;
        slider.maxValue = maxStamina;
        slider.value = 0;
        SliderFill = slider.transform.GetChild(1).GetChild(0).gameObject;
        SliderFill.GetComponent<Image>().color = greenFill;

        coroutine = DecreaseStamina();
        StartCoroutine(coroutine);
    }

    public void UseStamina(int amount)
    {
        if (!gameEngine.IsPlaying())
        {
            return;
        }
        if (gameEngine.IsExhausted())
        {

            if (!isShaking) { StartCoroutine(ShakeCamera()); }

            return;
        }

        currentStamina += amount;

        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
            gameEngine.SetExaustion(true);

            Instantiate(exhaustedBanner, targetPosition, Quaternion.identity);

            SliderFill.GetComponent<Image>().color = Color.red;
        }
    }

    private void FixedUpdate()
    {
        slider.value = currentStamina;
    }

    IEnumerator DecreaseStamina()
    {
        while (true)
        {
            if (gameEngine.IsExhausted())
            {
                yield return new WaitForSeconds(0.06f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }

            currentStamina = Mathf.Max(currentStamina - 1, 0);

            if (currentStamina == 30)
            {
                gameEngine.SetExaustion(false);
                SliderFill.GetComponent<Image>().color = greenFill;
            }
        }
    }

    IEnumerator ShakeCamera()
    {
        isShaking = true;
        Transform cameraTransform = uiBar.transform;
        Vector3 startPosition = cameraTransform.position;
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration && gameEngine.IsPlaying())
        {
            elapsedTime += Time.deltaTime;
            cameraTransform.position = startPosition + (Random.insideUnitSphere / 3);
            yield return null;
        }
        cameraTransform.position = startPosition;
        isShaking = false;
    }

}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Necesario para postprocesado URP

public class Sanity : MonoBehaviour
{
    public Slider sanitySlider;        // Referencia al slider del HUD
    public TextMeshProUGUI sanityText; // Referencia al texto que muestra la cordura
    public Volume postProcessingVolume; // Referencia al Volume del postprocesado

    private int _maxSanity = 1000;     // Valor máximo de cordura
    public int currentSanity;         // Cordura actual

    private Vignette vignette;        // Referencia al efecto Vignette
    private FilmGrain filmGrain;      // Referencia al efecto FilmGrain
    private LiftGammaGain liftGammaGain; // Referencia al efecto LiftGammaGain
    private DepthOfField depthOfField;  // Referencia al efecto Depth of Field
    private MotionBlur motionBlur;      // Referencia al efecto Motion Blur

    private float vignetteMinIntensity = 0.3f; // Intensidad mínima del Vignette
    private float vignetteMaxIntensity = 0.6f; // Intensidad máxima del Vignette
    private float vignetteMinSmoothness = 0.3f; // Suavidad mínima del Vignette
    private float vignetteMaxSmoothness = 0.6f; // Suavidad máxima del Vignette

    private float grainMinIntensity = 0.3f; // Intensidad mínima del Grain
    private float grainMaxIntensity = 1.0f; // Intensidad máxima del Grain

    private float liftGammaMinValue = 0.0f; // Valor mínimo de gamma
    private float liftGammaMaxValue = 1.0f; // Valor máximo de gamma

    private float focalLengthMin = 1.0f;    // Focal length mínimo
    private float focalLengthMax = 40.0f;   // Focal length máximo

    private float motionBlurMinIntensity = 0.0f; // Intensidad mínima del Motion Blur
    private float motionBlurMaxIntensity = 1.0f; // Intensidad máxima del Motion Blur

    void Start()
    {
        // Inicializar la cordura al máximo
        currentSanity = _maxSanity;

        // Buscar efectos Vignette, FilmGrain, LiftGammaGain, DepthOfField y MotionBlur en el Volume
        if (postProcessingVolume != null)
        {
            if (!postProcessingVolume.profile.TryGet(out vignette))
            {
                Debug.LogWarning("Vignette no encontrado en el Volume de postprocesado.");
            }

            if (!postProcessingVolume.profile.TryGet(out filmGrain))
            {
                Debug.LogWarning("FilmGrain no encontrado en el Volume de postprocesado.");
            }

            if (!postProcessingVolume.profile.TryGet(out liftGammaGain))
            {
                Debug.LogWarning("LiftGammaGain no encontrado en el Volume de postprocesado.");
            }

            if (!postProcessingVolume.profile.TryGet(out depthOfField))
            {
                Debug.LogWarning("DepthOfField no encontrado en el Volume de postprocesado.");
            }

            if (!postProcessingVolume.profile.TryGet(out motionBlur))
            {
                Debug.LogWarning("MotionBlur no encontrado en el Volume de postprocesado.");
            }

            // Establecer valores iniciales del Vignette
            if (vignette != null)
            {
                vignette.intensity.value = vignetteMinIntensity;
                vignette.smoothness.value = vignetteMinSmoothness;
            }

            // Establecer valores iniciales del FilmGrain
            if (filmGrain != null)
            {
                filmGrain.intensity.value = grainMinIntensity;
            }

            // Establecer valores iniciales del LiftGammaGain
            if (liftGammaGain != null)
            {
                liftGammaGain.gamma.value = new Vector4(liftGammaMinValue, liftGammaMinValue, liftGammaMinValue, liftGammaMinValue);
            }

            // Establecer valores iniciales del Depth of Field
            if (depthOfField != null)
            {
                depthOfField.focalLength.value = focalLengthMax; // Cordura alta, focal length máximo
            }

            // Establecer valores iniciales del Motion Blur
            if (motionBlur != null)
            {
                motionBlur.intensity.value = motionBlurMinIntensity; // Cordura alta, motion blur mínimo
            }
        }
    }

    void Update()
    {
        UpdateSanityHUD();
        UpdateVignetteEffect();
        UpdateFilmGrainEffect();
        UpdateLiftGammaGainEffect();
        UpdateDepthOfFieldEffect();
        UpdateMotionBlurEffect(); // Agregar lógica para Motion Blur
    }

    // Método para aumentar la cordura
    public void IncreaseSanity(int amount)
    {
        if (currentSanity < _maxSanity)
        {
            currentSanity += amount;
            currentSanity = Mathf.Clamp(currentSanity, 0, _maxSanity);
            UpdateSanityHUD();
        }
    }

    // Método para reducir la cordura
    public void ReduceSanity(int amount)
    {
        if (currentSanity > 0)
        {
            currentSanity -= amount;
            currentSanity = Mathf.Clamp(currentSanity, 0, _maxSanity);
            UpdateSanityHUD();
        }
    }

    // Método para actualizar el HUD
    private void UpdateSanityHUD()
    {
        // Controla que no pueda ser menor a 0 o mayor a maxSanity
        currentSanity = Mathf.Clamp(currentSanity, 0, _maxSanity);

        // Actualiza Slider
        if (sanitySlider != null)
        {
            sanitySlider.value = (float)currentSanity / _maxSanity;
        }

        // Actualiza el número
        if (sanityText != null)
        {
            sanityText.text = currentSanity.ToString();
        }
    }

    // Método para actualizar el efecto Vignette
    private void UpdateVignetteEffect()
    {
        if (vignette != null)
        {
            float sanityRatio = (float)currentSanity / _maxSanity;

            // Interpolar valores en función de la cordura
            vignette.intensity.value = Mathf.Lerp(vignetteMaxIntensity, vignetteMinIntensity, sanityRatio);
            vignette.smoothness.value = Mathf.Lerp(vignetteMaxSmoothness, vignetteMinSmoothness, sanityRatio);
        }
    }

    // Método para actualizar el efecto FilmGrain
    private void UpdateFilmGrainEffect()
    {
        if (filmGrain != null)
        {
            float sanityRatio = (float)currentSanity / _maxSanity;

            // Interpolar valores en función de la cordura
            filmGrain.intensity.value = Mathf.Lerp(grainMaxIntensity, grainMinIntensity, sanityRatio);
        }
    }

    // Método para actualizar el efecto LiftGammaGain
    private void UpdateLiftGammaGainEffect()
    {
        if (liftGammaGain != null)
        {
            // Obtener el valor de gamma completo
            var gammaValue = liftGammaGain.gamma.value;
            float sanityRatio = (float)currentSanity / _maxSanity;

            if (currentSanity < _maxSanity * 0.5f) // Si la cordura está por debajo del 50%
            {
                // Interpolamos el gamma a medida que baja la cordura
                gammaValue.x = Mathf.Lerp(liftGammaMaxValue, liftGammaMinValue, sanityRatio * 2);
            }
            else
            {
                gammaValue.x = liftGammaMinValue; // Restablecemos el valor si la cordura es alta
            }

            liftGammaGain.gamma.value = gammaValue;
        }
    }

    // Método para actualizar el efecto Depth of Field
    private void UpdateDepthOfFieldEffect()
    {
        if (depthOfField != null)
        {
            float sanityRatio = (float)currentSanity / _maxSanity;

            if (currentSanity < _maxSanity * 0.5f) // Si la cordura está por debajo del 50%
            {
                // Interpolar el focal length entre el mínimo y el máximo
                depthOfField.focalLength.value = Mathf.Lerp(focalLengthMax, focalLengthMin, sanityRatio * 2); 
            }
            else
            {
                // Restablecer el focal length al máximo si la cordura es alta
                depthOfField.focalLength.value = focalLengthMin;
            }
        }
    }

    // Método para actualizar el efecto Motion Blur
    private void UpdateMotionBlurEffect()
    {
        if (motionBlur != null)
        {
            float sanityRatio = (float)currentSanity / _maxSanity;

            if (currentSanity < _maxSanity * 0.65f) // Si la cordura está por debajo del 65%
            {
                // Interpolar la intensidad del Motion Blur
                motionBlur.intensity.value = Mathf.Lerp(motionBlurMaxIntensity, motionBlurMinIntensity, sanityRatio / 0.65f);
            }
            else
            {
                // Restablecer la intensidad al mínimo si la cordura es alta
                motionBlur.intensity.value = motionBlurMinIntensity;
            }
        }
    }
}
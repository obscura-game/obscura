using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Necesario para postprocesado URP

public class Sanity : MonoBehaviour
{
    // --AJUSTES DE CORDURA--
    [Header("Sanity Settings")]
    [Range(0, 100)]
    private int _maxSanity = 1000;     // Valor máximo de cordura
    public int currentSanity;         // Cordura actual, está en público para poder editarlo desde el inspector
    
    // --HUD--
    [Header("UI Components")] 
    public Image skullImage;
    public Image sanityFillImage;
    public Material backgroundMaterial;
    public Sprite[] skullSprites;
    
    // --SONIDOS--
    public AudioSource breathingAudioSource;
    public AudioSource heartbeatAudioSource;
    public AudioSource jumpscareAudioSource1;
    public AudioSource jumpscareAudioSource2;
    
    // Breathing
    private float breathingMinVolume = 0.005f;
    private float breathingMaxVolume = 1f;
    
    // Heartbeat
    private float heartbeatMinVolume = 0f;
    private float heartbeatMaxVolume = 0.8f;
    
    // --EFECTO SHAKE--
    private CameraFollow cameraShake; // Referencia al script CameraShake

    // --EFECTOS DE POSTPROCESADO--
    public Volume postProcessingVolume; // Referencia al Volume del postprocesado
    
    private Vignette vignette;
    private FilmGrain filmGrain;
    private LiftGammaGain liftGammaGain;
    private DepthOfField depthOfField;
    private MotionBlur motionBlur;
    
    // Vignette
    private float vignetteMinIntensity = 0.3f; // Siempre hay al menos un poco de Vignette activo
    private float vignetteMaxIntensity = 0.6f; 
    private float vignetteMinSmoothness = 0.3f; 
    private float vignetteMaxSmoothness = 0.6f; 

    // Grain
    private float grainMinIntensity = 0.3f; // Siempre hay efecto de grano activo (como con el Vignette)
    private float grainMaxIntensity = 1.0f; 

    // Lift Gamma Gain (efecto color rojo)
    private float liftGammaMinValue = 0.0f; // Valor mínimo de gamma
    private float liftGammaMaxValue = 1.0f; // Valor máximo de gamma

    // Depth of Field (efecto borroso)
    private float focalLengthMin = 1.0f;    // Focal length normal
    private float focalLengthMax = 40.0f;   // Focal length máximo (es el tope del efecto)

    // Motion blur (efecto al moverse)
    private float motionBlurMinIntensity = 0.0f;
    private float motionBlurMaxIntensity = 1.0f;

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
        
        cameraShake = FindObjectOfType<CameraFollow>();
    }

    void Update()
    {
        UpdateSanityHUD();
        
        // --Efectos de postprocesado--
        UpdateVignetteEffect();
        UpdateFilmGrainEffect();
        UpdateLiftGammaGainEffect();
        UpdateDepthOfFieldEffect();
        UpdateMotionBlurEffect();
        
        // --Efecto shake-
        UpdateShakeEffect();
        
        // --Efectos de sonido--
        UpdateBreathingSound();
        UpdateHeartbeatSound();
    }

    // --METODOS PARA MANIPULAR LA CORDURA--
    
    // Método para aumentar la cordura
    public void IncreaseSanity(int amount)
    {
        if (currentSanity < _maxSanity)
        {
            currentSanity += amount;
            UpdateSanityHUD();
        }
    }

    // Método para reducir la cordura
    public void ReduceSanity(int amount)
    {
        if (currentSanity > 0)
        {
            currentSanity -= amount;
            UpdateSanityHUD();
        }
    }

    public void Reduce200()
    {
        // Reproducir el sonido y reducir inicialmente la cordura
        jumpscareAudioSource1.Play();
        ReduceSanity(200);
        ReduceSanity(500);
    
        // Iniciar la coroutine para esperar y restaurar la cordura
        StartCoroutine(RestoreSanityAfterDelay(5f, 500, 0.5f));
    }
    
    private IEnumerator RestoreSanityAfterDelay(float restoreDuration, int sanityToRestore, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    
        // Restaurar gradualmente la cordura perdida
        float elapsedTime = 0f;
        
        while (elapsedTime < restoreDuration)
        {
            float sanityChunk = (sanityToRestore / restoreDuration) * Time.deltaTime;
            IncreaseSanity((int)sanityChunk);
        
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    
        // Asegurar que se restaure toda la cordura exactamente
        IncreaseSanity(sanityToRestore - (int)(elapsedTime * (sanityToRestore / restoreDuration)));
    }
    
    // --HUD--

    // Método para actualizar el HUD
    private void UpdateSanityHUD()
    {
        // Controla que no pueda ser menor a 0 o mayor a maxSanity.
        // Nota: Esto realmente es solo para cuando se modifica desde el inspector para las pruebas, 
        // porque al modificarlo desde los métodos ya está controlado que no se pase del mínimo ni del máximo.
        currentSanity = Mathf.Clamp(currentSanity, 0, _maxSanity);

        // Actualiza el fillAmount del Sanitymeter en el componente Image, es decir, la cantidad de relleno
        if (sanityFillImage != null)
        {
            float fillAmount = (float)currentSanity / _maxSanity;
            sanityFillImage.fillAmount = fillAmount;
        }

        // Actualiza el color del fondo del Sanitymeter
        if (backgroundMaterial != null)
        {
            float fillAmount = (float)currentSanity / _maxSanity;
            backgroundMaterial.SetFloat("_FillAmount", fillAmount);
            
        }

        // Esto es lo del shader de las olas (que no va)
        if (currentSanity < 600)
        {
            backgroundMaterial.SetInt("_olas", 1);
        }


        // Actualiza la imagen del cráneo
        if (skullImage != null && skullSprites.Length > 0)
        {
            if (currentSanity > 600)
                skullImage.sprite = skullSprites[0];
            else if (currentSanity > 400)
                skullImage.sprite = skullSprites[1];
            else if (currentSanity > 200)
                skullImage.sprite = skullSprites[2];
            else
                skullImage.sprite = skullSprites[3];
        }
        
    }

    // --METODOS PARA ACTUALIZAR LOS EFECTOS--
    
    // -SONIDOS-
    
    // Método para actualizar el sonido de respiración
    private void UpdateBreathingSound()
    {
        float sanityRatio = currentSanity / (float)_maxSanity;

        if (currentSanity < _maxSanity * 0.4f) // Cuando la cordura baje del 40%
        {
            breathingAudioSource.volume = Mathf.Lerp(breathingMaxVolume, 0.1f, sanityRatio);
            heartbeatAudioSource.volume = Mathf.Lerp(breathingMaxVolume, 0.1f, sanityRatio);
        } else if (currentSanity < _maxSanity * 0.9f)// Cuando la cordura baje del 90%
        {
            breathingAudioSource.volume = Mathf.Lerp(0.1f, breathingMinVolume, sanityRatio);
            heartbeatAudioSource.volume = Mathf.Lerp(0.1f, breathingMinVolume, sanityRatio);
        }
        else
        {
            breathingAudioSource.volume = breathingMinVolume;
            heartbeatAudioSource.volume = breathingMinVolume;
        }
    }
    
    // Método para actualizar el sonido de latidos
    private void UpdateHeartbeatSound()
    {
        float sanityRatio = currentSanity / (float)_maxSanity;
        
        if (currentSanity < _maxSanity * 0.4f) // Cuando la cordura baje del 40%
        {
            heartbeatAudioSource.volume = Mathf.Lerp(heartbeatMaxVolume, 0.08f, sanityRatio);
        } else if (currentSanity < _maxSanity * 0.9f)// Cuando la cordura baje del 90%
        {
            heartbeatAudioSource.volume = Mathf.Lerp(0.08f, heartbeatMinVolume, sanityRatio);
        }
        else
        {
            heartbeatAudioSource.volume = heartbeatMinVolume;
        }
    }
    
    // -VISUALES-
    
    // Método para actualizar el efecto Shake
    private void UpdateShakeEffect()
    {
        float sanityRatio = currentSanity / _maxSanity; // Relación actual de sanity con respecto al máximo

        if (currentSanity < _maxSanity * 0.2f && cameraShake != null)
        {
            float shakeMagnitude = Mathf.Lerp(0.2f, 0.02f, sanityRatio); // De 0.2 a 0.02 según la sanity
            cameraShake.TriggerShake(0.5f, shakeMagnitude); // Duración fija, magnitud variable
        }
        else if (currentSanity < _maxSanity * 0.4f)
        {
            float shakeMagnitude = Mathf.Lerp(0.02f, 0f, sanityRatio); // De 0.05 a 0 según la sanity
            cameraShake.TriggerShake(0.5f, shakeMagnitude); // Duración fija, magnitud decreciente
        }
    }
    
    // -POSTPROCESADO-

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
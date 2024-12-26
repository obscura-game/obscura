using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatientCard : MonoBehaviour
{
    // Referencias a los componentes de la UI
    public TextMeshProUGUI nombreText;
    public TextMeshProUGUI fechaNacimientoText;
    public TextMeshProUGUI alturaText;
    public TextMeshProUGUI pesoText;
    public TextMeshProUGUI colorOjosText;
    public TextMeshProUGUI diagnosticosText;
    public TextMeshProUGUI descripcionText;
    public Image fotoImage;  // Imagen para mostrar la foto del paciente

    // Referencia al ScriptableObject con los datos del paciente
    public PatientData patientData;

    void Start()
    {
        // Llamamos al método para actualizar la UI con los datos del paciente
        UpdatePatientInfo();
    }

    // Método que actualiza los valores de la UI con los datos del ScriptableObject
    void UpdatePatientInfo()
    {
        if (patientData != null)
        {
            nombreText.text =  patientData.nombre;
            fechaNacimientoText.text = patientData.fechaNacimiento;
            alturaText.text = patientData.altura.ToString("F2") + " cm";
            pesoText.text = patientData.peso.ToString("F2") + " kg";
            colorOjosText.text =  patientData.colorOjos;
            diagnosticosText.text =  patientData.diagnosticos;
            descripcionText.text = patientData.descripcion;

            // Asignar la foto del paciente al componente Image
            if (fotoImage != null && patientData.foto != null)
            {
                fotoImage.sprite = patientData.foto;
            }
        }
    }
}

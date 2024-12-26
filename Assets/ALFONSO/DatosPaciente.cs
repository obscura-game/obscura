using UnityEngine;

[CreateAssetMenu(fileName = "NewPatient", menuName = "Patient Data")]
public class PatientData : ScriptableObject
{
    public string nombre;
    public string fechaNacimiento;
    public float altura;
    public float peso;
    public string colorOjos;
    public string diagnosticos;
    public string descripcion;
    public Sprite foto; // Foto del paciente
}
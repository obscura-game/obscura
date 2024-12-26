using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPaciente", menuName = "Datos Ficha Paciente")]
public class PatientData : ScriptableObject
{
    public string nombre;
    public string fechaNacimiento;
    public float altura;
    public float peso;
    public string colorOjos;
    public string diagnosticos;
    public string descripcion;
    public string fechaUltimaRevision;
    public string doctorEncargado;
    public float nivelPeligrosidad;
    public Sprite foto; // Foto del paciente
}
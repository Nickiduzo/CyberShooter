using System;
using UnityEngine;

[CreateAssetMenu(fileName ="DoubleSwords", menuName ="Swords/DoubleSwords")]
public class DoubleSwords : ScriptableObject
{
    [SerializeField] private GameObject[] usual;
    [SerializeField] private GameObject[] donate;
    [SerializeField] private GameObject[] exclusive;

    [SerializeField] private float damage;

}

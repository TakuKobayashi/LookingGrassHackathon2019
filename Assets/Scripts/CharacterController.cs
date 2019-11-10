using System.Collections.Generic;
using UnityEngine;
using LookingGlass;

public class CharacterController : SingletonBehaviour<CharacterController>
{
    [SerializeField] private GameObject characterObj;
    [SerializeField] private Holoplay previewField;
    [SerializeField] private Vector3 defaultPosition;
    [SerializeField] private Vector3 defaultScale;

    public Holoplay PreviewField
    {
        get
        {
            return previewField;
        }
    }

    private List<Character> characters = new List<Character>();

    void Start()
    {
        Character character = ComponentUtil.InstantiateTo<Character>(this.gameObject, characterObj);
        Vector3 targetPosition = previewField.transform.position;
        targetPosition.z -= previewField.GetCamDistance();
        character.InputPreviewTarget(targetPosition);
        character.InitDefaults(defaultPosition, defaultScale);
        characters.Add(character);
    }
}

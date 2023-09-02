using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A die shape which maps side directions to values.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Die")]
public class DieShape : ScriptableObject
{
    [SerializeField]
    private SerializableDictionary<Vector3, int> sides;

    /// <summary>
    /// The value in (or closest to) the given direction.
    /// </summary>
    public int ValueInDirection(Vector3 direction)
    {
        return sides
            .OrderByDescending(pair => Vector3.Dot(pair.Key, direction))
            .First()
            .Value;
    }

    /// <summary>
    /// A direction with the given value. Returns Vector3.zero if the value is not present.
    /// </summary>
    public Vector3 DirectionOfValue(int value)
    {
        return sides.First(pair => pair.Value == value).Key;
    }

    /// <summary>
    /// All directions with the given value.
    /// </summary>
    public IEnumerable<Vector3> DirectionsOfValue(int value)
    {
        return sides
            .Where(pair => pair.Value == value)
            .Select(pair => pair.Key);
    }
}
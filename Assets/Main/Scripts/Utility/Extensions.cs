using UnityEngine;
using System;
static class ExtensionMethods
{
    /// <summary>
    /// Rounds Vector3.
    /// </summary>
    /// <param name="vector3"></param>
    /// <param name="decimalPlaces"></param>
    /// <returns></returns>
    public static Vector3 Round(this Vector3 vector3, int decimalPlaces)
    {
        return new Vector3(
             (float)Math.Round(vector3.x, decimalPlaces),
            (float)Math.Round(vector3.y, decimalPlaces),
            (float)Math.Round(vector3.z, decimalPlaces));
    }
}
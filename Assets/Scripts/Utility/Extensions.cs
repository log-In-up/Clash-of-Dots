using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utility
{
    public static class Extensions
    {
        public static void GetSafeComponent<T>(this GameObject gameObject, out T component) where T : Component
        {
            if (gameObject.TryGetComponent(out T foundComponent))
            {
                component = foundComponent;
            }
            else
                throw new MissingComponentException($"There is no {typeof(T)} component on the {gameObject}.");
        }
    }
}
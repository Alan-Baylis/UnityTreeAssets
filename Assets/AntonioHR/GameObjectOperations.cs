using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AntonioHR
{
    public static class GameObjectOperations
    {
        public static GameObject CreateChild(this GameObject self, string name)
        {
            var newObj = new GameObject(name);
            newObj.transform.parent = self.transform;
            return newObj;
        }
    }
}

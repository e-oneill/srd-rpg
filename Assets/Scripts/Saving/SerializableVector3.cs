using UnityEngine;
namespace RPG.Saves
{
    [System.Serializable]
    public class SerializableVector3 
    {
        float x, y, z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;

            
        }

        public Vector3 ToVector()
        {
            Vector3 retVector = new Vector3();
            retVector.x = this.x;
            retVector.y = this.y;
            retVector.z = this.z;

            return retVector;
        }
    }
}
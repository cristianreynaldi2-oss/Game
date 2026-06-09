using UnityEngine;

public class Exception : MonoBehaviour
{
    void Start()
    {
        try
        {
            float damage = -10;

            if (damage < 0)
            {
                throw new InvalidDamageException(
                    "Damage tidak boleh negatif"
                );
            }
        }
        catch (InvalidDamageException ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Material originalMaterial;
    public Material damageMaterial;
    public float damageTime = 0.2f;
    public Coroutine damageCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public virtual void SetDamageMaterial()
    {
        EnterDamageVFX();
    }

    protected virtual void EnterDamageVFX()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(EnterEntityVFCWithCor());
    }

    protected virtual IEnumerator EnterEntityVFCWithCor()
    {
        spriteRenderer.material = damageMaterial;
        yield return new WaitForSeconds(damageTime);
        spriteRenderer.material = originalMaterial;
    }


}
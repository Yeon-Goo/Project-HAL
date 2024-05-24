using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [SerializeField] private List<GameObject> effectPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEffect(string effectName, Vector2 position, Vector2 scale)
    {
        GameObject effectPrefab = effectPrefabs.Find(e => e.name == effectName);
        if (effectPrefab != null)
        {
            StartCoroutine(PlayAndDestroy(effectPrefab, position, scale));
        }
        else
        {
            Debug.LogWarning("Effect not found: " + effectName);
        }
    }

    private IEnumerator PlayAndDestroy(GameObject effectPrefab, Vector2 position, Vector2 scale)
    {
        GameObject effectInstance = Instantiate(effectPrefab, position, Quaternion.identity);
        effectInstance.transform.localScale = new Vector3(scale.x, scale.y, 1); // 이펙트 크기 설정
        Animator animator = effectInstance.GetComponent<Animator>();

        if (animator != null)
        {
            AnimatorClipInfo[] clips = animator.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0)
            {
                float animationLength = clips[0].clip.length;
                yield return new WaitForSeconds(animationLength);
            }
        }
        else
        {
            ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                yield return new WaitForSeconds(particleSystem.main.duration);
            }
        }

        Destroy(effectInstance);
    }
}

using UnityEngine;

public class Doorkey : MonoBehaviour
{
    [HideInInspector]
    public Door m_Door;
    public SpriteRenderer mainSprite;
    public Sprite offSprite;
    public Sprite onSprite;
    public Light pointLight;
    MaterialPropertyBlock rendererPropertyBlock;
    public Color color;
    public ParticleSystem openingParticleSystem;
    AudioSource audioSource;
    public Animator animator;

    void OnValidate() {
        rendererPropertyBlock = null;
        SetColor(color);
    }

    void Start() {
        SetColor(color);
        audioSource = GetComponent<AudioSource>();
    }

    public void SetColor(Color color) {
        this.color = color;
        if (mainSprite == null) return;

        if(rendererPropertyBlock == null) {
            rendererPropertyBlock = new MaterialPropertyBlock();
            mainSprite.GetPropertyBlock(rendererPropertyBlock);
        }

        rendererPropertyBlock.SetColor("_EmissionColor", color);
        mainSprite.SetPropertyBlock(rendererPropertyBlock);
        if (pointLight != null) pointLight.color = color;

	    if (openingParticleSystem != null)
	    {
		    var main = openingParticleSystem.main;
		    main.startColor = color;
	    }
    }

    void OnTriggerEnter2D ( Collider2D other )
    {
        if ( !other.gameObject.CompareTag ( "Player" ) )
            return;

        bool status = mainSprite.sprite == offSprite;

        m_Door.KeyTriggered ( !status );

        if ( audioSource != null )
            audioSource.Play ();

        mainSprite.sprite = status ? onSprite : offSprite;

        openingParticleSystem.transform.SetParent ( null );
        var main = openingParticleSystem.main;
        main.startLifetimeMultiplier = Vector3.Distance ( transform.position, m_Door.transform.position ) / main.startSpeedMultiplier;
        openingParticleSystem.transform.position = transform.position + Vector3.up * .5f;
        openingParticleSystem.transform.up = ( m_Door.transform.position - transform.position ).normalized;
        openingParticleSystem.Play ();
    }
}

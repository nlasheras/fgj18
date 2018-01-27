using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    private Animator m_animator;
    private SpriteRenderer m_renderer;

    private int m_idleAnim;

    public int m_anim;
    public bool m_mirror;


    private int m_currentAnim;

	
	void Start () {
        m_animator = GetComponent<Animator>();
        m_renderer = GetComponent<SpriteRenderer>();

        m_idleAnim = Animator.StringToHash("Base Layer.idle_right");
        m_currentAnim = -1;
	}
	
	void Update () {
        if (m_anim != m_currentAnim)
        {
            m_animator.Play(m_idleAnim, -1);
        }
        m_renderer.flipX = m_mirror;
	}
}

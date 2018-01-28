using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueAnimation : MonoBehaviour {

    private Animator m_animator;
    private int m_effectAnim;

	void Start () {
        m_animator = GetComponent<Animator>();
        m_effectAnim = Animator.StringToHash("Base Layer.statue_transmission");
	}
	
    public void PlayAnimation()
    {
        m_animator.Play(m_effectAnim, -1);
    }

}

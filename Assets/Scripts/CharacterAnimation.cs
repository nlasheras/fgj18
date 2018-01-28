using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    private Animator m_animator;
    private SpriteRenderer m_renderer;

    private int m_idleAnim;
    private int m_walkAnim;
    private int m_attackAnim;
    private int m_jumpAnim;

    private int m_desiredAnim;
    private int m_currentAnim;
    private bool m_mirror;

    private const int STATE_IDLE = 0;
    private const int STATE_WALK = 1;
	private const int STATE_ATTACK = 2;
    private const int STATE_JUMP = 3;
    private const int STATE_TRANSMISSION_ATTACK = 4;
    private const int STATE_TRANSMISSION_JUMP = 5;
    private const int STATE_TRANSMISSION_BACK = 6;

    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_renderer = GetComponent<SpriteRenderer>();

        m_idleAnim = Animator.StringToHash("Base Layer.idle_right");
        m_walkAnim = Animator.StringToHash("Base Layer.walk_right");
        m_attackAnim = Animator.StringToHash("Base Layer.attack_right");
        m_jumpAnim = Animator.StringToHash("Base Layer.jump_right");

        m_currentAnim = -1;
        m_desiredAnim = STATE_IDLE;
	}
	
    int getHashForState(int state)
    {
        switch(state)
        {
            case STATE_IDLE: return m_idleAnim;
            case STATE_WALK: return m_walkAnim;
            case STATE_ATTACK: return m_attackAnim;
            case STATE_JUMP: return m_jumpAnim;

            case STATE_TRANSMISSION_BACK: return Animator.StringToHash("Base Layer.transmission_back");
            case STATE_TRANSMISSION_JUMP: return Animator.StringToHash("Base Layer.transmission_jump");
            case STATE_TRANSMISSION_ATTACK: return Animator.StringToHash("Base Layer.transmission_attack");
        }
        return m_idleAnim;
    }

	void Update () 
    {
        if (m_desiredAnim != m_currentAnim)
        {
            m_animator.Play(getHashForState(m_desiredAnim), -1);
        }
        m_renderer.flipX = m_mirror;
	}

    private void setState(int state, bool mirror)
    {
        m_desiredAnim = state;
        m_mirror = mirror;
    }

    public void moveRight() { setState(STATE_WALK, false); }
    public void moveLeft() { setState(STATE_WALK, true); }

    public void idleRight() { setState(STATE_IDLE, false); }
    public void idleLeft() { setState(STATE_IDLE, true); }

    public void jumpRight() { setState(STATE_JUMP, false); }
    public void jumpLeft() { setState(STATE_JUMP, true); }

    public void attackRight() { setState(STATE_ATTACK, false); }
    public void attackLeft() { setState(STATE_ATTACK, true); }

    public void playTransmission(Player.PlayerSkill skill, int delay = 150)
    {
        if (skill == Player.PlayerSkill.ATTACK)
            setState(STATE_TRANSMISSION_ATTACK, false);
        else if (skill == Player.PlayerSkill.JUMP)
            setState(STATE_TRANSMISSION_JUMP, false);
        else
            setState(STATE_TRANSMISSION_BACK, false);
    }


}

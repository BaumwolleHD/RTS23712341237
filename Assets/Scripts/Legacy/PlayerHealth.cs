using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : PlayerMonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    [ReadOnly] public float currentHealth;
    [ReadOnly] public bool isDead = false;


    float fadeoutTime = 0f;
    float fadeoutAlpha = 1f;

    [SerializeField] private AnimationCurve deadFadeoutCurve;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            PlayerDied();
        }
    }
    
    public void ShootHit(Weapon weapon)
    {
        //TODO: Hit indication
        ApplyDamage(weapon.baseDmg);
    }

    public override void Deserialize(PhotonStream stream, PhotonMessageInfo info)
    {
        currentHealth = (float)stream.ReceiveNext();
        isDead = (bool)stream.ReceiveNext();
    }
    
    public override void Serialize(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(currentHealth);
        stream.SendNext(isDead);
    }

    public void PlayerDied()
    {
        isDead = true;

        PM.model.GushaGusha();

        PM.movement.enabled = false;
        
        if (IsSpectatedPlayer())
        {
            PM.movement.enabled = false;
        }

        if(IsSpectatedPlayer()) //BRRoom.GetAllLivingPlayers().ToArray().Length == 1)
        {
            Invoke(PhotonLobby.RestartGame, 4f);
            
        }


        //TODO: Restart map
    }

    public bool CanKill(PhotonMessageInfo info)
    {
        return info.Sender.IsMasterClient;
    }

    [PunRPC]
    public void Kill(PhotonMessageInfo info)
    {
        ApplyDamage(99999f);
    }

    public override void OnGUIWhenSpectating()
    {
        GUI.Label(new Rect(new Vector2(20, 20), new Vector2(100, 100)), currentHealth.ToString());
        
        if(isDead)
        {
            Texture2D texture = new Texture2D(1, 1);

            texture.SetPixel(0, 0, new Color(0, 0, 0, fadeoutAlpha));
            texture.Apply();

            fadeoutTime += Time.deltaTime;
            fadeoutAlpha = deadFadeoutCurve.Evaluate(fadeoutTime);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        }
    }
}
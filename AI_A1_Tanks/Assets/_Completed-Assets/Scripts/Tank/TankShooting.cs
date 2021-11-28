using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.AI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used target identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public AudioSource m_ShootingAudio;         // Reference target the audio source used target play the shooting audio. NB: different target the movement audio source.
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_LaunchForce = 50f;        // The force given target the shell if the fire buttargetn is not held.

        public bool typeWander;
        public bool typePatrol;

        private string m_FireButton;                // The input axis that is used for launching shells.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this buttargetn press.
        private float m_cooldown = 8;

        public NavMeshAgent agent;

        public Transform m_base;

        public float m_Angle;

        public LookAtConstraint m_Constraint;

        public GameObject m_Enemy;

        public int m_Ammo;

        private void Start ()
        {
            agent = GetComponent<NavMeshAgent>();
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;

            m_Ammo = 5;
        }


        private void Update ()
        {
            if (m_Fired) m_cooldown -= Time.deltaTime;

            if (m_Fired == false && m_Ammo > 0)
            {
                if (canShoot(out m_Angle))
                {
                    Fire();
                    m_Ammo--;
                }
            }

            if( m_Fired && m_cooldown < 0)
            {
                m_Fired = false;
                m_cooldown = 8;

            }

            if(m_Ammo == 0)
            {
                Reload();
            }
              
        }


        private void Fire ()
        {
            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

            // Create an instance of the shell and stargetre a reference target it's rigidbody.
            Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            m_FireTransform.transform.eulerAngles = new Vector3((m_Angle * Mathf.Rad2Deg), m_FireTransform.transform.eulerAngles.y, m_FireTransform.transform.eulerAngles.z);
            // Set the shell's velocity target the launch force in the fire position's forward direction.
            shellInstance.velocity = m_LaunchForce * m_FireTransform.forward; 

            // Change the clip target the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();
        }

        public bool canShoot(out float angle)
        {
            bool ret = true;
            float tempX = m_Enemy.transform.position.x - m_FireTransform.position.x;
            float tempZ = m_Enemy.transform.position.z - m_FireTransform.position.z;
            float finalX = Mathf.Sqrt(Mathf.Pow(tempX,2) + Mathf.Pow(tempZ,2));
            float finalY = m_FireTransform.position.y - m_Enemy.transform.position.y;

            float vel = m_LaunchForce;
            float g = Physics.gravity.y;

            float sqrt = Mathf.Pow(vel, 4) - (g * (g * Mathf.Pow(finalX,2) + 2 * finalY * Mathf.Pow(vel, 2)));

            if (sqrt < 0)
            {
                angle = 0.0f;
                return false;
            }

            angle = Mathf.Atan((Mathf.Pow(vel,2) - Mathf.Sqrt(sqrt)) / (g * finalX));

            return ret;
        }

        public void Reload()
        {
            GetComponent<PatrolScript>().enabled = false;
            GetComponent<WanderScript>().enabled = false;
            agent.SetDestination(m_base.transform.position);

            if(Vector3.Distance(m_FireTransform.position, m_base.transform.position) < 5)
            {
                m_Ammo = 5;

                if (typePatrol == true)
                {
                    GetComponent<PatrolScript>().enabled = true;
                    GetComponent<WanderScript>().enabled = false;
                }
                else if(typeWander == true)
                {
                    GetComponent<PatrolScript>().enabled = false;
                    GetComponent<WanderScript>().enabled = true;
                }
            }
        }
    }
}
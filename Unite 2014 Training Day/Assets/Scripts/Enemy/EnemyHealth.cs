using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;





public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
		/*setup messaging test with web sockets
		System.Net.Sockets clientSocket = new System.net.Sockets.TcpClient();
		clientSocket.connect("216.58.216.78", 80);

		NetworkStream s = new NetworkStream(soc);
		NetworkStream serverStream = clientSocket.GetStream();
		byte[] outStream = "Test Message";
		serverStream.Write(outStream, 0, outstream.Length);
		serverStream.Flush(); */
		byte[] data = new byte[1024];
		string input, stringData;
		input = "l";
		TcpClient server;
		
		
		try
		{
			server = new TcpClient("10.110.22.4", 8888);
		} catch (SocketException)
		{
			Console.WriteLine("Unable to connect to server");
			return;
		}
		NetworkStream ns = server.GetStream();
		ns.Write (System.Text.Encoding.ASCII.GetBytes(input),0,input.Length);
		ns.Flush();
		ns.Close();
		server.Close();


    }


    public void StartSinking ()
    {
        GetComponent <NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        //ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}

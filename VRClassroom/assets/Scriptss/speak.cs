using UnityEngine;
using System.Collections;
//Interconectar con el oculus.
public class speak : MonoBehaviour {
	public AudioClip[] spanishAudioClip;
	public AudioClip[] englishAudioClip;
	public AudioClip[] attSpanishAudioClip;
	public AudioClip[] attEnglishAudioClip;
	AudioSource audio;
	private int currentClip;
	public Animator animator;

	private string nameAudio;
	private string nameAnimation;
	private string nameAnimationEn;
	private string nameAnimationSp;
	private int num;
	private int lenguage;
	private int att;
	// Use this for initialization
	void Start () {
		//englishAudioClip= new AudioClip[16];
		//spanishAudioClip= new AudioClip[48];
		//atentionlishAudioClip= new AudioClip[3];
		att = 0;
		lenguage=0;
		currentClip = 6;
		num = 0;
		nameAudio = "videoen";
		nameAnimation="";
		nameAnimationEn="animen";
		nameAnimationSp="anim";
		//myAudioClip = new AudioClip[48]; 
		Animation animation = GetComponent<Animation>(); 
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
		animator.Play("interruption");
		//attention ();
		//audio.Stop();
		//Debug.Log (audio.clip.length);
		//audio.Clip = otherClip;
		//audio.Play();
		//currentClip++;
		//myAudioClip[1]=new AudioClip();
		//myAudioClip[1].
		//animator.Play(animator.GetHashCode);
		//animation.Play("anim1");
		//animation.Play (animacion);
		
	}

	void Update() {
		StartCoroutine(playAudio());
	}

	void setLenguage(int l){
		lenguage=l;
	}

	IEnumerator playAudio(){
		//Debug.Log (animator.StopPlayBack());

		//Debug.Log (audio.clip.length);
		if (!audio.isPlaying) {

			if(lenguage==0){
				audio.clip = englishAudioClip[currentClip-1];
				nameAnimation=nameAnimationEn;
			}else{
				audio.clip = spanishAudioClip[currentClip-1];
				nameAnimation=nameAnimationSp;
			}
			Debug.Log (nameAnimation+currentClip);
			Debug.Log (spanishAudioClip[currentClip-1]);
			Debug.Log (audio.clip.length);
			animator.Play(nameAnimation+currentClip);
			audio.Play();
			
			//yield WaitForSeconds(audio.clip.length);
			
			yield return new WaitForSeconds(audio.clip.length);
			audio.Stop();
			currentClip++;
			//attention();

			if(currentClip>15&&lenguage==0){
				currentClip=0;
			}
		

			if(att!=0){
				if(lenguage==0){
					audio.clip = attEnglishAudioClip[att-1];
					nameAnimation="atten";
				}else{
					audio.clip = attSpanishAudioClip[att-1];
					nameAnimation="attsp";
				}
				animator.Play(nameAnimation+att);
				audio.Play();

				yield return new WaitForSeconds(audio.clip.length);
				audio.Stop();
			}
		}
	}

	public void attention(){
		att++;
	}	
}


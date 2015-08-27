using UnityEngine;
using System.Collections;

public class speak : MonoBehaviour {
	public AudioClip[] spanishAudioClip;
	public AudioClip[] englishAudioClip;
	public AudioClip[] attSpanishAudioClip;
	public AudioClip[] attEnglishAudioClip;
	AudioSource audio;
	public int currentClip;
	public Animator animator;
	
	private string nameAudio;
	private string nameAnimation;
	private string nameAnimationEn;
	private string nameAnimationSp;
	private int num;
	public int lenguage;
	public int att;
	public int pause;
	public int call;
	public bool f;
	
	// Use this for initialization
	void Start () {
		f=false;
		att = 0;
		pause = 0;
		lenguage=GameObject.Find("PlaneO").GetComponent<lenguage>().l;
		//lenguage=0;
		currentClip = 1;
		num = 0;
		call = 0;
		nameAudio = "videoen";
		nameAnimation="";
		nameAnimationEn="animen";
		nameAnimationSp="anim";
		Animation animation = GetComponent<Animation>(); 
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
		animator.Play("interruption");
		StartCoroutine(playAudio());
	}
	
	void Update() {
		
	}
	
	void setLenguage(int l){
		lenguage=l;
	}
	
	public void pauseAnimation(){
		pause = 1;
	}
	
	public void unpauseAnimation(){
		pause = 0;
	}
	
	IEnumerator playAudio(){
		//Debug.Log (animator.StopPlayBack());  
		
		//Debug.Log (audio.clip.length);
		for (int i = 0; i < 317+lenguage*320; i++) {

			bool distracted=GameObject.Find("Scripts").GetComponent<IsLookingAt>().isStudentDistracted;
			//Debug.Log ("Distraido: "+distracted);
			//Debug.Log ("call: "+call);
			//Debug.Log ("Tocando: "+audio.isPlaying);
			if (!audio.isPlaying) {			
				if ((att != 0 && call==1)||(distracted==true && call==1)) {
					i--;
					if (lenguage == 0) {
						audio.clip = attEnglishAudioClip [att - 1];
						nameAnimation = "atten";
					} else {
						audio.clip = attSpanishAudioClip [att - 1];
						nameAnimation = "attsp";
					}
					//Debug.Log (nameAnimation + att);
					///Debug.Log (audio.clip.length);
					animator.Play (nameAnimation + att);
					audio.Play ();

					GameObject.Find ("Projector").GetComponent<Presentation>().pause = true;
					GameObject.Find ("Teacher").GetComponent<LookAtStudents>().pause = true;
					
					yield return new WaitForSeconds (audio.clip.length);
					audio.Stop ();
					GameObject.Find ("Projector").GetComponent<Presentation>().pause = false;
					GameObject.Find ("Teacher").GetComponent<LookAtStudents>().pause = false;

					call=0;
					pause=0;
					att++;

					if (att > 4) {
						pause = 1;						
					}

				}else{
					if(pause==0){
						if (lenguage == 0 ) {
							audio.clip = englishAudioClip [currentClip - 1];
							nameAnimation = nameAnimationEn;
						} else {
							audio.clip = spanishAudioClip [currentClip - 1];
							nameAnimation = nameAnimationSp;
						}
						//Debug.Log (nameAnimation + currentClip);
						//Debug.Log (audio.clip.length);
						animator.Play (nameAnimation + currentClip);
						audio.Play ();
						
						//yield return new WaitForSeconds (audio.clip.length);
						int t= (int) audio.clip.length+1;
						bool bouttonPress=false;
						for (int j = 0; j < t ; j++) {
							if(audio.isPlaying){
								yield return new WaitForSeconds (1);
							}
							if(bouttonPress==false){
								if(GameObject.Find("TheStudent").GetComponent<Hand>().buttonValue != -1 ){
									if(GameObject.Find("TheStudent").GetComponent<Hand>().buttonValue == 4 ){
										Debug.Log ("Adelanto");
										advance();
										GameObject.Find ("Projector").GetComponent<Presentation>().advance();
										audio.Stop ();
										animator.Play("interruption");
										bouttonPress=true;
										Debug.Log("SA1");
										break;
										Debug.Log("SA2");
									}else{									
										if(GameObject.Find("TheStudent").GetComponent<Hand>().buttonValue == 5 ){
											Debug.Log ("Retraso");
											backward();
											GameObject.Find ("Projector").GetComponent<Presentation>().getBack();
											i--;
											audio.Stop ();
											animator.Play("interruption");
											bouttonPress=true;
											break;
										}
									}
								}
							}
							Debug.Log("SA3");
						}
						Debug.Log("SA4");
						audio.Stop ();
						currentClip++;
						
						if ((currentClip > 17 && lenguage == 0)||(currentClip > 49 && lenguage == 1)) {
							pause=1;
							f=true;
						}

						distracted=GameObject.Find("Scripts").GetComponent<IsLookingAt>().isStudentDistracted;

						if(distracted==true&&call==0){
							if(att==0){
								att++;
							}
							call=1;						
						}
					}
				}


				
			} else {
				//Debug.Log ("Esperando 1" );
				//yield return new WaitForSeconds (1);
			}
		}
	}

	public void advance(){
		//currentClip++;

		if (currentClip>16) {
			currentClip=16;
		}
	}

	public void backward(){
		currentClip=currentClip-2;


		if(currentClip<0){
			currentClip=1;
		}
	}

	public void attention(){
		if(att==0){
			att++;
		}
		call = 1;
	}	
}


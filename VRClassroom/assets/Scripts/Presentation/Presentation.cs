using UnityEngine;
using System.Collections;

public class Presentation : MonoBehaviour {

	public Projector projectorLight;
	public Texture[] texturesSpanish;
	public Texture[] texturesEnglish;
	public float[] timesSpanish;
	public float[] timesEnglish;
	public int numSlides;

	public int language;

	public bool pause;

	private float timeLeft;
	private int actualSlide;

	// Use this for initialization
	void Start () {
		if (language == 1) {
			projectorLight.material.SetTexture ("_ShadowTex", texturesSpanish [0]);
			timeLeft = timesSpanish[0];
		} else {
			projectorLight.material.SetTexture ("_ShadowTex", texturesEnglish [0]);
			timeLeft = timesEnglish[0];
		}
		actualSlide = 0;
		pause = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (pause) {
			return;
		}
		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0) {
			actualSlide++;
			if(actualSlide < numSlides){
				if (language == 1) {
					projectorLight.material.SetTexture ("_ShadowTex", texturesSpanish [actualSlide]);
					timeLeft = timesSpanish[actualSlide];
				} else {
					projectorLight.material.SetTexture ("_ShadowTex", texturesEnglish [actualSlide]);
					timeLeft = timesEnglish[actualSlide];
				}
			} else{
				projectorLight.material.SetTexture ("_ShadowTex", null);
			}
		}
	}
}

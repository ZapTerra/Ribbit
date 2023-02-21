using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class UIManager : MonoBehaviour {

	Transform rt;
	bool open;
	Vector3 home;
	Vector3 superhome;
	float hidetimer;
	bool hidden;
	string CONSUMER_KEY = "4shqI7mZadGuZK4L9kPirxaw9";
	string CONSUMER_SECRET = "CLgmucO8fb9mJW7FCd7ksfWVxWWDvy0yM0cbLOkQPDQamwXuk6";
	Vector3 home2;
	BoxCollider col;
	float movespeed = 5;
	bool movey;
	float timer;
	float starttimer;
	bool started;
	int end;
	string scenetoload;
	Texture2D snapshot;
	Camera outputCam;
	RenderTexture outputCamRenderTexture;
	public Texture2D tempResidualTex;
	Rect screenRect;
	[SerializeField]
	GameObject[] screens;
	[SerializeField]
	GameObject screenshotobj;
	[SerializeField]
	GameObject screenshotobj2;
	[SerializeField]
	GameObject nameoffriend;
	Image sosr;
	Image sosr2;
	[SerializeField]
	GameObject[] usernameobject;
	string m_PIN;
	string requestToken;
	string sometext = "";
	[SerializeField]
	GameObject txtobj;
	[SerializeField]
	GameObject filloutobj;
	[SerializeField]
	GameObject okbutton;
	[SerializeField]
	GameObject[] infobuttons;
	Text txt;
	InputField txt2;


	void Start() {
		rt = GetComponent<Transform>();
		home2 = rt.position + Vector3.up;
		home = new Vector3(rt.position.x, -4, rt.position.z);
		superhome = home;
		col = GetComponent<BoxCollider>();
		end = 0;
		outputCam = Camera.main;
		outputCamRenderTexture = new RenderTexture(Screen.width, Screen.height, 0);
		tempResidualTex = new Texture2D(Screen.width, Screen.height);
		screenRect = new Rect(0, 0, Screen.width, Screen.height);
		sosr = screenshotobj.GetComponent<Image>();
		sosr2 = screenshotobj2.GetComponent<Image>();
		txt = txtobj.GetComponent<Text>();
		txt2 = filloutobj.GetComponent<InputField>();
	}

	// Update is called once per frame
	void Update() {

		if (open == true) {
			int val = Mathf.FloorToInt(Input.mousePosition.x / Screen.width * 4);
			for (int i = 0; i < 4; i++) {
				if (i == val) {
					infobuttons[i].SetActive(true);
				} else {
					infobuttons[i].SetActive(false);
				}
			}
		}

		hidetimer += Time.deltaTime;
		if (hidetimer > 0.5f && !hidden) {
			screens[2].SetActive(false);
			screens[3].SetActive(false);

			hidden = true;

		}
		if (started == false) {
			starttimer += Time.deltaTime;
			if (starttimer > 5) {
				movey = true;
				started = true;
			}
		}
		if (open == true && movey == true) {
			rt.position = Vector3.Lerp(rt.position, home, Time.deltaTime * movespeed);
			timer += Time.deltaTime;
			if (timer > 2) {
				movey = false;


			}

		} else if (open == false && movey == true) {
			rt.position = Vector3.Lerp(rt.position, home2, Time.deltaTime * movespeed);
			timer += Time.deltaTime;
			for (int i = 0; i < 4; i++) {
				infobuttons[i].SetActive(false);
			}
			if (timer > 2) {
				movey = false;

			}
		}

		if (end == 3 || end == 4) {
			starttimer += Time.deltaTime;
			if (starttimer > 0.16f && end == 3) {
				TransitionScene();
				end = 4;
				movespeed = 3;

			}
			if (starttimer > 1.15f) {
				SceneManager.LoadScene(scenetoload);

			}
		}

		if (end == 1 || end == 2) {
			starttimer += Time.deltaTime;
			if (starttimer > 0 && end == 1) {
				TransitionScene();
				movespeed = 14;

				end = 2;
			}
		}

		if (end == 5 || end == 6) {
			starttimer += Time.deltaTime;
			if (starttimer > 0.16f && end == 5) {
				TransitionScene();
				end = 6;
				movespeed = 3;

			}

		}

	}

	public void MoveUp() {
		if (end == 0) {
			open = true;
			movey = true;
			col.enabled = true;
			//movespeed = 14;

			timer = 0;
		}

	}



	private void OnMouseDown() {
		if (end == 0) {
			int val = Mathf.FloorToInt(Input.mousePosition.x / Screen.width * 4);

			if (val == 0) {
				end = 3;
				starttimer = 0;
				started = true;

				GetComponent<Renderer>().material.SetColor("_TransColor", Color.black);
				scenetoload = "SummonScene";
				screens[val].SetActive(true);

			}
			if (val == 1) {
				end = 3;
				starttimer = 0;
				started = true;
				scenetoload = SceneManager.GetActiveScene().name;
				screens[val].SetActive(true);

			}
			if (val == 2) {
				end = 1;
				starttimer = 0;
				started = true;
				StartCoroutine(TakeSnapshot(Screen.width, Screen.height));
				screens[val].SetActive(true);
				GetComponent<AudioSource>().Play();
				//scenetoload = SceneManager.GetActiveScene().name;
			}

			if (val == 3) {
				end = 5;
				starttimer = 0;
				started = true;
				screens[val].SetActive(true);

				//scenetoload = SceneManager.GetActiveScene().name;
			}


		}

	}

	public void ResetFromLast() {
		end = 0;
		open = false;
		movey = true;
		timer = 0;
		col.enabled = false;
		movespeed = 5;
		home = superhome;
		hidetimer = 0;
		hidden = false;
	}



	private void TransitionScene() {
		home = home + Vector3.up * 12;
		movey = true;
		timer = 0;
	}


	void OnMouseExit() {
		if (end == 0) {
			open = false;
			movey = true;
			timer = 0;
			col.enabled = false;
		}


		//The mouse is no longer hovering over the GameObject so output this message each frame
	}

	public void ToClipboard() {
		CopyToClipboard(tempResidualTex);
	}

	public void SaveImage() {
		byte[] textureBytes = tempResidualTex.EncodeToPNG();
		string str = nameoffriend.GetComponent<UnityEngine.UI.InputField>().text;
		if (str == "") {
			str = "Untitled";
		}
		//DownloadFile(textureBytes, textureBytes.Length, str + ".png");
	}

	public string GetSafeFilename(string filename) {

		return string.Join("_", filename.Split(System.IO.Path.GetInvalidFileNameChars()));

	}


	private void CopyToClipboard(Texture2D texture) {
		System.IO.Stream s = new System.IO.MemoryStream(texture.width * texture.height);
		byte[] bits = texture.EncodeToPNG();
		s.Write(bits, 0, bits.Length);
		s.Close();
		s.Dispose();
	}


	public IEnumerator TakeSnapshot(int width, int height) {
		yield return new WaitForEndOfFrame();
		outputCam.targetTexture = outputCamRenderTexture;
		RenderTexture.active = outputCamRenderTexture;
		outputCam.Render();
		tempResidualTex.ReadPixels(screenRect, 0, 0);
		tempResidualTex.Apply();
		Sprite spr = Sprite.Create(tempResidualTex, screenRect, Vector2.zero);
		sosr.sprite = spr;
		sosr2.sprite = spr;

		RenderTexture.active = null;
		outputCam.targetTexture = null;
	}








	public void GetPastedText(string newpastedtext) {
		sometext = newpastedtext;
	}



	//IEnumerator Wait()
	//{
	//print(Time.time);

	//print(Time.time);
	//}









}
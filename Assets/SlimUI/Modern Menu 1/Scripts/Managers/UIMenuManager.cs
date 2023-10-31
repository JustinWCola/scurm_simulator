using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu
{
	public class UIMenuManager : MonoBehaviour
	{
		private Animator CameraObject;

		// campaign button sub menu
		[Header("MENUS")]
		[Tooltip("The Menu for when the MAIN menu buttons")]
		public GameObject mainMenu;
		[Tooltip("THe first list of buttons")]
		public GameObject firstMenu;
		[Tooltip("The Menu for when the PLAY button is clicked")]
		public GameObject playMenu;
		[Tooltip("The Menu for when the EXIT button is clicked")]
		public GameObject exitMenu;
		[Tooltip("Optional 4th Menu")]

		public enum Theme { custom1, custom2, custom3 };
		[Header("THEME SETTINGS")]
		public Theme theme;
		public ThemedUIData themeController;

		[Header("PANELS")]
		[Tooltip("The UI Panel parenting all sub menus")]
		public GameObject mainCanvas;
		[Tooltip("The UI Panel that holds the 1st window tab")]
		public GameObject Panel1;
		[Tooltip("The UI Panel that holds the 2nd window tab")]
		public GameObject Panel2;
		[Tooltip("The UI Panel that holds the 3rd window tab")]
		public GameObject Panel3;

		// highlights in settings screen
		[Header("SETTINGS SCREEN")]
		[Tooltip("Highlight Image for when 1st Tab is selected in Settings")]
		public GameObject line1;
		[Tooltip("Highlight Image for when 2nd Tab is selected in Settings")]
		public GameObject line2;
		[Tooltip("Highlight Image for when 3rd Tab is selected in Settings")]
		public GameObject line3;

		[Header("LOADING SCREEN")]
		public GameObject loadingMenu;
		[Tooltip("The loading bar Slider UI element in the Loading Screen")]
		public Slider loadingBar;

		[Header("SFX")]
		[Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
		public AudioSource hoverSound;
		[Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
		public AudioSource sliderSound;
		[Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
		public AudioSource swooshSound;

		void Start()
		{
			CameraObject = transform.GetComponent<Animator>();

			playMenu.SetActive(false);
			exitMenu.SetActive(false);
			firstMenu.SetActive(true);
			mainMenu.SetActive(true);

			SetThemeColors();
		}

		void SetThemeColors()
		{
			switch (theme)
			{
				case Theme.custom1:
					themeController.currentColor = themeController.custom1.graphic1;
					themeController.textColor = themeController.custom1.text1;
					break;
				case Theme.custom2:
					themeController.currentColor = themeController.custom2.graphic2;
					themeController.textColor = themeController.custom2.text2;
					break;
				case Theme.custom3:
					themeController.currentColor = themeController.custom3.graphic3;
					themeController.textColor = themeController.custom3.text3;
					break;
				default:
					Debug.Log("Invalid theme selected.");
					break;
			}
		}

		public void PlayCampaign()
		{
			exitMenu.SetActive(false);
			playMenu.SetActive(true);
		}

		public void ReturnMenu()
		{
			playMenu.SetActive(false);
			exitMenu.SetActive(false);
			mainMenu.SetActive(true);
		}

		public void LoadScene(string scene)
		{
			if (scene != "")
			{
				StartCoroutine(LoadAsynchronously(scene));
			}
		}

		public void DisablePlayCampaign()
		{
			playMenu.SetActive(false);
		}

		public void Position1()
		{
			CameraObject.SetFloat("Animate", 0);
		}

		public void Position2()
		{
			DisablePlayCampaign();
			CameraObject.SetFloat("Animate", 1);
		}

		void DisablePanels()
		{
			Panel1.SetActive(false);
			Panel2.SetActive(false);
			Panel3.SetActive(false);

			line1.SetActive(false);
			line2.SetActive(false);
			line3.SetActive(false);
		}

		public void EnablePanel1()
		{
			DisablePanels();
			Panel1.SetActive(true);
			line1.SetActive(true);
		}

		public void EnablePanel2()
		{
			DisablePanels();
			Panel2.SetActive(true);
			line2.SetActive(true);
		}

		public void EnablePanel3()
		{
			DisablePanels();
			Panel3.SetActive(true);
			line3.SetActive(true);
		}

		public void PlayHover()
		{
			hoverSound.Play();
		}

		public void PlaySFXHover()
		{
			sliderSound.Play();
		}

		public void PlaySwoosh()
		{
			swooshSound.Play();
		}

		// Are You Sure - Quit Panel Pop Up
		public void AreYouSure()
		{
			exitMenu.SetActive(true);
			DisablePlayCampaign();
		}

		public void QuitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
		}

		// Load Bar synching animation
		IEnumerator LoadAsynchronously(string sceneName)
		{ // scene name is just the name of the current scene being loaded
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
			operation.allowSceneActivation = false;
			// mainCanvas.SetActive(false);
			loadingMenu.SetActive(true);

			while (!operation.isDone)
			{
				float progress = Mathf.Clamp01(operation.progress / .95f);
				loadingBar.value = progress;
				if (operation.progress >= 0.9f)
				{
					operation.allowSceneActivation = true;
				}
				yield return null;
			}
		}
	}
}
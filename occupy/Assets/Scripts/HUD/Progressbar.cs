using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Progressbar : MonoBehaviour {
	public Image ProgressbarImage;
		
	public void SetProgressAmount(float progressAmount){
		if(ProgressbarImage != null)
			ProgressbarImage.fillAmount = progressAmount / 100;
	}

}

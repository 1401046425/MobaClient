// Blend two profiles based on camera entered into  trigger

using System.Collections;
using UnityEngine;

public class LB_LightingTrigger_Profile : MonoBehaviour
{
    private MobileColorGrading current;
    private MobileColorGrading temp;

    [Tooltip("target profile for blending from current to it - FinalTarget")]
    public MobileColorGrading targetProfile;

    [Tooltip("FinalTarget camera tag for trigger enter and exit")]
    public string cameraTag = "MainCamera";

    [Tooltip("Blend lerp speed * Time.deltaTime")]
    public float blendSpeed = 10f;

    [Tooltip("Update time duration. used for optimization")]
    public float blendDuration = 3f;

    private void Start()
    {
        current = GameObject.FindGameObjectWithTag(cameraTag).GetComponent<MobileColorGrading>();
        temp = new MobileColorGrading();
        temp.Exposure = current.Exposure;
        temp.Contrast = current.Contrast;
        temp.Saturation = current.Saturation;
        temp.Gamma = current.Gamma;
        temp.vignetteIntensity = current.vignetteIntensity;
        temp.R = current.R;
        temp.G = current.G;
        temp.B = current.B;
    }

    private bool isChanging;
    private bool isUpdating;

    private void Update()
    {
        if (!isUpdating)
            return;

        if (isChanging)
        {
            current.Exposure = Mathf.Lerp(current.Exposure, targetProfile.Exposure, Time.deltaTime * blendSpeed);
            current.Contrast = Mathf.Lerp(current.Contrast, targetProfile.Contrast, Time.deltaTime * blendSpeed);
            current.Gamma = Mathf.Lerp(current.Gamma, targetProfile.Gamma, Time.deltaTime * blendSpeed);
            current.Saturation = Mathf.Lerp(current.Saturation, targetProfile.Saturation, Time.deltaTime * blendSpeed);
            current.vignetteIntensity = Mathf.Lerp(current.vignetteIntensity, targetProfile.vignetteIntensity, Time.deltaTime * blendSpeed);
            current.R = Mathf.Lerp(current.R, targetProfile.R, Time.deltaTime * blendSpeed);
            current.G = Mathf.Lerp(current.G, targetProfile.G, Time.deltaTime * blendSpeed);
            current.B = Mathf.Lerp(current.B, targetProfile.B, Time.deltaTime * blendSpeed);
        }
        else
        {
            current.Exposure = Mathf.Lerp(current.Exposure, temp.Exposure, Time.deltaTime * blendSpeed);
            current.Contrast = Mathf.Lerp(current.Contrast, temp.Contrast, Time.deltaTime * blendSpeed);
            current.Gamma = Mathf.Lerp(current.Gamma, temp.Gamma, Time.deltaTime * blendSpeed);
            current.Saturation = Mathf.Lerp(current.Saturation, temp.Saturation, Time.deltaTime * blendSpeed);
            current.vignetteIntensity = Mathf.Lerp(current.vignetteIntensity, temp.vignetteIntensity, Time.deltaTime * blendSpeed);
            current.R = Mathf.Lerp(current.R, temp.R, Time.deltaTime * blendSpeed);
            current.G = Mathf.Lerp(current.G, temp.G, Time.deltaTime * blendSpeed);
            current.B = Mathf.Lerp(current.B, temp.B, Time.deltaTime * blendSpeed);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == cameraTag)
        {
            StopCoroutine("StopUpdating");
            StartCoroutine("StopUpdating");
            isChanging = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == cameraTag)
        {
            StopCoroutine("StopUpdating");
            StartCoroutine("StopUpdating");
            isChanging = false;
        }
    }

    // Stop update function after passing blennd duration in seconds
    private IEnumerator StopUpdating()
    {
        isUpdating = true;
        yield return new WaitForSeconds(blendDuration);
        isUpdating = false;
    }
}
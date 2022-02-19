using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	/*****************
	CreateDate: 
	Functionality:
	Notes:
	Dependencies:
	******************/
	
/// <summary>
/// Lerping curves with different characteristics. Not clamped, only inputs between 0 and 1
/// </summary>
public static class LerpCurves{
    //verify plots by inserting the commented functions into this plotter https://rechneronline.de/function-graphs/
    //Cosinus soft in soft out from 0F to 1F. Coroutine useage example:
    //    float time    = 1F;
    //    float timer    = 0F;
    //    while(timer < time){
    //        float lerpFactor =    LerpCurves.SoftestInSoftOut01(timer/time);
    //        image.color = new Color(1F,1F,1F,lerpFactor); //Do something with lerpFactor e.g. this creates a fade in
    //        yield return null;
    //        timer += Time.deltaTime;
    //    }
    //    image.color = new Color(1F,1F,1F,1F); //fully finish the lerp at the end
 
    public    static float SofterInSofterOut01    ( float valueFrom0to1) { return                    (-Mathf.Cos(valueFrom0to1 * Mathf.PI))*0.5F+0.5F;        }    //    ( cos(x*pi)*0.5+0.5)
    public    static float SofterInSofterOut10    ( float valueFrom0to1) { return    1F -            (-Mathf.Cos(valueFrom0to1 * Mathf.PI))*0.5F+0.5F;        }    //   1-( cos(x*pi)*0.5+0.5)
    public    static float SoftestInSoftOut01        ( float valueFrom0to1) { return        Mathf.Pow(    (-Mathf.Cos(valueFrom0to1 * Mathf.PI))*0.5F+0.5F, 2F);    }    //     (-cos(x*pi)*0.5+0.5)^2
    public    static float SoftestInSoftOut10        ( float valueFrom0to1) { return    1F -Mathf.Pow(    (-Mathf.Cos(valueFrom0to1 * Mathf.PI))*0.5F+0.5F, 2F);    }    //    1-(-cos(x*pi)*0.5+0.5)^2
 
    //pretty much the same as SofterInSofterOut
    public    static float SmoothStep01            ( float valueFrom0to1) { return        valueFrom0to1*valueFrom0to1*(3-2*valueFrom0to1);                    }    //     (x*x * (3 - 2*x))
    public    static float SmoothStep10            ( float valueFrom0to1) { return    1F- valueFrom0to1*valueFrom0to1*(3-2*valueFrom0to1);                    }    //    1-(x*x * (3 - 2*x))
 
    //Symmetric, SoftestInSoftOut is not
    public    static float SmootherStep01            ( float valueFrom0to1) { return        valueFrom0to1*valueFrom0to1*valueFrom0to1*(valueFrom0to1*(6F*valueFrom0to1-15F)+10F);    }    //     x*x*x * (x* (6*x - 15) + 10)
    public    static float SmootherStep10            ( float valueFrom0to1) { return    1F- valueFrom0to1*valueFrom0to1*valueFrom0to1*(valueFrom0to1*(6F*valueFrom0to1-15F)+10F);    }    //    1-x*x*x * (x* (6*x - 15) + 10)
 
    public    static float Linear01                ( float valueFrom0to1) { return        valueFrom0to1;                                                        }    //        x
 
    public    static float SoftInHardOut01        ( float valueFrom0to1) { return        1-Mathf.Pow(valueFrom0to1,3);                                        }    //        x^3
    public    static float SoftInHardOut10        ( float valueFrom0to1) { return        1-Mathf.Pow(valueFrom0to1,3);                                        }    //    1-   x^3
    public    static float HardInSoftOut01        ( float valueFrom0to1) { return        Mathf.Pow(1-valueFrom0to1,3);                                        }    //     (1-x)^3
    public    static float HardInSoftOut10        ( float valueFrom0to1) { return        Mathf.Pow(1-valueFrom0to1,3);                                        }    //    1-(1-x)^3
}
 


//#define PUSH_ENABLED
////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;

public class DeviceTokenListner : MonoBehaviour {
	
	

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public static void Create() {
		 new GameObject ("DeviceTockenListner").AddComponent<DeviceTokenListner> ();
	}


	void Awake() {
		DontDestroyOnLoad (gameObject);

	}


	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	


	  
	 
	#if (UNITY_IPHONE && !UNITY_EDITOR && PUSH_ENABLED) || SA_DEBUG_MODE
	
	private bool tokenSent = false;

	void  FixedUpdate () {

		
		if (!tokenSent) {

			byte[] token   = NotificationServices.deviceToken;
			if(token != null) {

				IOSNotificationDeviceToken t = new IOSNotificationDeviceToken(token);
				IOSNotificationController.instance.OnDeviceTockeReceivedAction (t);
				Destroy (gameObject);
			}
		}

	}

	#endif

	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}

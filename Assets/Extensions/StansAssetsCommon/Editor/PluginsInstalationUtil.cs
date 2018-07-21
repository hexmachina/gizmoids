using UnityEngine;
using UnityEditor;
using System.Collections;

public class PluginsInstalationUtil : MonoBehaviour {
	
	
	public const string ANDROID_SOURCE_PATH       = "Plugins/StansAssets/Android/";
	public const string ANDROID_DESTANATION_PATH  = "Plugins/Android/";
	
	
	public const string IOS_SOURCE_PATH       = "Plugins/StansAssets/IOS/";
	public const string IOS_DESTANATION_PATH  = "Plugins/IOS/";
	
	
	
	
	
	public static void IOS_UpdatePlugin() {
		IOS_InstallPlugin(false);
	}
	
	public static void IOS_InstallPlugin(bool IsFirstInstall = true) {
		FileStaticAPI.CopyFolder(IOS_SOURCE_PATH, IOS_DESTANATION_PATH);
	}
	
	
	public static void Android_UpdatePlugin() {
		Android_InstallPlugin(false);
	}
	
	public static void Android_InstallPlugin(bool IsFirstInstall = true) {
		
		
		
		
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + "libs/android-support-v4.jar", 			ANDROID_DESTANATION_PATH + "libs/android-support-v4.jar");
		
		
		
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + "libs/android-support-v4.jar", 			ANDROID_DESTANATION_PATH + "libs/android-support-v4.jar");
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + "libs/google-play-services.jar", 			ANDROID_DESTANATION_PATH + "libs/google-play-services.jar");
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + "libs/httpclient-4.3.1.jar", 				ANDROID_DESTANATION_PATH + "libs/httpclient-4.3.1.jar");
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + "libs/libGoogleAnalyticsServices.jar", 	ANDROID_DESTANATION_PATH + "libs/libGoogleAnalyticsServices.jar");
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + "libs/signpost-commonshttp4-1.2.1.2.jar", ANDROID_DESTANATION_PATH + "libs/signpost-commonshttp4-1.2.1.2.jar");
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + "libs/signpost-core-1.2.1.2.jar", 		ANDROID_DESTANATION_PATH + "libs/signpost-core-1.2.1.2.jar");
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + "libs/twitter4j-core-3.0.5.jar", 			ANDROID_DESTANATION_PATH + "libs/twitter4j-core-3.0.5.jar");
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + "libs/image-chooser-library-1.3.0.jar", 	ANDROID_DESTANATION_PATH + "libs/image-chooser-library-1.3.0.jar");
		
		
		
		FileStaticAPI.CopyFolder(ANDROID_SOURCE_PATH + "facebook", 			ANDROID_DESTANATION_PATH + "facebook");
		
		
		string file;
		file = "res/values/" + "analytics.xml";
		if(!FileStaticAPI.IsFileExists(ANDROID_DESTANATION_PATH + file)) {
			FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + file, 	ANDROID_DESTANATION_PATH + file);
		}
		
		
		file = "res/values/" + "ids.xml";
		if(!FileStaticAPI.IsFileExists(ANDROID_DESTANATION_PATH + file)) {
			FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + file, 	ANDROID_DESTANATION_PATH + file);
		}
		
		file = "res/xml/" + "file_paths.xml";
		if(!FileStaticAPI.IsFileExists(ANDROID_DESTANATION_PATH + file)) {
			FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + file, 	ANDROID_DESTANATION_PATH + file);
		}
		
		
		file = "res/values/" + "version.xml";
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + file, 	ANDROID_DESTANATION_PATH + file);
		
		file = "androidnative.jar";
		FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + file, 	ANDROID_DESTANATION_PATH + file);
		
		
		
		//First install dependense
		
		file = "AndroidManifest.xml";
		if(!FileStaticAPI.IsFileExists(ANDROID_DESTANATION_PATH + file)) {
			
			FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + file, 	ANDROID_DESTANATION_PATH + file);
			
		} else {
			if(IsFirstInstall) {
				int options = EditorUtility.DisplayDialogComplex(
					"AndroidManifest.xml detected",
					"Looks like you already have AndroidManifest.xml in your project, probably it's part of another android plugin. AndroidManifest.xml is reuired for Androud Native plugin. You can merge or replace Manifest, please choose from options bellow.",
					"Replace",
					"Learn more",
					"Do nothing");
				
				switch(options) {
				case 0:
					FileStaticAPI.CopyFile(ANDROID_SOURCE_PATH + file, 	ANDROID_DESTANATION_PATH + file);
					break;
				case 1:
					Application.OpenURL("http://goo.gl/UX30B3");
					break;
					
				}
			} else {
				
				
			}
		}
		
		AssetDatabase.Refresh();
		
	}
}

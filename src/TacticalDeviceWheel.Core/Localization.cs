namespace TacticalDeviceWheel.Core;

public static class Localization
{
	public static string Pick(UiLanguage language, string english, string chinese)
	{
		if (language != UiLanguage.English)
		{
			return chinese;
		}
		return english;
	}

	public static string Capability(UiLanguage language, string type, string customChinese = null, string customEnglish = null)
	{
		if (language == UiLanguage.English && !string.IsNullOrEmpty(customEnglish))
		{
			return customEnglish;
		}
		if (language != UiLanguage.English && !string.IsNullOrEmpty(customChinese))
		{
			return customChinese;
		}
		return type switch
		{
			"WhiteLight" => Pick(language, "White light", "白光"), 
			"WhiteLightStrobe" => Pick(language, "White-light strobe", "白光爆闪"), 
			"VisibleLaser" => Pick(language, "Visible laser", "可见激光"), 
			"IRLaser" => Pick(language, "IR laser", "IR 激光"), 
			"IRLaserLow" => Pick(language, "IR laser · low", "低功率 IR 激光"), 
			"IRLaserHigh" => Pick(language, "IR laser · high", "高功率 IR 激光"), 
			"IRIlluminator" => Pick(language, "IR illuminator", "IR 补光"), 
			"Rangefinder" => Pick(language, "Rangefinder", "测距"), 
			_ => customEnglish ?? customChinese ?? type, 
		};
	}
}

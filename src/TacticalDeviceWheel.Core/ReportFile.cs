using System.IO;
using System.Text;

namespace TacticalDeviceWheel.Core;

public static class ReportFile
{
	public static void Write(string path, string json)
	{
		path = Path.GetFullPath(path);
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		string text = path + ".tmp";
		try
		{
			File.WriteAllText(text, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			if (File.Exists(path))
			{
				File.Replace(text, path, null);
			}
			else
			{
				File.Move(text, path);
			}
		}
		finally
		{
			if (File.Exists(text))
			{
				File.Delete(text);
			}
		}
	}
}

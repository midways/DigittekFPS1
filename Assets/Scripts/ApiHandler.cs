using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Fysiskverden {
	public class ApiData
	{
		public bool on;
		public uint bri;
		public uint hue;
		public uint sat;
	}
	public class ApiHandler : MonoBehaviour
	{
		const string key = "dLNgnUiTHUMNGz12ATTvVwKWXtvIEXjLBnAddA-H";
		const string ip = "192.168.50.206";
		string url = ip + "/api/" + key + "/lights/";

		/// <summary>
		/// Sender en put request til hue controlleren med farve info og om den er tændt.
		/// </summary>
		/// <param name="lys">Lysets nummer</param>
		/// <param name="state">Om lyset er tændt eller ej. True/False</param>
		/// <param name="hue">Farve af lyset. 0-65535</param>
		/// <param name="brightness">Lysstyrke af lyset. 1-254</param>
		/// <param name="saturation">Saturation af lyset. 0-254</param>
		public void SetColor(uint lys, bool state, uint hue, uint brightness, uint saturation)
		{
			string uri = url + lys + "/state";

			var data = new ApiData();
			data.on = state;
			data.hue = hue;
			data.sat = saturation;
			data.bri = brightness;

			string json = JsonUtility.ToJson(data);
			StartCoroutine(Put(uri, json));
		}

		/// <summary>
		/// Sender en put request til hue controlleren som blinker lyset.
		/// </summary>
		/// <param name="lys">Lysets nummer</param>
		public void Blink(uint lys)
		{
			string uri = url + lys + "/state";
			StartCoroutine(Put(uri, "{\"alert\":\"select\"}"));
		}

		//Selve PUT requesten. 
		public IEnumerator Put(string url, string data)
		{
			using (UnityWebRequest www = UnityWebRequest.Put(url, data))
			{
				yield return www.SendWebRequest();

				if (www.result != UnityWebRequest.Result.Success)
				{
					Debug.Log(www.error);
				}
				else
				{
					Debug.Log("Upload complete!");
				}
			}
		}
	}
}

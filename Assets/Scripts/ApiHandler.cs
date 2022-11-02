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

	public class ApiDataXY
	{
		public bool on;
		public List<float> xy;
	}

	public class xy
	{
		public float x;
		public float y;
	}
	public class ApiHandler : MonoBehaviour
	{
		const string key = "dLNgnUiTHUMNGz12ATTvVwKWXtvIEXjLBnAddA-H";
		const string ip = "192.168.50.206";
		string url = ip + "/api/" + key + "/lights/";

		//ColorGamut værdier fået fra lysapi e.
		float GamutRedX = 0.7f;
		float GamutRedY = 0.3f;
		float GamutGreenX = 0.11f;
		float GamutGreenY = 0.82f;
		float GamutBlueX = 0.12f;
		float GamutBlueY = 0.08f;

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
		//Checker om farven givet er inde for pærens color gamut
		//Meget af koden er konverteret til c# fra ts fra dette repo https://github.com/peter-murray/node-hue-api/blob/081da8354c761d3405ddf240ed53d83e01ee7a6c/src/rgb.ts#L162
		private bool inColorGamut(xy point) {

			float v1x = GamutGreenX - GamutRedX;
			float v1y = GamutGreenY - GamutRedY;
			float v2x = GamutBlueX - GamutRedX;
			float v2y = GamutBlueY - GamutRedY;
			float qx = point.x - GamutRedX;
			float qy = point.y - GamutRedY;

			float s = (qx * v2y) - (qy * v2x) / (v1x * v2y) - (v1y * v2x);
			float t = (v2x * qy) - (v2y * qx) / (v1x * v2y) - (v1y * v2x);

			return (s >= 0.0) && (t >= 0.0) && (s + t <= 1.0);
		}

		//Giver xy værdien til blinkfarve. 
		public void ColorSetup(uint lys, float pointx, float pointy) 
		{
			var _xy = new xy();
			_xy.x = pointx;
			 _xy.y = pointy;
			if(!inColorGamut(_xy)){
				_xy = ResolveXYPoint(_xy);
			}

			BlinkFarve(lys, _xy); 
		}


		//finder det nærmeste fra givet vektorer.
		private xy ClosestPoint(float startx, float starty, float stopx, float stopy, xy point) 
		{
			float APx = point.x - startx;
			float APy = point.y - starty;
			float ABx = stopx - startx;
			float ABy = stopy - starty;

			float ab2 = ABx * ABx + ABy * ABy;
			float ap_ab = APx * ABx + APy * ABy;

			float t = ap_ab / ab2;
			if (t < 0.0f) {
				t = 0.0f;
			} else if (t > 1.0f) {
				t = 1.0f;
			}
			
			var complete = new xy();
			complete.x = startx + ABx * t;
			complete.y = starty + ABy * t;

			return complete;
		}

		//Finder længden mellem to punkter
		private float DistanceBetweenPoints(xy point1, xy point2)
		{
			float dx = point1.x - point2.x;
			float dy = point1.y - point2.y;
			return Mathf.Sqrt(dx * dx + dy * dy);
		}

		//Finder det tætteste punkt i trekanten hvis farven givet er uden for.
		private xy ResolveXYPoint(xy point)
		{
			xy pAB = ClosestPoint(GamutRedX, GamutRedY, GamutGreenX, GamutGreenY, point);
			xy pAC = ClosestPoint(GamutBlueX, GamutBlueY, GamutRedX, GamutRedY, point);
			xy pBC = ClosestPoint(GamutGreenX, GamutGreenY, GamutBlueX, GamutBlueY, point);
			float dAB = DistanceBetweenPoints(point, pAB);
			float dAC = DistanceBetweenPoints(point, pAC);
			float dBC = DistanceBetweenPoints(point, pBC);

			float lowest = dAB;
			xy closestPoint = pAB;

			if (dAC < lowest) {
				lowest = dAC;
				closestPoint = pAC;
			}

			if (dBC < lowest) {
				closestPoint = pBC;
			}

			return closestPoint;
		}

		
		//Blinker farven.
		public void BlinkFarve(uint lys, xy point)
		{
			List<float> xy = new List<float>();
			xy.Add(point.x);
			xy.Add(point.y);
			var data = new ApiDataXY();
			data.on = true;
			data.xy = xy;

			string json = JsonUtility.ToJson(data);
			string uri = url + lys + "/state";
			StartCoroutine(Put(uri, json));
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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoaBonanza.Core
{
	public class FPSDisplay: MonoBehaviour {
		
		public TMPro.TextMeshProUGUI FPSCounter; // inspector
		
		const int SHORTLISTLENGTH = 10;

		int frameCount = 0;
		float dtAccumulator = 0.0f;
		float fps = 0.0f;
		public float updateRate = 2.0f;  // 2 updates per sec.

		float minFPS = 9999999;
		float maxFPS = 0;

		List<float> shortListFPS = new List<float>(SHORTLISTLENGTH);
		List<float> longListFPS = new List<float>();

		public void Update() {
			frameCount++;
			dtAccumulator += Time.deltaTime;
			if (dtAccumulator > 1.0f / updateRate) {
				fps = frameCount / dtAccumulator;
				frameCount = 0;
				dtAccumulator -= 1.0f / updateRate;
				maxFPS = Mathf.Max(maxFPS, fps);
				minFPS = Mathf.Min(minFPS, fps);

				shortListFPS.Add(fps);
				if(shortListFPS.Count >= SHORTLISTLENGTH) {
					longListFPS.Add(shortListFPS.Average());
					shortListFPS.Clear();
				}
			}

			FPSCounter.text = GetCurrentFPS().ToString();
		}

		public int GetCurrentFPS() {
			return Mathf.RoundToInt(fps);
		}

		public int GetMinFPS() {
			return Mathf.RoundToInt(minFPS);
		}

		public int GetMaxFPS() {
			return Mathf.RoundToInt(maxFPS);
		}

		public int GetAverageFPS() {
			if (longListFPS.Count <= 0) {
				return 0;
			}
			return Mathf.RoundToInt(longListFPS.Average());
		}
	}
}

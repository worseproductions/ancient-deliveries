using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AncientDeliveries.scripts;

public static class JobGenerator {
	public static List<string> GetJobs(int jobSize, int addressLength) {
		var random = new Random();
		const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456{|}~";
		var jobs = new List<string>();
		for (var jobIdx = 0; jobIdx < jobSize; jobIdx++) {
			addressLength = Math.Clamp(addressLength, 1, 20);

			var address = new char[addressLength];
			for (var i = 0; i < addressLength; i++) {
				address[i] = alphabet[random.Next(alphabet.Length)];
			}

			jobs.Add(new string(address));
		}

		return jobs;
	}
}
using System;
using System.Collections.Generic;

namespace AncientDeliveries.scripts;

public class JobGenerator
{
    public static List<String> GetJobs(int jobsize, int addressLength)
    {
        var random = new Random();
        const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456";
        List<String> jobs = new List<String>();
        for (int job_idx = 0; job_idx < jobsize; job_idx++)
        {
            addressLength = Math.Clamp(addressLength, 1, 20);

            char[] address = new char[addressLength];
            for (int i = 0; i < addressLength; i++)
            {
                address[i] = alphabet[random.Next(alphabet.Length)];
            }

            jobs.Add(new String(address));
        }
        return jobs;
    }
}
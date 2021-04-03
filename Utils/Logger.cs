using System;
using UnityEngine;

namespace FMenu.Utils
{
    public class Logger
    {
        public Logger()
        {
        }

        public void log(string message, string subprefix = null)
        {
            string output = $"{GetTimestamp(DateTime.Now)} [FMenu Logger";
            if (subprefix != null) output += $" | {subprefix}] {message}";
            else output += $"] {message}";
            Debug.Log(output);
        }

        private static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoDemos
{

    public class EncryptDecrypt 
    {
        private int counter = 0;

        public static void Test()
        {
            string txt = "Simple is better than complex.";
            string key = "supersecretkey";

            string encrypted = EncryptDecrypt.Encrypt(txt, key);
            string decrypted = EncryptDecrypt.Decrypt(encrypted, key);
            var result = (decrypted == txt) ? "Decrypt passed" : "Decrypt failed";
            Console.WriteLine(result);

            /*
            string txt1 = "DiArJTs0JzgjJDYgOj0+Y2U7JiBzPiApNCwxczknIGIrOj04PGUzPDAgOSYqLGhfWW1vZW\r\nJpdXNtb2ViaXVzbW9lYml1c21vZWJpdXNtb2ViaXVzY2hibEN1c21vZWJpe3RqYWViaXVz\r\nbWFlYml1c21vZWhucnltb2VicwoPYhB/Yml1c21hT2JpdXNtb38dFXoMd29lYhYJe2IQZW\r\nJnb31nEBltFn9zbW9/YmYJc3dvZWxue2ljaGtIaXV9amhreGl6D211ZWJpe3xkE2ViaXJp\r\namVlbRV1eW11ZWJue31qYWViZGhpInV4b0N1aRITah1zcn13dX9saXVzbWhlaG5yeW1vZW\r\nJjdXRjaGpsbnUMEWdqHW57dHdoa2VDdWltYBlic3Vpd3V/eGl1c21vbx0Vegxnb2ViaXV+\r\ncG8qYnR4c21gbB5pdXNtaE9iaXJ9Y2hlYm5vaXdoZWJpdXNnb2oeaX9zbW9lYmdyfGNoa2\r\nJpdXRHb2ViaXVzbW9lYml1c21vZWJpdXljYW9iaXVzbW9lYmlvWQ==\r\n";
            string result1 = EncryptDecrypt.Decrypt(txt1, key);
            Console.WriteLine($"Answer = {result1}");
            */

        }

        public static string Encrypt(string txt, string key)
        {
            var txtBytes = Encoding.ASCII.GetBytes(txt);
            var txt1 = Encoding.ASCII.GetString(txtBytes);
            var keyBytes = Encoding.ASCII.GetBytes(key);
            int ctrKey = 0, ctrTxt = 0;
            var resultBytes = new byte[txtBytes.Length];
            foreach(var byt in txtBytes)
            {
                resultBytes[ctrTxt++]  = (byte) (byt ^ keyBytes[ctrKey]);
                ctrKey = (ctrKey == ( keyBytes.Length - 1)) ? 0 : ctrKey + 1;
            }

            return Convert.ToBase64String(resultBytes);
        }

        public static string Decrypt(string txt, string key)
        {
            var txtBytes = Convert.FromBase64String(txt);
            var keyBytes = Encoding.ASCII.GetBytes(key);
            int ctrKey = 0, ctrTxt = 0;
            var resultBytes = new byte[txtBytes.Length];
            foreach (var byt in txtBytes)
            {
                resultBytes[ctrTxt++] = (byte)(byt ^ keyBytes[ctrKey]);
                ctrKey = (ctrKey == (keyBytes.Length - 1)) ? 0 : ctrKey + 1;
            }
            return Encoding.ASCII.GetString(resultBytes);
        }
           

    }


}

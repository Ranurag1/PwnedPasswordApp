using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PwnedPassword
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var isPassworedPwned = HasPasswordBeenPwned(txtPassword.Text.Trim()).Result;
        }

        public async Task<bool> HasPasswordBeenPwned(string password, CancellationToken cancellationToken = default)
        {
           var _client=new HttpClient();
            var sha1Password = SHA1Util.SHA1HashStringForUTF8String(password);
            var sha1Prefix = sha1Password.Substring(0, 5);
            var sha1Suffix = sha1Password.Substring(5);
            try
            {
                lblifException.Text = "";
               
                using (var client = new HttpClient())
                {
                    var url = @"https://api.pwnedpasswords.com/range/" + sha1Prefix;
                    //Passing service base url
                    //client.BaseAddress = new Uri(Baseurl);
                    //client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    var responseTask =   client.GetAsync(url);
                    responseTask.Wait();
                    var response= responseTask.Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var frequency = await Contains(response.Content, sha1Suffix);
                        var isPwned = (frequency >= 1);
                        if (isPwned)
                        {
                            lblMsg.Text = "pwned password";
                        }
                        else
                        {
                            lblMsg.Text = "great password is not pwned!";
                        }
                        return isPwned;
                    }
                  
                     
                } 
            }
            catch (Exception ex)
            {
                lblifException.Text = ex.Message;
            }
            return false;
        }

        internal static async Task<long> Contains(HttpContent content, string sha1Suffix)
        {
            using (var streamReader = new StreamReader(await content.ReadAsStreamAsync()))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = await streamReader.ReadLineAsync();
                    var segments = line.Split(':');
                    if (segments.Length == 2
                        && string.Equals(segments[0], sha1Suffix, StringComparison.OrdinalIgnoreCase)
                        && long.TryParse(segments[1], out var count))
                    {
                        return count;
                    }
                }
            }

            return 0;

        }
        internal static class SHA1Util
        {
            private static readonly SHA1 _sha1 = SHA1.Create();

            /// <summary>
            /// Compute hash for string
            /// </summary>
            /// <param name="s">String to be hashed</param>
            /// <returns>40-character hex string</returns>
            public static string SHA1HashStringForUTF8String(string s)
            {
                byte[] bytes = Encoding.Default.GetBytes(s);

                byte[] hashBytes = _sha1.ComputeHash(bytes);

                return HexStringFromBytes(hashBytes);
            }

            /// <summary>
            /// Convert an array of bytes to a string of hex digits
            /// </summary>
            /// <param name="bytes">array of bytes</param>
            /// <returns>String of hex digits</returns>
            private static string HexStringFromBytes(byte[] bytes)
            {
                var sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    var hex = b.ToString("X2");
                    sb.Append(hex);
                }
                return sb.ToString();
            }
        }
    }
}
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyView
{
	/// <summary>
	/// Handles all requests to and from the Unsplash server.
	/// </summary>
	public class UnsplashAdapter
	{
		#region TYPES
		public enum UriMode
		{
			Absolute, PhotoID
		}
		#endregion
		
		
		#region CONSTANTS
        /// The unique ID of this app 
		const string APP_ID = "5692dd4b4fe6468ed6adbccf3c531466bc8dd8f51676227b54213ea5bbe64d9e";
        const string SECRET = " 9a73c4df2713c2e7577e2b479e9aa9389465114c4b524a6b9de03304f44adbd1";

        const string BASE_API = "http://api.unsplash.com";
        const string BASE_SITE = "https://unsplash.com";
        const string CLIENT_AUTH = "client_id=" + APP_ID;
		#endregion


		#region PROPERTIES
		public static UnsplashAdapter Instance { get { return GetAdapterInstance(); } }
		#endregion


		#region VARIABLES
		private static UnsplashAdapter m_UnsplashAdapter;
        private static HttpClient m_HttpClient = new HttpClient();
        private static WebClient m_WebClient = new WebClient();
        #endregion


        #region PUBLIC API
        /// <summary>
        /// Retrieves a random photo from the server.
        /// Note that a null photo is returned if the call fails.
        /// </summary>
        /// <returns></returns>
        public async Task<UnsplashImage> GetRandomPhotoAsync()
        {
            try
            {
                var response = await m_HttpClient.GetAsync($"{BASE_API}/photos/random/?{CLIENT_AUTH}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(jsonResponse);

                    //dynamic photoObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
                    //UnsplashImage photoObject = Newtonsoft.Json.JsonConvert.DeserializeObject<UnsplashImage>(jsonResponse);

                    return new UnsplashImage();//photoObject;
                }
                else
                {
                    Debug.WriteLine("GetRandomPhoto failed: " + response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"GetRandomPhoto exception: \n{e}");
            }

            return null;
        }
        
        /// <summary>
		/// Retrieves the specified data from the provided URI.
		/// </summary>
		/// <returns>The data at the provided end-point.</returns>
		/// <param name="uri">End-point.</param>
		public static async Task<byte[]> DownloadPhotoAsync(string uri, UriMode mode)
		{
			byte[] data = null;
			
			try
			{
				string finalUri = null;
				switch (mode)
				{
					case UriMode.Absolute:		finalUri = $"{uri}";									break;
					case UriMode.PhotoID:		finalUri = $"{BASE_SITE}/photos/{uri}/download/";		break;
				}

				data = await m_WebClient.DownloadDataTaskAsync(finalUri);
				
				Console.WriteLine($"Photo downloaded. Size = {data.Length} bytes. Location = {finalUri}");
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"Downloading file failed: Exception:\n{e}");
			}

			return data;
		}
        #endregion


        #region HELPERS
        static UnsplashAdapter GetAdapterInstance()
		{
			if (m_UnsplashAdapter == null)
			{
				m_UnsplashAdapter = new UnsplashAdapter();
			}

			return m_UnsplashAdapter; 
		}
		#endregion
	}
}

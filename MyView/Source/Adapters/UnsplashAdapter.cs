using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyView.Adapters
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
        
        const string GENERIC_FAILURE_MESSAGE = "Unsplash could not be reached for images. Are you sure you are online?";
        const string SERVER_ERROR_MESSAGE = "Something went wrong with Unsplash! Please try again.";
        
		#endregion
		
		
		#region EVENTS
		/// <summary>
		/// Is raised when an error occurs. The provided string paramter passes a message that describes the error.
		/// </summary>
		public static  Action<string> OnErrorThrown = delegate {};
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
                Debug.Write("GetRandomPhotoAsync(): Requesting image ... ");
            	
                var response = await m_HttpClient.GetAsync($"{BASE_API}/photos/random/?{CLIENT_AUTH}");

                Debug.WriteLine("Done!");
				
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine("GetRandomPhotoAsync() Server response: " + jsonResponse);
                    
                    var json = LightWeightJsonParser.LWJson.Parse(jsonResponse);
                    //Debug.WriteLine("LWJson :\n" + json.ToString());

                    return new UnsplashImage();
                }
                else
                {
                    Debug.WriteLine("GetRandomPhotoAsync() failed: " + response.Content.ReadAsStringAsync().Result);
                	OnErrorThrown(SERVER_ERROR_MESSAGE);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"GetRandomPhotoAsync() exception: \n{e}");
                OnErrorThrown(GENERIC_FAILURE_MESSAGE);
            }

            return null;
        }
        
        /// <summary>
		/// Retrieves the specified data from the provided URI.
		/// </summary>
		/// <returns>The data at the provided end-point.</returns>
		/// <param name="image">The image to download and return.</param>
		public static async Task<byte[]> DownloadPhotoAsync(UnsplashImage image, UriMode mode)
		{
			byte[] data = null;
			var uri = image.id;
			
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
                OnErrorThrown(GENERIC_FAILURE_MESSAGE);
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

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using LightWeightJsonParser;
using MyView.Unsplash;

namespace MyView.Adapters
{
	/// <summary>
	/// Handles all requests to and from the Unsplash server.
	/// </summary>
	public class UnsplashAdapter
	{
		#region CONSTANTS
        /// The unique ID of this app 
		const string APP_ID = "5692dd4b4fe6468ed6adbccf3c531466bc8dd8f51676227b54213ea5bbe64d9e";
        const string SECRET = " 9a73c4df2713c2e7577e2b479e9aa9389465114c4b524a6b9de03304f44adbd1";

        const string BASE_API = "http://api.unsplash.com";
        const string BASE_SITE = "https://unsplash.com";
        
        const string PARAM_CLIENT_AUTH = "client_id=" + APP_ID;
        
        const string GENERIC_FAILURE_MESSAGE = "Unsplash could not be reached at this time.";
        const string SERVER_ERROR_MESSAGE = "Something went wrong with Unsplash! Please try again later.";
		#endregion
		
		
		#region EVENTS
		/// <summary>
		/// Is raised when an error occurs. The provided string paramter passes a message that describes the error.
		/// </summary>
		public static event Action<string> OnErrorThrown = delegate {};
		#endregion


		#region PROPERTIES
		public static UnsplashAdapter Instance { get { return GetAdapterInstance(); } }
		#endregion


		#region VARIABLES
		private static UnsplashAdapter m_UnsplashAdapter;
        private static HttpClient m_HttpClient = new HttpClient();
        private static WebClient m_WebClient = new WebClient();
        #endregion


        #region PUBLIC API - UNSPLASH REQUESTS
        /// <summary>
        /// Retrieves a random photo from the server.
        /// Note that a null photo is returned if the call fails.
        /// </summary>
        /// <returns></returns>
        public async Task<UnsplashImage> GetRandomPhotoAsync()
        {
        	try
        	{
                var response = await m_HttpClient.GetAsync($"{BASE_API}/photos/random/?{PARAM_CLIENT_AUTH}&orientation=landscape");
				
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var jsonObj = LWJson.Parse(jsonResponse);
					var image = new UnsplashImage();
					image.FromLWJson(jsonObj);

                    return image;
                }
                else
                {
                    Console.WriteLine("GetRandomPhotoAsync() failed: " + response.Content.ReadAsStringAsync().Result);
                	OnErrorThrown(SERVER_ERROR_MESSAGE);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"GetRandomPhotoAsync() exception: \n{e}");
                OnErrorThrown(GENERIC_FAILURE_MESSAGE);
            }

            return null;
        }
        
        /// <summary>
        /// Retrieves a random photo from the server using a query request according to what parameter is provided.
        /// Note that a null photo is returned if the call fails.
        /// Format: <paramref name="queryParam"/> can take the form of a search query, such as "forests".
        /// </summary>
        /// <returns></returns>
        public async Task<UnsplashImage> GetRandomQueryAsync(string queryParam)
        {
            try
            {
                var response = await m_HttpClient.GetAsync($"{BASE_API}/photos/random/?{PARAM_CLIENT_AUTH}&orientation=landscape&query={queryParam}");
				
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var jsonObj = LWJson.Parse(jsonResponse);
					var image = new UnsplashImage();
					image.FromLWJson(jsonObj);

                    return image;
                }
                else
                {
                    Console.WriteLine("GetRandomPhotoAsync() failed: " + response.Content.ReadAsStringAsync().Result);
                	OnErrorThrown(SERVER_ERROR_MESSAGE);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"GetRandomPhotoAsync() exception: \n{e}");
                OnErrorThrown(GENERIC_FAILURE_MESSAGE);
            }

            return null;
        }
		
		/// <summary>
		/// Fetches the list counts for all of Unsplash.
		/// </summary>
		/// <returns>The latest Unsplash statistics.</returns>
		public static async Task<UnsplashStats> GetStats()
		{
			try
            {
				var response = await m_HttpClient.GetAsync($"{BASE_API}/stats/total");
				
				if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var jsonObj = LWJson.Parse(jsonResponse);
                    var stats = new UnsplashStats();
                    stats.FromLWJson(jsonObj);
                    
                    return stats;
                }
                else
                {
                    Console.WriteLine("GetRandomPhotoAsync() failed: " + response.Content.ReadAsStringAsync().Result);
                	OnErrorThrown(SERVER_ERROR_MESSAGE);
                }
			}
            catch (Exception e)
            {
                Console.WriteLine($"GetRandomPhotoAsync() exception: \n{e}");
                OnErrorThrown(GENERIC_FAILURE_MESSAGE);
            }
            
            return null;
		}
        #endregion
        
        
        #region PUBLIC API - HELPERS
        /// <summary>
		/// Retrieves the specified data from the provided URI.
		/// </summary>
		/// <returns>The data at the provided end-point.</returns>
		/// <param name="image">The image to download and return.</param>
		public static async Task<byte[]> DownloadPhotoAsync(UnsplashImage image)
		{
			byte[] data = null;
			var uri = image.id;
			
			try
			{
				data = await m_WebClient.DownloadDataTaskAsync(image.links.download);
				Console.WriteLine($"Photo downloaded. Size = {data.Length} bytes. Location = {image.links.download}");
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
		
		async Task<UnsplashImage> GetRequest(string endpoint)
        {
            try
            {
                var response = await m_HttpClient.GetAsync(endpoint);
				
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var jsonObj = LWJson.Parse(jsonResponse);
					var image = new UnsplashImage();
					image.FromLWJson(jsonObj);

                    return image;
                }
                else
                {
                    Console.WriteLine("Failure: " + response.Content.ReadAsStringAsync().Result);
                	OnErrorThrown(SERVER_ERROR_MESSAGE);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: \n{e}");
                OnErrorThrown(GENERIC_FAILURE_MESSAGE);
            }

            return null;
        }
		#endregion
	}
}

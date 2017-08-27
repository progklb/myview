using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using LightWeightJsonParser;
using MyView.Unsplash;

using UnsplashImageSizes = MyView.Unsplash.UnsplashImage.UnsplashImageSizes;

namespace MyView.Adapters
{
	/// <summary>
	/// Handles all requests to and from the Unsplash server.
	/// </summary>
	public class UnsplashAdapter
	{
		#region TYPES
		/// <summary>
		/// Optional image sizing parameters that can be applied to queries.
		/// </summary>
		public enum SizingParameters
		{
			W1280H720,
			W1600H900,
			W1920H1080,
			Full
		}
		#endregion
		
		
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
		
		/// A custom size that can be specified for image downloads. This is original (full) size by default.
		public static SizingParameters CustomSize { get; set; } = SizingParameters.Full;
		#endregion


		#region VARIABLES
		private static UnsplashAdapter m_UnsplashAdapter;
        private static HttpClient m_HttpClient = new HttpClient();
        private static WebClient m_WebClient = new WebClient();
        #endregion


        #region UNSPLASH REQUESTS
        /// <summary>
        /// Retrieves a random photo from the server. Note that a null object is returned if the call fails.
        /// </summary>
        /// <returns>The parsed server response.</returns>
        public async Task<UnsplashImage> GetRandomPhotoAsync()
        {
            return await RequestImageAsync($"{BASE_API}/photos/random/?{PARAM_CLIENT_AUTH}&orientation=landscape");
        }
        
        /// <summary>
        /// Retrieves a random photo from the server using a query request according to what parameter is provided. Note that a null object is returned if the call fails.
        /// Format: <paramref name="queryParam"/> can take the form of a search query, such as "forests".
        /// </summary>
        /// <returns>The parsed server response.</returns>
        public async Task<UnsplashImage> GetRandomQueryAsync(string queryParam)
        {
           	return await RequestImageAsync($"{BASE_API}/photos/random/?{PARAM_CLIENT_AUTH}&orientation=landscape&query={queryParam}");
        }
        
		/// <summary>
		/// Makes the call to the server using the provided endpoint. If the response is successful, the return JSON is 
		/// parsed to produce an <see cref="UnsplashImage"/> that is returned to the calling function. If there is an error,
		/// null is returned.
		/// </summary>
		/// <returns>The response represented as an <see cref="UnsplashImage"/>.</returns>
		/// <param name="request">Endpoint.</param>
		async Task<UnsplashImage> RequestImageAsync(string request)
        {
        	// TODO Remove this debug
        	string jsonResponse = "";
        	string parsing = "";
        	
        	LWJson.OnItemParsed += (msg) => { parsing += msg; };
        	
            try
            {
            	AddCustomSizeParameter(ref request);
            	
                var response = await m_HttpClient.GetAsync(request);
				
                if (response.IsSuccessStatusCode)
                {
                    jsonResponse = response.Content.ReadAsStringAsync().Result;
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
                
                // TODO Remove this debug
                var msg = jsonResponse + "\n\n\nEXCEPTION:\n" + e + "\n\n\nPARSE PROGRESSION:" + parsing;
                Console.WriteLine($"---------------------------------------------------------------------------\n\n{msg}\n\n---------------------------------------------------------------------------");
            }
            
            LWJson.OnItemParsed = delegate { };

            return null;
        }
        
        /// <summary>
        /// Applies any custom sizing parameters to the query based on <see cref="CustomSize"/>
        /// </summary>
        /// <param name="request">Request.</param>
        void AddCustomSizeParameter(ref string request)
        {
        	switch (CustomSize)
        	{
        		case SizingParameters.W1280H720:
        			request += "&w=1280&h=720";
        			break;
        		case SizingParameters.W1600H900:
        			request += "&w=1600&h=900";
        			break;
        		case SizingParameters.W1920H1080:
        			request += "&w=1920&h=1080";
        			break;
        	}
        }
        #endregion
        
        
        #region PUBLIC API - HELPERS
        /// <summary>
		/// Retrieves the specified data from the provided URI.
		/// </summary>
		/// <returns>The data at the provided end-point.</returns>
		/// <param name="image">The image to download and return.</param>
		public static async Task<byte[]> DownloadPhotoAsync(UnsplashImage image, UnsplashImageSizes customSizeOverride = UnsplashImageSizes.Default)
		{
			byte[] data = null;
			var uri = image.id;
			
			try
			{
				var endpoint = GetSizeEndpoint(image, customSizeOverride);
				data = await m_WebClient.DownloadDataTaskAsync(endpoint);
				
				Console.WriteLine($"Photo downloaded. Size = {data.Length} bytes. Location = {endpoint}");
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"Downloading file failed: Exception:\n{e}");
                OnErrorThrown(GENERIC_FAILURE_MESSAGE);
			}

			return data;
		}
		
		/// <summary>
		/// Returns the specific endpoint corresponding to the custom size override parameter, or if this is default the global setting.
		/// </summary>
		/// <returns>The size endpoint.</returns>
		/// <param name="image">Unsplash image to get download endpoint.</param>
		/// <param name="customSizeOverride">Custom size override.</param>
		static string GetSizeEndpoint(UnsplashImage image, UnsplashImageSizes customSizeOverride)
		{
			switch (customSizeOverride)
			{
				// If we have a size override, download this size instead.
				case UnsplashImageSizes.Full:		return image.urls.full;
				case UnsplashImageSizes.Regular:	return image.urls.regular;
				case UnsplashImageSizes.Small:		return image.urls.small;
				case UnsplashImageSizes.Thumb:		return image.urls.thumb;
				case UnsplashImageSizes.Custom:		return image.urls.custom;
				
				// Otherwise check the global setting and act accordingly.
				case UnsplashImageSizes.Default:
				default:
					return CustomSize == SizingParameters.Full ? image.urls.full : image.urls.custom;
			}
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

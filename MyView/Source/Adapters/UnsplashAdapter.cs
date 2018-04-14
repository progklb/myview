using System;
using System.Collections.Generic;
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
	public sealed class UnsplashAdapter
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
        const string SERVER_ERROR_MESSAGE = "Something went wrong! We'll try again shortly.";
		#endregion
		
		
		#region EVENTS
		/// <summary>
		/// Is raised when an error occurs. The provided string paramter passes a message that describes the error.
		/// </summary>
		public static event Action<string> OnErrorThrown = delegate {};
		#endregion


		#region PROPERTIES
		public static UnsplashAdapter Instance { get => GetAdapterInstance(); }
		
		/// A custom size that can be specified for image downloads. This is original (full) size by default.
		public static SizingParameters CustomSize { get; set; } = SizingParameters.Full;
		/// The number of items that should be returned when requesting a list of items. Minimum = 1, maximum = 30.
		public static int ListCount { get; set; } = 30;
		#endregion


		#region VARIABLES
        private static UnsplashAdapter m_Instance;

        private static HttpClient m_HttpClient = new HttpClient();
        private static WebClient m_WebClient = new WebClient();
        #endregion


        #region PUBLC API - UNSPLASH REQUESTS
        /// <summary>
        /// Retrieves a random photo from the server. Query is optional. Note that a null object is returned if the call fails.
        /// Format: <paramref name="queryParam"/> can take the form of a search query, such as "forests".
        /// </summary>
        /// <returns>The resulting image from the parsed server response.</returns>
        /// <param name="queryParam">The query to add (keywords).</param>
        public async Task<UnsplashImage> GetRandomPhotoAsync(string queryParam = null)
        {
        	var queryString = (queryParam != null ? $"&query={queryParam}" : "");
           	var json = await RequestAsync($"{BASE_API}/photos/random/?{PARAM_CLIENT_AUTH}&orientation=landscape{queryString}");
           	
           	Console.WriteLine($"[{nameof(UnsplashAdapter)}] Random image retrieved. Query = \'{queryParam}\'");
           	
            return json != null ? ToUnsplashImage(json) : null;
        }
        
        /// <summary>
        /// Retrieves a list of random photos from the server. A query can be optionally provided. Note that a null object is returned if the call fails.
        /// The number of photos returned is governed by <see cref="ListCount"/>.
        /// Query format: <paramref name="queryParam"/> can take the form of a search query, such as "forests".
        /// </summary>
        /// <returns>The resulting list of images from the parsed server response.</returns>
		/// <param name="queryParam">The query to add (keywords).</param>
        public async Task<List<UnsplashImage>> GetRandomListAsync(string queryParam = null)
        {
        	var queryString = (queryParam != null ? $"&query={queryParam}" : "");
           	var json = await RequestAsync($"{BASE_API}/photos/random/?{PARAM_CLIENT_AUTH}&orientation=landscape&count={ListCount}{queryString}");
           	
           	List<UnsplashImage> list = null;
           	if (json != null)
           	{
           		list = new List<UnsplashImage>();
		       	foreach (var imgObj in json.AsArray().ArrayData)
		       	{
		       		var img = ToUnsplashImage(imgObj);
		       		if (img != null)
		       		{
		           		list.Add(img);
		       		}
		       	}
           	}
           	
           	var queryInfo = queryParam != null ? $"Query = \'{queryParam}\'" : "";
		    var countInfo = list != null ? $"Count = {list.Count}/{ListCount}." : "";
		    Console.WriteLine($"[{nameof(UnsplashAdapter)}] Random image list retrieved. {countInfo} {queryInfo}");
           	
           	return list;
        }
        #endregion
        
        
        #region UNSPLASH REQUESTS
		/// <summary>
		/// Makes the call to the server using the provided endpoint. If the response is successful, the return JSON is 
		/// parsed to produce an <see cref="UnsplashImage"/> that is returned to the calling function. If there is an error,
		/// null is returned.
		/// </summary>
		/// <returns>The response represented as an <see cref="LWJson"/> object.</returns>
		/// <param name="request">Endpoint.</param>
		async Task<LWJson> RequestAsync(string request)
        {
            try
            {
            	AddCustomSizeParameter(ref request);
            	
                var response = await m_HttpClient.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var jsonObj = LWJson.Parse(jsonResponse);

                    return jsonObj;
                }
                else
                {
                    Console.WriteLine($"[{nameof(UnsplashAdapter)}] Server-side failure: Non-success returned. Response:\n{response.Content.ReadAsStringAsync().Result}");
                	OnErrorThrown(SERVER_ERROR_MESSAGE);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[{nameof(UnsplashAdapter)}] Exception:\n{e}");
                OnErrorThrown(GENERIC_FAILURE_MESSAGE);
            }

            return null;
        }
        
        /// <summary>
        /// Converts a provided <see cref="LWJson"/> object to an <see cref="UnsplashImage"/> object.
        /// </summary>
        /// <param name="json">Json.</param>
		/// <returns>The <see cref="LWJson"/> represented as an <see cref="UnsplashImage"/> object.</returns>
        UnsplashImage ToUnsplashImage(LWJson json)
        {
        	try
        	{
        		var image = new UnsplashImage();
				image.FromLWJson(json);
				return image;
        	}
        	catch (Exception e)
        	{
        		Console.WriteLine($"[{nameof(UnsplashAdapter)}] Failure converting LWJson to UnsplashImage.\n{e}");
                OnErrorThrown(SERVER_ERROR_MESSAGE);
        	}
        	
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
				var endpoint = image.links.download_location;
				data = await m_WebClient.DownloadDataTaskAsync(endpoint);
				
				Console.WriteLine($"[{nameof(UnsplashAdapter)}] Photo downloaded. Size = {data.Length} bytes. Location = {endpoint}");
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"[{nameof(UnsplashAdapter)}] Downloading file failed: Exception:\n{e}");
                OnErrorThrown(GENERIC_FAILURE_MESSAGE);
			}

			return data;
		}
        #endregion


        #region HELPERS
        static UnsplashAdapter GetAdapterInstance()
		{
			if (m_Instance == null)
			{
				m_Instance = new UnsplashAdapter();
			}

			return m_Instance; 
		}
		#endregion
	}
}

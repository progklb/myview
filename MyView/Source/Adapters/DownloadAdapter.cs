using System;
using System.Net;
using System.Threading.Tasks;


namespace MyView
{
	public static class DownloadAdapter
	{
		/// <summary>
		/// Retrieves the specified data from the provided URI.
		/// </summary>
		/// <returns>The data at the provided end-point.</returns>
		/// <param name="url">End-point.</param>
		public static async Task<byte[]> DownloadFileAsync(string url)
		{
			byte[] data;
			using (WebClient client = new WebClient())
			{
				data = await client.DownloadDataTaskAsync(url);
			}

			return data;
		}

		/// <summary>
		/// Downloads the data at the provided end-point and stores it on the device at the specified local path. A callback can be provided which will execute when downloading is complete.
		/// </summary>
		/// <param name="url">End-point.</param>
		/// <param name="localPath">Local path at which to store the downloaded file.</param>
		/// <param name="callback">Callback to execute when downloading completes.</param>
		public static async Task DownloadFileAsync(string url, string localPath, Action callback = null)
		{
			using (WebClient webClient = new WebClient())
			{
				await webClient.DownloadFileTaskAsync(url, localPath);
				
				if (callback != null)
				{
					callback();
				}
			}
		}
	}
}

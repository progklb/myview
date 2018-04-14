using LightWeightJsonParser;
using MyView.Adapters;
using MyView.Unsplash;

namespace MyView
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Sets this object using the specified <see cref="LWJson"/> generated from the server response.
		/// </summary>
		/// <param name="json">Json response to set.</param>
		public static void FromLWJson(this UnsplashImage image, LWJson json)
		{
			// TODO Set the rest of these fields.
			
			image.id = json["id"].AsString();

            image.user.id = json["user"]["id"].AsString();
			image.user.name = json["user"]["name"].AsString();
			image.user.username = json["user"]["username"].AsString();
			
			// Location data is not mandatory, so sanity check.
			if (json.AsObject().ContainsKey("location") && json["location"] != null)
			{
				if (json["location"].AsObject().ContainsKey("city") && json["location"]["city"] != null)
				{
					image.location.city = json["location"]["city"].AsString();		
				}
				
				if (json["location"].AsObject().ContainsKey("country") && json["location"]["country"] != null)
				{
					image.location.country = json["location"]["country"].AsString();		
				}
			}
			
			// "urls" object to be used for hotlinking only.

			image.urls.raw = json["urls"]["raw"].AsString();
			image.urls.full = json["urls"]["full"].AsString();
			image.urls.regular = json["urls"]["regular"].AsString();
			image.urls.small = json["urls"]["small"].AsString();
			image.urls.thumb = json["urls"]["thumb"].AsString();
			
			// Check that this field exists as it will only be present if a custom size was requested.
			if (json["urls"].AsObject().ContainsKey("custom"))
			{
				image.urls.custom = json["urls"]["custom"].AsString();		
			}

			// "links" object to be used for any downloads.
			
			image.links.html = json["links"]["html"].AsString();
			image.links.download = json["links"]["download"].AsString();
			image.links.download_location = json["links"]["download_location"].AsString();
		}
		
		/// <summary>
		/// Sets this object using the specified <see cref="LWJson"/> generated from the server response.
		/// </summary>
		/// <param name="json">Json response to set.</param>
		public static void FromLWJson(this UnsplashStats stats, LWJson json)
		{
			stats.photos = json["photos"].AsInteger();
			stats.photos = json["downloads"].AsInteger();
			stats.photos = json["views"].AsInteger();
			stats.photos = json["likes"].AsInteger();
			stats.photos = json["photographers"].AsInteger();
			stats.photos = json["pixels"].AsInteger();
			stats.photos = json["downloads_per_second"].AsInteger();
			stats.photos = json["views_per_second"].AsInteger();
			stats.photos = json["developers"].AsInteger();
			stats.photos = json["applications"].AsInteger();
			stats.photos = json["requests"].AsInteger();
		}
	}
}

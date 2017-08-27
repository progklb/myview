using System.IO;

using MyView.Adapters;

namespace MyView.Additional
{
	/// <summary>
	/// Represents a category that the user can select to display.
	/// </summary>
	public class SlideshowCategory
	{
		#region PROPERTIES
		/// The on-screen name of this category
		public string DisplayName { get; set; }
		/// The query parameter for the server call when requesting images.
		public string QueryString { get; set; }
		
		/// The mode that will be used to display this category.
		public SlideshowAdapter.SlideshowModes SlideshowMode { get; set; }
		
		/// Image to display in category select.
		public string PreviewPath { get { return $"{Path.Combine(Constants.Images.PlaceholderPhotoPath, DisplayName)}.jpg"; } }
		#endregion
		
		
		#region OPERATOR OVERRIDES
		public override bool Equals(object obj)
		{
			return obj is SlideshowCategory && this == (SlideshowCategory)obj;
		}
		
		public override int GetHashCode()
		{
			return DisplayName.GetHashCode() ^ QueryString.GetHashCode() ^ SlideshowMode.GetHashCode();
		}
		
		public static bool operator ==(SlideshowCategory x, SlideshowCategory y)
		{
			if (((object)x != null) && ((object)y != null))
			{
				return 
					x.DisplayName == y.DisplayName && 
					x.QueryString == y.QueryString && 
					x.SlideshowMode == y.SlideshowMode;
			}
			else
			{
				return (((object)x == null) && ((object)y == null));
			}
		}
		
		public static bool operator !=(SlideshowCategory x, SlideshowCategory y)
		{
			return !(x == y);
		}
		#endregion
	}
}

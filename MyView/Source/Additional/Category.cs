namespace MyView.Additional
{
	/// <summary>
	/// Represents a category that the user can select to display.
	/// </summary>
	public class Category
	{
		#region PROPERTIES
		/// The on-screen name of this category
		public string DisplayName { get; set; }
		/// The query parameter for the server call when requesting images.
		public string QueryString { get; set; }
		/// Image to display in category select.
		public string PreviewPath { get; set; }
		#endregion
		
		
		//Note: We are only worrying about the display name and query strying - the preview image path is negligible.
		#region OPERATOR OVERRIDES
		public override bool Equals(object obj)
		{
			return obj is Category && this == (Category)obj;
		}
		
		public override int GetHashCode()
		{
			return DisplayName.GetHashCode() ^ QueryString.GetHashCode();
		}
		
		public static bool operator ==(Category x, Category y)
		{
			return x.DisplayName == y.DisplayName && x.QueryString == y.QueryString;
		}
		
		public static bool operator !=(Category x, Category y)
		{
			return !(x == y);
		}
		#endregion
	}
}

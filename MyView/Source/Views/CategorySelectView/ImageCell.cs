using System;

using CoreGraphics;
using Foundation;
using UIKit;

namespace MyView.Views
{
    public partial class ImageCell : UICollectionViewCell
    {
    	#region PROPERTIES
    	/// The reuse identifier for the collection view cells.
    	public static string CellIdentifier { get { return nameof(ImageCell); } }
    	
    	/// The category which this cell represents.
    	public string Category { get; private set; }
    	#endregion
    	
    	
    	#region VARIABLES
    	/// The view that will display the image of this category.
        private UIImageView m_UIImageViewImage { get; set; }
    	#endregion
    	
    	
    	#region CONSTRUCTOR
        public ImageCell(IntPtr handle) : base (handle) { }

        [Export("initWithFrame:")]
		public ImageCell(CGRect frame) : base (frame)
		{
			// Bug: Storyboard design doesn't get loaded. Let's do this the old-fashioned way.
			
			// Calculate image constraints slightly smaller than the cell.
			var cornerX = frame.Width / 8;
			var cornerY = frame.Height / 8;
			var width = frame.Width - (2 * cornerX);
			var height = frame.Height - (2 * cornerY);

			// Set photo frame size, and load a placeholder to indicate that this cell does not yet represent a photo.
			m_UIImageViewImage = new UIImageView(UIImage.FromFile(Constants.Images.PlaceholderPhoto));
			m_UIImageViewImage.Frame = new CGRect(cornerX, cornerY, width, height);
			m_UIImageViewImage.ContentMode = UIViewContentMode.ScaleAspectFill;
			
			// Allow Focus engine to display focused affects.
			m_UIImageViewImage.AdjustsImageWhenAncestorFocused = true;
			
			ContentView.AddSubview(m_UIImageViewImage);
		}
        #endregion
        
        
        #region PUBLIC API
        public void SetCell(string name, UIImage image = null)
        {
        	Category = name;
        	
        	if (image != null)
        	{
	        	m_UIImageViewImage.Image = image;
        	}
        }
        #endregion
    }
}
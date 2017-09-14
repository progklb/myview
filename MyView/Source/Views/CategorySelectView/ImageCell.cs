using System;

using CoreGraphics;
using Foundation;
using UIKit;

using MyView.Additional;

namespace MyView.Views
{
    public partial class ImageCell : UICollectionViewCell
    {
    	#region PROPERTIES
    	/// The reuse identifier for the collection view cells.
    	public static string CellIdentifier { get { return nameof(ImageCell); } }
    	
    	/// The category which this cell represents.
    	public SlideshowCategory Category { get; private set; }
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
			var cornerX = frame.Width / 16;
			var cornerY = frame.Height / 16;
			var width = frame.Width - (2 * cornerX);
			var height = frame.Height - (2 * cornerY);

			// Set photo frame size, and load a placeholder to indicate that this cell does not yet represent a photo.
			m_UIImageViewImage = new UIImageView();
			m_UIImageViewImage.Frame = new CGRect(cornerX, cornerY, width, height);
			m_UIImageViewImage.ContentMode = UIViewContentMode.ScaleAspectFit;

			// Add corner radii to images. This doesn't work as expected with tvOS.
			//m_UIImageViewImage.Layer.CornerRadius = 10f;
			//m_UIImageViewImage.Layer.MasksToBounds = true;
			
			m_UIImageViewImage.Layer.ShadowOpacity = 0.5f;
			m_UIImageViewImage.Layer.ShadowRadius = 15f;
			m_UIImageViewImage.Layer.ShadowOffset = new CGSize(10f, 20f);
			
			// Allow Focus engine to display focused affects.
			m_UIImageViewImage.AdjustsImageWhenAncestorFocused = true;
			
			ContentView.AddSubview(m_UIImageViewImage);
		}
        #endregion
        

        #region PUBLIC API
        public void SetCell(SlideshowCategory category)
        {
        	Category = category;

	        m_UIImageViewImage.Image = UIImage.FromFile(category.PreviewPath);
	        
	        if (m_UIImageViewImage.Image == null)
	        {
	        	m_UIImageViewImage.Image = UIImage.FromFile(Constants.Images.PlaceholderPhoto);
	        }
        }
        #endregion
    }
}
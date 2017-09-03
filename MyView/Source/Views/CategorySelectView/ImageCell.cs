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
    	
    	/// This returns the override if one has been assigned, otherwise returns the natural behaviour.
    	public override UIView PreferredFocusedView
		{
			get
			{
				if (g_FocusOverride != null)
				{
					return g_FocusOverride;
				}
				else
				{
					return base.PreferredFocusedView;
				}
			}
		}
    	#endregion
    	
    	
    	#region VARIABLES
    	/// Points to the cell that should be focused upon.
		private static ImageCell g_FocusOverride;
    	
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
			m_UIImageViewImage.ContentMode = UIViewContentMode.ScaleAspectFill;

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
        
        /// <summary>
        /// Sets this cell to take precedence when the focus engine attempts to update focus.
        /// This cell will be returned instead of the next natural selection.
        /// </summary>
        public void SetWillOverrideFocus()
        {
        	g_FocusOverride = this;
        }
        
        /// <summary>
        /// Unsets any focus override that has been set.
        /// Note that this can be called on any cell - calling this on the original overriden cell is not necessary.
        /// </summary>
        public static void UnsetWillOverrideFocus()
        {
        	g_FocusOverride = null;
        }
        #endregion
    }
}
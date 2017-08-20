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
    	#endregion
    	
    	
    	#region CONSTRUCTOR
        public ImageCell(IntPtr handle) : base (handle)
        {
        }
        #endregion
        
        
        #region PUBLIC API
        public void SetCell(string name, UIImage image = null)
        {
        	UILabelName.Text = name;
        	
        	if (image != null)
        	{
	        	UIImageViewImage.Image = image;
        	}
        }
        #endregion
    }
}
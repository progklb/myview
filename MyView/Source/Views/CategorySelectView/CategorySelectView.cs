using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace MyView.Views
{
	/// <summary>
	/// The bottom footer bar that is displayed as an overlay allowing the user to
	/// select an image category to view.
	/// </summary>
    public partial class CategorySelectView : BaseView
    {
    	#region CONSTTRUCTOR
        public CategorySelectView(IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region INHERITED METHODS
        public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			
			var categoriesList = new List<string>();
			categoriesList.AddRange(Constants.Slideshow.Categories);
			categoriesList.Add(Constants.Slideshow.Random);
			
			UICollectionCategories.RegisterClassForCell(typeof(ImageCell), new NSString(ImageCell.CellIdentifier));
			UICollectionCategories.Source = new CategorySelectSource(categoriesList);
			
	        // TODO Set the first cell as the focused cell when launching the View.
			// view.PreferredFocusedView = true;
		}
        #endregion
    }
}
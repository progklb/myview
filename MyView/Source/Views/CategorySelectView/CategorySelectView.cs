using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using MyView.Adapters;

namespace MyView.Views
{
	/// <summary>
	/// The bottom footer bar that is displayed as an overlay allowing the user to
	/// select an image category to view.
	/// </summary>
    public partial class CategorySelectView : BaseView
    {
    	#region PROPERTIES
    	/// The slidesho that this select view will manipulate.
    	public SlideshowController Slideshow { get; set; }
    	#endregion
    	
    	
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
			
			(UICollectionCategories.Source as CategorySelectSource).SetItemSelectedCallback(OnCategorySelect);
			(UICollectionCategories.Source as CategorySelectSource).SetItemFocusedCallback(OnCategorySelect);
			
	        // TODO Set the first cell as the focused cell when launching the View.
			// view.PreferredFocusedView = true;
		}
        #endregion
        
        
        #region HELPERS
        void OnCategorySelect(string category)
        {
        	if (category == Constants.Slideshow.Random)
        	{
        		Console.WriteLine("Slideshow mode changed: Random");
	        	Slideshow.SetSlideshowMode(SlideshowController.SlideshowModes.Random);
        	}
        	else
        	{
        		Console.WriteLine($"Slideshow mode changed: Query:{category}");
	        	Slideshow.SetSlideshowMode(SlideshowController.SlideshowModes.RandomQuery, category);
        	}
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using MyView.Adapters;
using MyView.Additional;

namespace MyView.Views
{
	/// <summary>
	/// The bottom footer bar that is displayed as an overlay allowing the user to
	/// select an image category to view.
	/// </summary>
    public partial class CategorySelectView : BaseView
    {
    	#region PROPERTIES
    	/// The slideshow that this select view will manipulate.
    	public SlideshowController Slideshow { get; set; }
    	#endregion
    	
    	
    	#region VARIABLES
    	/// Callback to invoke when a category item is selected.
    	private Action m_OnItemSelectedCallback;
    	
    	/// Callback to invoke when a category item is focused on.
    	private Action m_OnItemFocusCallback;
    	#endregion
    	
    	
    	#region CONSTRUCTOR
        public CategorySelectView(IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region INHERITED METHODS
        public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			
			var categoriesList = new List<Category>();
			categoriesList.AddRange(Constants.Slideshow.Categories);
			categoriesList.Add(Constants.Slideshow.Random);
			
			UICollectionCategories.RegisterClassForCell(typeof(ImageCell), new NSString(ImageCell.CellIdentifier));
			UICollectionCategories.Source = new CategorySelectSource(categoriesList);
			
			(UICollectionCategories.Source as CategorySelectSource).SetItemSelectedCallback(OnItemSelected);
			(UICollectionCategories.Source as CategorySelectSource).SetItemFocusedCallback(OnItemFocused);
			
	        // TODO Set the first cell as the focused cell when launching the View.
			// For reference: view.PreferredFocusedView = true;
		}
        #endregion
        
        
        #region PUBLIC API
        /// <summary>
        /// Sets a callback to be invoked when the a category is selected.
        /// To unset, provide a null callback.
        /// </summary>
        /// <param name="callback">Callback to invoke.</param>
        public void SetItemSelectedCallback(Action callback)
        {
        	m_OnItemSelectedCallback = callback;
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the a category is focused on.
        /// To unset, provide a null callback.
        /// </summary>
        /// <param name="callback">Callback to invoke.</param>
        public void SetItemFocusedCallback(Action callback)
        {
        	m_OnItemFocusCallback = callback;
        }
        #endregion
        
        
        #region HELPERS
        void OnItemSelected(Category category)
        {
        	OnCategorySelect(category);
        	
        	if (m_OnItemSelectedCallback != null)
        	{
        		m_OnItemSelectedCallback();
        	}
        }
        
        void OnItemFocused(Category category)
        {
        	OnCategorySelect(category);
        	
        	if (m_OnItemFocusCallback != null)
        	{
        		m_OnItemFocusCallback();
        	}
        	
        	UILabelCategory.Text = category.DisplayName;
        }
        
        void OnCategorySelect(Category category)
        {
        	if (category == Constants.Slideshow.Random)
        	{
        		Console.WriteLine("Slideshow mode changed: Random");
	        	Slideshow.SetSlideshowMode(SlideshowController.SlideshowModes.Random);
        	}
        	else
        	{
        		Console.WriteLine($"Slideshow mode changed: Query:{category}");
	        	Slideshow.SetSlideshowMode(SlideshowController.SlideshowModes.RandomQuery, category.QueryString);
        	}
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;

using Foundation;

using MyView.Additional;

namespace MyView.Views
{
	/// <summary>
	/// The bottom footer bar that is displayed as an overlay allowing the user to
	/// select an image category to view.
	/// </summary>
    public partial class CategorySelectView : BaseView
    {
    	#region VARIABLES
    	/// Callback to invoke when a category item is selected.
    	private List<Action<SlideshowCategory>> m_OnItemSelectedCallback = new List<Action<SlideshowCategory>>();
    	/// Callback to invoke when a category item is focused on.
    	private List<Action<SlideshowCategory>> m_OnItemFocusCallback = new List<Action<SlideshowCategory>>();
    	#endregion
    	
    	
    	#region CONSTRUCTOR
        public CategorySelectView(IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region INHERITED METHODS
        public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			
			var categoriesList = new List<SlideshowCategory>();
			categoriesList.Add(Constants.Slideshow.Random);
			categoriesList.AddRange(Constants.Slideshow.Categories);
			
			UICollectionCategories.RegisterClassForCell(typeof(ImageCell), new NSString(ImageCell.CellIdentifier));
			UICollectionCategories.Source = new CategorySelectSource(categoriesList);
			
			(UICollectionCategories.Source as CategorySelectSource).SetItemSelectedCallback(OnItemSelected);
			(UICollectionCategories.Source as CategorySelectSource).SetItemFocusedCallback(OnItemFocused);
		}
        #endregion
        
        
        #region PUBLIC API
        /// <summary>
        /// Sets a callback to be invoked when the a category is selected.
        /// </summary>
        /// <param name="callback">Callback to invoke.</param>
        public void AddItemSelectedCallback(Action<SlideshowCategory> callback)
        {
        	m_OnItemSelectedCallback.Add(callback);
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the a category is focused on.
        /// </summary>
        /// <param name="callback">Callback to invoke.</param>
        public void AddItemFocusedCallback(Action<SlideshowCategory> callback)
        {
        	m_OnItemFocusCallback.Add(callback);
        }
        #endregion
        
        
        #region HELPERS
       /// <summary>
       /// Is called when a category item is selected.
       /// </summary>
       /// <param name="category">Category selected.</param>
        void OnItemSelected(SlideshowCategory category)
        {
        	foreach (var callback in m_OnItemSelectedCallback)
        	{
        		callback(category);
        	}
        }
        
        /// <summary>
        /// Is called when a category item is focused on.
        /// </summary>
        /// <param name="category">Category focused on.</param>
        void OnItemFocused(SlideshowCategory category)
        {
        	foreach (var callback in m_OnItemFocusCallback)
        	{
        		callback(category);
        	}
        	
        	UILabelCategory.Text = category.DisplayName;
        }
        #endregion
    }
}
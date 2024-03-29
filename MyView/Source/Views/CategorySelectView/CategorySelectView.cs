using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using CoreGraphics;

using MyView.Additional;

namespace MyView.Views
{
	/// <summary>
	/// The bottom footer bar that is displayed as an overlay allowing the user to
	/// select an image category to view.
	/// </summary>
    public partial class CategorySelectView : AdvancedBaseView
    {
    	#region PROPERTIES
    	public CategorySelectSource Source { get { return UICollectionCategories.Source as CategorySelectSource; } }
    	
    	/// The last item selected by the user.
		public SlideshowCategory LastSelectedItem { get; private set; }
		/// The last item focused on by the user.
		public SlideshowCategory LastFocusedItem { get; private set; }
    	#endregion
    	
    	
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
			
            // Add random as first item
			var categoriesList = new List<SlideshowCategory>();
			categoriesList.Add(Constants.Slideshow.Random);

            // Add all other categories, first sorting alphabetically by display name
            var categories = Constants.Slideshow.Categories.OrderBy(cat => cat.DisplayName);
            categoriesList.AddRange(categories);
			
			UICollectionCategories.RegisterClassForCell(typeof(ImageCell), new NSString(ImageCell.CellIdentifier));
			UICollectionCategories.Source = new CategorySelectSource(categoriesList);
			
			Source.SetItemSelectedCallback(OnItemSelected);
			Source.SetItemFocusedCallback(OnItemFocused);
		}
		
		public override void AnimateIn()
		{
            base.AnimateIn();

            // Start displaying the last category we had selected after the duration of the animation.
            ExecuteAsync(
                () => { 
        			if (LastFocusedItem != null)
        			{
        				OnItemFocused(LastFocusedItem);
        			}
        		},
                (int)(AnimateInDuration * 1000)
			);
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
        
        /// <summary>
        /// Sets the category text that will be displayed. 
		/// Note that this text will be replaced on item focus/select to the newly focused/selected item.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void SetCategoryText(string text)
        {
       		UILabelCategory.Text = text;
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
        	
        	LastSelectedItem = category;
        	
        	Console.WriteLine("SELECTED : {0}", category.DisplayName);
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
        	
        	SetCategoryText(category.DisplayName);
        	LastFocusedItem = category;
        	
        	Console.WriteLine("FOCUSED : {0}", category.DisplayName);
        }
        #endregion
    }
}
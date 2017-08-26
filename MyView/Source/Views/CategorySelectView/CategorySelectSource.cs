using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using MyView.Additional;

namespace MyView.Views
{
	/// <summary>
	/// The source for the <see cref="CategorySelectView"/>, providing data to display.
	/// </summary>
	public class CategorySelectSource : UICollectionViewSource
	{
		#region VARIABLES
		/// Holds the data to display.
		private List<Category> m_Items;

		/// The callback raised when an item is focused.		
		private Action<Category> m_ItemFocusedCallback;
		/// The callback raised when an item is selected.		
		private Action<Category> m_ItemSelectedCallback;
		#endregion
		
		
		#region CONSTRUCTOR
		public CategorySelectSource(List<Category> items = null)
		{
			if (items != null)
			{
				m_Items = items;
			}
			else
			{
				m_Items = new List<Category>();
			}
		}
		#endregion
		
		
		#region INHERITED METHODS
		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return m_Items.Count;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var imageCell = (ImageCell)collectionView.DequeueReusableCell(ImageCell.CellIdentifier, indexPath);
			imageCell.SetCell(m_Items[indexPath.Row]);
			
			return imageCell;
		}
		
		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			m_ItemSelectedCallback(m_Items[indexPath.Row]);
		}
		
		public override void DidUpdateFocus(UICollectionView collectionView, UICollectionViewFocusUpdateContext context, UIFocusAnimationCoordinator coordinator)
		{
			// Ensure that the item is not null. This can happen if this view recieves a focus update but is not currently in focus itself. (?)
			if (context.NextFocusedIndexPath != null)
			{
				m_ItemFocusedCallback(m_Items[context.NextFocusedIndexPath.Row]);
			}
		}
		#endregion
		
		
		#region PUBLIC API
		/// <summary>
		/// Sets a callback that will be invoked when a <see cref="Category"/> item is selected.
		/// </summary>
		/// <param name="callback">Callback to invoke.</param>
		public void SetItemSelectedCallback(Action<Category> callback)
		{
			m_ItemSelectedCallback = callback;
		}
		
		/// <summary>
		/// Sets a callback that will be invoked when a <see cref="Category"/> item is focused on.
		/// </summary>
		/// <param name="callback">Callback to invoke.</param>
		public void SetItemFocusedCallback(Action<Category> callback)
		{
			m_ItemFocusedCallback = callback;
		}
		#endregion
	}
}

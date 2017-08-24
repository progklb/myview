using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

using MyView;

namespace MyView.Views
{
	/// <summary>
	/// The source for the <see cref="CategorySelectView"/>, providing data to display.
	/// </summary>
	public class CategorySelectSource : UICollectionViewSource
	{
		#region VARIABLES
		/// Holds the data to display.
		private List<string> m_Items;

		/// The callback raised when an item is focused.		
		private Action<string> m_ItemFocusedCallback;
		/// The callback raised when an item is selected.		
		private Action<string> m_ItemSelectedCallback;
		#endregion
		
		
		#region CONSTRUCTOR
		public CategorySelectSource(List<string> items = null)
		{
			if (items != null)
			{
				m_Items = items;
			}
			else
			{
				m_Items = new List<string>();
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
			m_ItemFocusedCallback(m_Items[context.NextFocusedIndexPath.Row]);
		}
		#endregion
		
		
		#region PUBLIC API
		public void SetItemSelectedCallback(Action<string> callback)
		{
			m_ItemSelectedCallback = callback;
		}
		
		public void SetItemFocusedCallback(Action<string> callback)
		{
			m_ItemFocusedCallback = callback;
		}
		#endregion
	}
}

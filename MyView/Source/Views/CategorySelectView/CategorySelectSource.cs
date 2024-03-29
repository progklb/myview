﻿using System;
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
		private List<SlideshowCategory> m_Items;

		/// The callback raised when an item is focused.		
		private Action<SlideshowCategory> m_ItemFocusedCallback;
		/// The callback raised when an item is selected.		
		private Action<SlideshowCategory> m_ItemSelectedCallback;
		
		/// Indicates whether or not the first-ever focus has taken place.
		/// This allows us to direct the focus to the first item of the list at the start (instead of the last, as it does by default).
		private bool m_InitialFocusComplete;
		#endregion
		
		
		#region CONSTRUCTOR
		public CategorySelectSource(List<SlideshowCategory> items = null)
		{
			if (items != null)
			{
				m_Items = items;
			}
			else
			{
				m_Items = new List<SlideshowCategory>();
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
			
			// Set index 0 to be the first item we focus on when this category select opens.
			if (!m_InitialFocusComplete && indexPath.Row == 0)
			{
				FocusTarget.SetFocusTarget(imageCell);
			}
			
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
			
			// Unset our focus override as the user has now manually changed focus.
			m_InitialFocusComplete = true;
			FocusTarget.UnsetFocusTarget();
		}
		#endregion
		
		
		#region PUBLIC API
		/// <summary>
		/// Sets a callback that will be invoked when a <see cref="SlideshowCategory"/> item is selected.
		/// </summary>
		/// <param name="callback">Callback to invoke.</param>
		public void SetItemSelectedCallback(Action<SlideshowCategory> callback)
		{
			m_ItemSelectedCallback = callback;
		}
		
		/// <summary>
		/// Sets a callback that will be invoked when a <see cref="SlideshowCategory"/> item is focused on.
		/// </summary>
		/// <param name="callback">Callback to invoke.</param>
		public void SetItemFocusedCallback(Action<SlideshowCategory> callback)
		{
			m_ItemFocusedCallback = callback;
		}
		#endregion
	}
}

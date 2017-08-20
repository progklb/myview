using System;

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
		#region INHERITED METHODS
		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return 1;//Constants.Categories.List.Length;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var imageCell = (ImageCell)collectionView.DequeueReusableCell(ImageCell.CellIdentifier, indexPath);
			//imageCell.SetCell(Constants.Categories.List[indexPath.Row]);
			
			return imageCell;
		}
		#endregion
	}
}

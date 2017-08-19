// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MyView.Views
{
    [Register ("CategorySelectView")]
    partial class CategorySelectView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView UICollectionCategories { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UILabelCategory { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UIViewBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UIViewBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UIViewDetails { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIVisualEffectView UIVisualEffectBlur { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (UICollectionCategories != null) {
                UICollectionCategories.Dispose ();
                UICollectionCategories = null;
            }

            if (UILabelCategory != null) {
                UILabelCategory.Dispose ();
                UILabelCategory = null;
            }

            if (UIViewBackground != null) {
                UIViewBackground.Dispose ();
                UIViewBackground = null;
            }

            if (UIViewBar != null) {
                UIViewBar.Dispose ();
                UIViewBar = null;
            }

            if (UIViewDetails != null) {
                UIViewDetails.Dispose ();
                UIViewDetails = null;
            }

            if (UIVisualEffectBlur != null) {
                UIVisualEffectBlur.Dispose ();
                UIVisualEffectBlur = null;
            }
        }
    }
}
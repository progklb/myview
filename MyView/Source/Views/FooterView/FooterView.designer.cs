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
    [Register ("FooterView")]
    partial class FooterView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UIImageFavourite { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UILabelAuthorHandle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UILabelAuthorName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UILabelLocationCity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UILabelLocationCountry { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UIViewGradient { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (UIImageFavourite != null) {
                UIImageFavourite.Dispose ();
                UIImageFavourite = null;
            }

            if (UILabelAuthorHandle != null) {
                UILabelAuthorHandle.Dispose ();
                UILabelAuthorHandle = null;
            }

            if (UILabelAuthorName != null) {
                UILabelAuthorName.Dispose ();
                UILabelAuthorName = null;
            }

            if (UILabelLocationCity != null) {
                UILabelLocationCity.Dispose ();
                UILabelLocationCity = null;
            }

            if (UILabelLocationCountry != null) {
                UILabelLocationCountry.Dispose ();
                UILabelLocationCountry = null;
            }

            if (UIViewGradient != null) {
                UIViewGradient.Dispose ();
                UIViewGradient = null;
            }
        }
    }
}
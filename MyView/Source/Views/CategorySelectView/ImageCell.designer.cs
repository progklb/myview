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
    [Register ("ImageCell")]
    partial class ImageCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UIImageViewImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UILabelName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (UIImageViewImage != null) {
                UIImageViewImage.Dispose ();
                UIImageViewImage = null;
            }

            if (UILabelName != null) {
                UILabelName.Dispose ();
                UILabelName = null;
            }
        }
    }
}
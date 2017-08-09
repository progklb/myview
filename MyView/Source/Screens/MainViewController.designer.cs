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

namespace MyView
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UIImageBackground1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UIImageBackground2 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (UIImageBackground1 != null) {
                UIImageBackground1.Dispose ();
                UIImageBackground1 = null;
            }

            if (UIImageBackground2 != null) {
                UIImageBackground2.Dispose ();
                UIImageBackground2 = null;
            }
        }
    }
}
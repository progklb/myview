// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MyView.Views
{
    [Register ("HeaderView")]
    partial class HeaderView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UILabelCategory { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UIViewGradient { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (UILabelCategory != null) {
                UILabelCategory.Dispose ();
                UILabelCategory = null;
            }

            if (UIViewGradient != null) {
                UIViewGradient.Dispose ();
                UIViewGradient = null;
            }
        }
    }
}
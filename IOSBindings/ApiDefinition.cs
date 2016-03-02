using System;
using System.Drawing;

using ObjCRuntime;
using Foundation;
using UIKit;

namespace IOSBindings
{
    // The first step to creating a binding is to add your native library ("libNativeLibrary.a")
    // to the project by right-clicking (or Control-clicking) the folder containing this source
    // file and clicking "Add files..." and then simply select the native library (or libraries)
    // that you want to bind.
    //
    // When you do that, you'll notice that MonoDevelop generates a code-behind file for each
    // native library which will contain a [LinkWith] attribute. VisualStudio auto-detects the
    // architectures that the native library supports and fills in that information for you,
    // however, it cannot auto-detect any Frameworks or other system libraries that the
    // native library may depend on, so you'll need to fill in that information yourself.
    //
    // Once you've done that, you're ready to move on to binding the API...
    //
    //
    // Here is where you'd define your API definition for the native Objective-C library.
    //
    // For example, to bind the following Objective-C class:
    //
    //     @interface Widget : NSObject {
    //     }
    //
    // The C# binding would look like this:
    //
    //     [BaseType (typeof (NSObject))]
    //     interface Widget {
    //     }
    //
    // To bind Objective-C properties, such as:
    //
    //     @property (nonatomic, readwrite, assign) CGPoint center;
    //
    // You would add a property definition in the C# interface like so:
    //
    //     [Export ("center")]
    //     CGPoint Center { get; set; }
    //
    // To bind an Objective-C method, such as:
    //
    //     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
    //
    // You would add a method definition to the C# interface like so:
    //
    //     [Export ("doSomething:atIndex:")]
    //     void DoSomething (NSObject object, int index);
    //
    // Objective-C "constructors" such as:
    //
    //     -(id)initWithElmo:(ElmoMuppet *)elmo;
    //
    // Can be bound as:
    //
    //     [Export ("initWithElmo:")]
    //     IntPtr Constructor (ElmoMuppet elmo);
    //
    // For more information, see http://docs.xamarin.com/ios/advanced_topics/binding_objective-c_types
    //
    [Model, BaseType(typeof(NSObject)), Protocol]
    public interface MLPAutoCompleteTextFieldDataSource
    {
        [Export("autoCompleteTextField:possibleCompletionsForString:"),
            DelegateName("PossibleCompletionsForStringDelegate"),
            DefaultValue(null)]
        MLPAutoCompletionObject[] PossibleCompletionsForString(MLPAutoCompleteTextField textField, string text);
    }

    [BaseType(typeof(NSObject))]
    [Model, Protocol]
    public interface MLPAutoCompletionObject
    {

        [Abstract]
        [Export("autocompleteString")]
        string AutocompleteString { get; }
    }

    [Model, BaseType(typeof(NSObject)), Protocol]
    public interface MLPAutoCompleteTextFieldDelegate
    {

        [Export("autoCompleteTextField:shouldStyleAutoCompleteTableView:forBorderStyle:"),
            DelegateName("ShouldStyleAutoCompleteTableView"),
            DefaultValue(true)]
        bool ShouldStyleAutoCompleteTableView(MLPAutoCompleteTextField textField, UITableView autoCompleteTableView, UITextBorderStyle borderStyle);

        [Export("autoCompleteTextField:shouldConfigureCell:withAutoCompleteString:withAttributedString:forAutoCompleteObject:forRowAtIndexPath:"),
            DelegateName("ShouldConfigureCell"),
            DefaultValue(true)]
        bool ShouldConfigureCell(MLPAutoCompleteTextField textField, UITableViewCell cell, string autocompleteString, NSAttributedString boldedString, MLPAutoCompletionObject autocompleteObject, NSIndexPath indexPath);

        [Export("autoCompleteTextField:didSelectAutoCompleteString:withAutoCompleteObject:forRowAtIndexPath:"),
            EventArgs("DidSelectAutoCompleteString")]
        void DidSelectAutoCompleteString(MLPAutoCompleteTextField textField, string selectedString, MLPAutoCompletionObject selectedObject, NSIndexPath indexPath);

        [Export("autoCompleteTextField:willShowAutoCompleteTableView:"),
            EventArgs("WillShowAutoCompleteTableView")]
        void WillShowAutoCompleteTableView(MLPAutoCompleteTextField textField, UITableView autoCompleteTableView);
    }

    [Model, BaseType(typeof(NSObject)), Protocol]
    public interface MLPAutoCompleteSortOperationDelegate
    {

        [Export("autoCompleteTermsDidSort:")]
        void AutoCompleteTermsDidSort(MLPAutoCompletionObject[] completions);
    }


    [Model, BaseType(typeof(NSObject)), Protocol]
    public interface MLPAutoCompleteFetchOperationDelegate
    {

        [Export("autoCompleteTermsDidFetch:")]
        void AutoCompleteTermsDidFetch(NSDictionary fetchInfo);
    }

    [BaseType(typeof(UITextField),
        Delegates = new string[] { "AutoCompleteDataSource", "AutoCompleteDelegate" },
        Events = new Type[] { typeof(MLPAutoCompleteTextFieldDataSource), typeof(MLPAutoCompleteTextFieldDelegate) })]
    public interface MLPAutoCompleteTextField
    {

        [Export("autoCompleteTableView", ArgumentSemantic.Retain)]
        UITableView AutoCompleteTableView { get; }

        [Export("autoCompleteDataSource", ArgumentSemantic.Retain)]
        MLPAutoCompleteTextFieldDataSource AutoCompleteDataSource { get; set; }

        [Export("autoCompleteDelegate", ArgumentSemantic.Assign)]
        MLPAutoCompleteTextFieldDelegate AutoCompleteDelegate { get; set; }

        [Export("autoCompleteFetchRequestDelay")]
        double AutoCompleteFetchRequestDelay { get; set; }

        [Export("sortAutoCompleteSuggestionsByClosestMatch")]
        bool SortAutoCompleteSuggestionsByClosestMatch { get; set; }

        [Export("applyBoldEffectToAutoCompleteSuggestions")]
        bool ApplyBoldEffectToAutoCompleteSuggestions { get; set; }

        [Export("reverseAutoCompleteSuggestionsBoldEffect")]
        bool ReverseAutoCompleteSuggestionsBoldEffect { get; set; }

        [Export("showTextFieldDropShadowWhenAutoCompleteTableIsOpen")]
        bool ShowTextFieldDropShadowWhenAutoCompleteTableIsOpen { get; set; }

        [Export("showAutoCompleteTableWhenEditingBegins")]
        bool ShowAutoCompleteTableWhenEditingBegins { get; set; }

        [Export("disableAutoCompleteTableUserInteractionWhileFetching")]
        bool DisableAutoCompleteTableUserInteractionWhileFetching { get; set; }

        [Export("autoCompleteTableAppearsAsKeyboardAccessory")]
        bool AutoCompleteTableAppearsAsKeyboardAccessory { get; set; }

        [Export("autoCompleteTableViewHidden")]
        bool AutoCompleteTableViewHidden { get; set; }

        [Export("autoCompleteFontSize")]
        float AutoCompleteFontSize { get; set; }

        [Export("autoCompleteBoldFontName", ArgumentSemantic.Retain)]
        string AutoCompleteBoldFontName { get; set; }

        [Export("autoCompleteRegularFontName", ArgumentSemantic.Retain)]
        string AutoCompleteRegularFontName { get; set; }

        [Export("maximumNumberOfAutoCompleteRows")]
        int MaximumNumberOfAutoCompleteRows { get; set; }

        [Export("partOfAutoCompleteRowHeightToCut")]
        float PartOfAutoCompleteRowHeightToCut { get; set; }

        [Export("autoCompleteRowHeight")]
        float AutoCompleteRowHeight { get; set; }

        [Export("autoCompleteTableOriginOffset", ArgumentSemantic.Assign)]
        SizeF AutoCompleteTableOriginOffset { get; set; }

        [Export("autoCompleteTableCornerRadius")]
        float AutoCompleteTableCornerRadius { get; set; }

        [Export("autoCompleteContentInsets", ArgumentSemantic.Assign)]
        UIEdgeInsets AutoCompleteContentInsets { get; set; }

        [Export("autoCompleteScrollIndicatorInsets", ArgumentSemantic.Assign)]
        UIEdgeInsets AutoCompleteScrollIndicatorInsets { get; set; }

        [Export("autoCompleteTableBorderColor", ArgumentSemantic.Retain)]
        UIColor AutoCompleteTableBorderColor { get; set; }

        [Export("autoCompleteTableBorderWidth")]
        float AutoCompleteTableBorderWidth { get; set; }

        [Export("autoCompleteTableBackgroundColor", ArgumentSemantic.Retain)]
        UIColor AutoCompleteTableBackgroundColor { get; set; }

        [Export("autoCompleteTableCellBackgroundColor", ArgumentSemantic.Retain)]
        UIColor AutoCompleteTableCellBackgroundColor { get; set; }

        [Export("autoCompleteTableCellTextColor", ArgumentSemantic.Retain)]
        UIColor AutoCompleteTableCellTextColor { get; set; }

        [Export("registerAutoCompleteCellNib:forCellReuseIdentifier:")]
        void RegisterAutoCompleteCellNib(UINib nib, string reuseIdentifier);

        [Export("registerAutoCompleteCellClass:forCellReuseIdentifier:")]
        void RegisterAutoCompleteCellClass(Class cellClass, string reuseIdentifier);
    }
}


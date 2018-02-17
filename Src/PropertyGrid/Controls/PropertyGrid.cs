
#region Namespace Imports

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

#endregion

namespace Vasu.Wpf.Controls
{
    /// <summary>
    /// Defines a <see cref="PropertyGrid"/> control.
    /// </summary>
    [ToolboxItem(true)]
	[TemplatePart(Name="PART_propertyItemsControl", Type=typeof(PropertyItemsControl)),
	TemplatePart(Name="PART_descriptionViewer",Type=typeof(ContentControl)),
	TemplatePart(Name = "PART_CategoryViewButton", Type = typeof(ToggleButton)),
	TemplatePart(Name = "PART_AlphabeticalViewButton", Type = typeof(ToggleButton))]
	public class PropertyGrid : Control
	{
		#region Ctor

		static PropertyGrid()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyGrid), new FrameworkPropertyMetadata(typeof(PropertyGrid)));
		}

		public PropertyGrid()
		{
			Loaded += new RoutedEventHandler(OnPropertyGridLoaded);
		}

		#endregion

		#region Dependency Properties

		public object SelectedObject
		{
			get { return (object)GetValue(SelectedObjectProperty); }
			set { SetValue(SelectedObjectProperty, value); }
		}

		public static readonly DependencyProperty SelectedObjectProperty =
			DependencyProperty.Register("SelectedObject", typeof(object), typeof(PropertyGrid), new UIPropertyMetadata(null));

		protected static void OnSelectedObjectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var propertyGrid = d as PropertyGrid;
			
			if (propertyGrid == null) return;

			propertyGrid.SelectedObjectChanged(e);
		}

		public PropertyField SelectedField
		{
			get { return (PropertyField)GetValue(SelectedFieldProperty); }
			set { SetValue(SelectedFieldProperty, value); }
		}

		public static readonly DependencyProperty SelectedFieldProperty =
			DependencyProperty.Register("SelectedField", typeof(PropertyField), typeof(PropertyGrid), new UIPropertyMetadata(null,OnSelectedFieldPropertyChanged));

		private static void OnSelectedFieldPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var pGrid = d as PropertyGrid;

			var nameBinding = new Binding("Name");
			nameBinding.Source = e.NewValue;
			pGrid.SetBinding(FieldNameProperty, nameBinding);

			Binding descBinding = new Binding("Description");
			descBinding.Source =e.NewValue;
			pGrid.SetBinding(FieldDescriptionProperty, descBinding);
		}

		public string FieldName
		{
			get { return (string)GetValue(FieldNameProperty); }
			set { SetValue(FieldNameProperty, value); }
		}

		public static readonly DependencyProperty FieldNameProperty =
			DependencyProperty.Register("FieldName", typeof(string), typeof(PropertyGrid), new UIPropertyMetadata(string.Empty));

		public string FieldDescription
		{
			get { return (string)GetValue(FieldDescriptionProperty); }
			set { SetValue(FieldDescriptionProperty, value); }
		}

		public static readonly DependencyProperty FieldDescriptionProperty =
			DependencyProperty.Register("FieldDescription", typeof(string), typeof(PropertyGrid), new UIPropertyMetadata(string.Empty));

		#endregion

		#region Overrides

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			propertyItemsControl = GetTemplateChild("PART_propertyItemsControl") as PropertyItemsControl;
			categoryViewButton = GetTemplateChild("PART_CategoryViewButton") as ToggleButton;
			alphabeticalViewButton = GetTemplateChild("PART_AlphabeticalViewButton") as ToggleButton;

			categoryViewButton.Checked += new RoutedEventHandler(OnCategoryViewButtonChecked);
			alphabeticalViewButton.Checked += new RoutedEventHandler(OnAlphabeticalViewButtonChecked);
		}

		protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonDown(e);
		}

		#endregion

		#region Events and Handlers

		void OnAlphabeticalViewButtonChecked(object sender, RoutedEventArgs e)
		{
			if (propertyFields == null || propertyItemsControl == null) return;

			categoryViewButton.IsChecked = false;

			var collectionView = CollectionViewSource.GetDefaultView(propertyFields);
			collectionView.GroupDescriptions.Clear();
			collectionView.SortDescriptions.Clear();

			propertyItemsControl.ItemsSource = propertyFields;
		}

		void OnCategoryViewButtonChecked(object sender, RoutedEventArgs e)
		{
			if (propertyFields == null || propertyItemsControl == null) return;

			alphabeticalViewButton.IsChecked = false;

			var collectionView = CollectionViewSource.GetDefaultView(propertyFields);
			collectionView.GroupDescriptions.Clear();
			collectionView.SortDescriptions.Clear();
			collectionView.GroupDescriptions.Add(new PropertyGroupDescription("CategoryName"));
			collectionView.SortDescriptions.Add(new SortDescription("CategoryName", ListSortDirection.Ascending));
			collectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

			propertyItemsControl.ItemsSource = collectionView;
		}

		void OnPropertyGridLoaded(object sender, RoutedEventArgs e)
		{
			Loaded -= new RoutedEventHandler(OnPropertyGridLoaded);

			PopulatePropertyFields();

			propertyItemsControl.ItemsSource = propertyFields;

			propertyItemsControl.SelectedValue = propertyItemsControl.Items.OfType<PropertyField>().FirstOrDefault();

			Binding fieldBinding = new Binding("SelectedValue");
			fieldBinding.Source = propertyItemsControl;
			SetBinding(SelectedFieldProperty, fieldBinding);
		}

		protected void SelectedObjectChanged(DependencyPropertyChangedEventArgs e)
		{
			PopulatePropertyFields();
		}

		internal void RaiseValueChanged(ValueChangedEventArgs e)
		{
			var handler = ValueChanged;

			if (handler == null) return;

			handler(this, e);
		}

		/// <summary>
		/// Raised when a property value is changed by the user. Subscribe to this event and sync values with the target object.
		/// </summary>
		public event ValueChangedEventHandler ValueChanged;

		#endregion

		#region Implementations

		private void PopulatePropertyFields()
		{
			if (SelectedObject == null) return;

			if (propertyFields != null)
				propertyFields.ForEach<PropertyField>(f => f.Dispose());

			#region ** Commented Out **

			//propertyFields = TypeDescriptor.GetProperties(SelectedObject)
			//    .AsEnumerable<PropertyDescriptor>()
			//    .Where(p => p.IsBrowsable && !p.DesignTimeOnly)
			//    .Select(p => new PropertyField { 
			//        PropertyDescriptor =p, 
			//        PropertyGrid = this, 
			//        SourceObject = SelectedObject,
			//        Value = p.GetValue(SelectedObject)
			//    });

			#endregion

			propertyFields = SelectedObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.OrderBy(p => p.Name)
				.Select(p => new PropertyField { 
					 PropertyInfo = p,
					 SourceObject = SelectedObject,
					 PropertyGrid = this,
					 Value = p.GetValue(SelectedObject,null)
				});
		}

		private bool IsBrowsable(PropertyInfo p)
		{
		   return 
			   p.Attributes!= PropertyAttributes.None &&
			   (p.GetCustomAttributes(typeof(BrowsableAttribute), false)[0] as BrowsableAttribute).Browsable;
		}

		#endregion

		#region Fields

		private IEnumerable<PropertyField> propertyFields = null;
		private PropertyItemsControl propertyItemsControl = null;
		private ToggleButton categoryViewButton = null;
		private ToggleButton alphabeticalViewButton = null;

		#endregion
	}
}

using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class PasswordListCommandBar : UserControl
    {
        private DependencyObject[] _anis;

        public PasswordListCommandBar()
        {
            this.InitializeComponent();

            Loaded += PasswordListCommandBar_Loaded;
        }


        public bool IsSelectAll
        {
            get { return (bool)GetValue(IsSelectAllProperty); }
            set { SetValue(IsSelectAllProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelectAll.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectAllProperty =
            DependencyProperty.Register("IsSelectAll", typeof(bool), typeof(PasswordListCommandBar), new PropertyMetadata(false));


        public bool IsItemSelected
        {
            get { return (bool)GetValue(IsItemSelectedProperty); }
            set { SetValue(IsItemSelectedProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IsItemSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsItemSelectedProperty =
            DependencyProperty.Register("IsItemSelected", typeof(bool), typeof(PasswordListCommandBar), new PropertyMetadata(false));

        public bool IsMultiSelect
        {
            get { return (bool)GetValue(IsMultiSelectProperty); }
            set { SetValue(IsMultiSelectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMultiSelect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMultiSelectProperty =
            DependencyProperty.Register("IsMultiSelect", typeof(bool), typeof(PasswordListCommandBar), new PropertyMetadata(false));


        private void PasswordListCommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= PasswordListCommandBar_Loaded;
            var behavior0 = Interaction.GetBehaviors(MultipleButton)[0] as EventTriggerBehavior;
            var behavior1 = Interaction.GetBehaviors(MultipleButton)[1] as EventTriggerBehavior;
            var anis = new DependencyObject[] { behavior0.Actions[1], behavior0.Actions[2] };
            behavior0.Actions.RemoveAt(2);
            behavior0.Actions.RemoveAt(1);
            behavior0.Actions.Add(behavior1.Actions[0]);
            behavior0.Actions.Add(behavior1.Actions[1]);
            _anis = anis;
        }

        private void MultipleButton_Click(object sender, RoutedEventArgs e)
        {
            IsMultiSelect = !IsMultiSelect;
            OnIsMultipleSelectChange();
        }

        private void OnIsMultipleSelectChange()
        {
            var behavior = Interaction.GetBehaviors(MultipleButton)[0] as EventTriggerBehavior;
            var anis = new DependencyObject[] { behavior.Actions[1], behavior.Actions[2] };
            behavior.Actions.RemoveAt(2);
            behavior.Actions.RemoveAt(1);
            behavior.Actions.Add(_anis[0]);
            behavior.Actions.Add(_anis[1]);
            _anis = anis;
        }

        public void SwitchMultipleManual()
        {
            IsMultiSelect = !IsMultiSelect;
            OnIsMultipleSelectChange();

            var behavior = Interaction.GetBehaviors(MultipleButton)[0] as EventTriggerBehavior;
            foreach (StartAnimationAction action in behavior.Actions)
            {
                action.Animation.Start();
            }
        }

        public event TypedEventHandler<Button, object> OnSelectAll;
        public event TypedEventHandler<Button, object> OnMoveTo;
        public event TypedEventHandler<Button, object> OnAdd;
        public event TypedEventHandler<Button, object> OnDelete;

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            OnSelectAll?.Invoke(sender as Button, null);
        }

        private void MoveToButton_Click(object sender, RoutedEventArgs e)
        {
            OnMoveTo?.Invoke(sender as Button, null);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            OnDelete?.Invoke(sender as Button, null);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            OnAdd?.Invoke(sender as Button, null);
        }
    }
}

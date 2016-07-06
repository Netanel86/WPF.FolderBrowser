using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using WPF.Common.Enums;

namespace WPF.Common.UI.Controls
{
    /// <summary>
    /// A Togglable TextBox control
    /// </summary>
    /// <remarks>
    /// Toggles between Edit and ReadOnly modes which are represented by a
    /// bindable dependency property <paramref name="Mode"/>.
    /// </remarks>
    public class TogglableTextBox : TextBox
    {
        #region Dependency Properties
        /// <summary>
        ///  Identifies the TogglableTextBox.Mode dependency property.
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(eTextControlMode), typeof(TogglableTextBox), new PropertyMetadata(eTextControlMode.ReadOnly));
        #endregion Dependency Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of <typeparamref name="TogglableTextBox"/>
        /// </summary>
        public TogglableTextBox()
        {
            base.BorderBrush = new SolidColorBrush(Colors.White);
            base.Background = new SolidColorBrush(Colors.White);
            textBoxModeHandler(this.Mode);
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets/sets the the current mode of the textbox
        /// </summary>
        public eTextControlMode Mode
        {
            get { return (eTextControlMode)base.GetValue(ModeProperty); }
            set { base.SetValue(ModeProperty, value); }
        }
        #endregion Properties

        #region Fields
        private string m_OldText = String.Empty;
        
        private Thickness m_NoBorder = new Thickness(0);
        
        private Thickness m_StandardThickness = new Thickness(1);
        #endregion Fields

        #region Methods
        private void textBoxModeHandler(eTextControlMode i_Mode)
        {
            switch (i_Mode)
            {
                case eTextControlMode.Editable:
                    initializeEditMode();
                    break;
                case eTextControlMode.ReadOnly:
                    initializeReadOnlyMode();
                    break;
            }
        }
        
        private void initializeEditMode()
        {
            m_OldText = base.Text;
            base.Background.Opacity = 1;
            base.BorderThickness = m_StandardThickness;
            base.Cursor = Cursors.IBeam;
            base.IsReadOnly = false;
            base.Focusable = true;
            base.Focus();
            base.SelectAll();
        }

        private void initializeReadOnlyMode()
        {
            base.Background.Opacity = 0;
            base.BorderThickness = m_NoBorder;
            base.Cursor = Cursors.Arrow;
            base.IsReadOnly = true;
            base.Focusable = false;
            base.Select(0, 0);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            this.Mode = eTextControlMode.ReadOnly;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name.CompareTo("Mode") == 0)
            {
                textBoxModeHandler(this.Mode);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Enter)
            {
                this.Mode = eTextControlMode.ReadOnly;
            }
            if (e.Key == Key.Escape)
            {
                base.Text = m_OldText;
                this.Mode = eTextControlMode.ReadOnly;
            }

        }
        #endregion Methods
    }
}

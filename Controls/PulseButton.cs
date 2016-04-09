using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace FolderBrowserDialog.Controls
{
    /// <summary>
    /// Button with image and built-in pulse animation.
    /// animation enabled only while control has mouse focus and enabled.
    /// while control disabled image is half transparent.
    /// </summary>
    /// <remarks>
    /// need's to be provided with an image. exposes two
    /// new Dependency Properties: Icon, ScaleMax.
    /// To use the button properly you need to include its XAML file as a resource.
    /// </remarks>
    /// <example>
    /// Example for adding resources;
    /// Add the button resource dictionary in the required Resource scope:
    /// <ResourceDictionary Source="/YourPath/PulseButton.xaml" />
    /// Exmaple to use the button in your view:
    /// <ButtonNamespace:PulseButton Icon="{Binding MyIconPath}" Command="{Binding MyCommand}" Height="20" Width="20" Margin="5,0,0,0" />
    /// </example>
    public class PulseButton : Button
    {
        #region Dependency Properties
        /// <summary>
        /// Gets/sets an <typeparamref name="ImageSource"/> representing
        /// the button's view.
        /// </summary>
        /// <remarks>
        /// Bindable property, This property must be set, otherwise the button is transperant.
        /// </remarks>
        public ImageSource Icon
        {
            get { return base.GetValue(IconProperty) as ImageSource; }
            set { base.SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(PulseButton));

        /// <summary>
        /// Gets/sets the maximum relative scale for the pulse animation.
        /// </summary>
        /// <remarks>
        /// Bindable property, default is x1.3 of the original size.
        /// </remarks>
        public double ScaleMax
        {
            get { return (double)base.GetValue(ScaleMaxProperty); }
            set { base.SetValue(ScaleMaxProperty, value); }
        }
        
        public static DependencyProperty ScaleMaxProperty =
            DependencyProperty.Register("ScaleMax", typeof(double), typeof(PulseButton));
        #endregion Dependency Properties
        
        #region Members
        private const double v_ScaleMaxDefault = 1.3;
       
        private const double v_ScaleMinDefault = 1;
        #endregion Members

        #region Constructors
        public PulseButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PulseButton), new FrameworkPropertyMetadata(typeof(PulseButton)));
            this.ScaleMax = v_ScaleMaxDefault;
            
        }
        #endregion Constructors

        #region On Event's Overrides
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //initialize animation components
            initializeStoryBoard();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            m_StoryBoard.Begin();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            m_StoryBoard.Stop();
        }
        #endregion On Event's Overrides

        #region Animation
        private Storyboard m_StoryBoard;

        private void initializeStoryBoard()
        {
            m_StoryBoard = new Storyboard();
            ScaleTransform scaleTransform = new ScaleTransform(v_ScaleMinDefault, v_ScaleMinDefault);
            base.RenderTransform = scaleTransform;
            base.RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleAnimation animationX = new DoubleAnimation()
            {
                From = v_ScaleMinDefault,
                To = this.ScaleMax,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromMilliseconds(600))
            };
            m_StoryBoard.Children.Add(animationX);

            DoubleAnimation animationY = new DoubleAnimation()
            {
                From = v_ScaleMinDefault,
                To = this.ScaleMax,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromMilliseconds(600))
            };
            m_StoryBoard.Children.Add(animationY);

            Storyboard.SetTargetProperty(animationX, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTargetProperty(animationY, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTarget(animationX, this);
            Storyboard.SetTarget(animationY, this);
        }
        #endregion Animation
    }
}

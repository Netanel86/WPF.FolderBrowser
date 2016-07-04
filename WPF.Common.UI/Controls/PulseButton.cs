using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WPF.Common.UI.Controls
{
    /// <summary>
    /// Button with image and built-in pulse animation.
    /// </summary>
    /// <remarks>
    /// animation enabled only while control has mouse focus and enabled.
    /// while control disabled image is half transparent.
    /// need's to be provided with an image. exposes two
    /// new Dependency Properties: Icon, ScaleMax.
    /// To use the button properly you need to include its XAML file as a resource.
    /// </remarks>
    /// <example>
    /// Example for adding resources;
    /// Add the button resource dictionary in the required Resource scope:
    /// <ResourceDictionary Source="pack://application:,,,/WPF.Common;component/Controls/PulseButton.xaml" />
    /// Exmaple to use the button in your view:
    /// <ButtonNamespace:PulseButton Icon="{Binding MyIconPath}" Command="{Binding MyCommand}" Height="20" Width="20" .../>
    /// </example>
    public class PulseButton : Button
    {
        #region Dependency Properties
        /// <summary>
        ///  Identifies the PulseButton.Icon dependency property.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(PulseButton));

        /// <summary>
        ///  Identifies the PulseButton.ScaleMax dependency property.
        /// </summary>
        public static DependencyProperty ScaleMaxProperty =
            DependencyProperty.Register("ScaleMax", typeof(double), typeof(PulseButton));
        #endregion Dependency Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of <typeparamref name="PulseButton"/>
        /// </summary>
        public PulseButton()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(PulseButton), new FrameworkPropertyMetadata(typeof(PulseButton)));
            this.ScaleMax = k_ScaleMaxDefault;

        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets/sets an <typeparamref name="ImageSource"/> representing
        /// the button's view.
        /// </summary>
        /// <remarks>
        /// Bindable property, This property must be set, otherwise the button is transperent.
        /// </remarks>
        public ImageSource Icon
        {
            get { return base.GetValue(IconProperty) as ImageSource; }
            set { base.SetValue(IconProperty, value); }
        }

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
        #endregion Properties
       
        #region Fields
        private const double k_ScaleMaxDefault = 1.3;
       
        private const double k_ScaleMinDefault = 1;
        
        private Storyboard m_StoryBoard;
        #endregion Fields

        #region Methods
        private void initializeStoryBoard()
        {
            m_StoryBoard = new Storyboard();
            ScaleTransform scaleTransform = new ScaleTransform(k_ScaleMinDefault, k_ScaleMinDefault);
            base.RenderTransform = scaleTransform;
            base.RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleAnimation animationX = new DoubleAnimation()
            {
                From = k_ScaleMinDefault,
                To = this.ScaleMax,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromMilliseconds(600))
            };
            m_StoryBoard.Children.Add(animationX);

            DoubleAnimation animationY = new DoubleAnimation()
            {
                From = k_ScaleMinDefault,
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
        #endregion Methods
    }
}

using SnapShotUndo.Core;
using SnapShotUndo.Model;
using SnapShotUndo.Strategy;
using SnapShotUndo.Strategy.BaseStaretegt;
using SnapShotUndo.Utility;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace SnapShotUndo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly SnapshotController _controller = new();


        private readonly SnapshotController _CompoSiteController = new();

        private readonly ObservableCollection<Person> _people = new ObservableCollection<Person>();


        List<ISnapshotStrategy> _snapStaratesyList = new();


        List<CheckBox> checkBoxes;
        List<RadioButton> radioButtons;
        List<Slider> Sliders = new();

        public MainWindow()
        {
            InitializeComponent();

            // 初期戦略は DataGrid 用
            var gridStrategy = new DataGridSnapshotStrategy(dataGrid1, _people);

            var textStrategy = new TextBoxSnapshotStrategy(textBox1);



            dataGrid1.ItemsSource = _people;

            checkBoxes = new List<CheckBox>();
            radioButtons = new List<RadioButton>();

            this.WalkInChildren(child =>
            {
                if (child is CheckBox checkBox)
                {
                    checkBoxes.Add(checkBox);
                }
            });



            var checkStratesy = new CheckBoxStrategy(checkBoxes);

            this.WalkInChildren(child =>
            {
                if (child is RadioButton radios)
                {
                    radioButtons.Add(radios);
                }
            });



            this.WalkInChildren(child =>
            {
                if (child is Slider slider)
                {
                    Sliders.Add(slider);
                }
            });

            var sliderStratesy = new SliderStrategy(Sliders);

            var radioStaratesy = new RadioGroupStrategy(radioButtons);


            _snapStaratesyList.Add(gridStrategy);
            _snapStaratesyList.Add(textStrategy);
            _snapStaratesyList.Add(checkStratesy);
            _snapStaratesyList.Add(radioStaratesy);
            _snapStaratesyList.Add(sliderStratesy);



            var compoStrategy = new CompositeStrategy(textStrategy, radioStaratesy, checkStratesy, sliderStratesy, gridStrategy);



            _CompoSiteController.SetStrategy(compoStrategy);


            _controller.SetStrategy(_snapStaratesyList);






            SheepSlider.Value = 255;
            SheepSlider2.Value = 255;
            SheepSlider3.Value = 255;

        }


        private void CaptureButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.IndividualCapture();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.Undo();
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.Redo();
        }



        private void AddDatadButton_Click(object sender, RoutedEventArgs e)
        {
            var newPerson = PersonCreater.RandomPerson();
            _people.Add(newPerson);
        }

        private void SheepSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Windowの背景ブラシから現在の色を取得
            var currentBrush = (SolidColorBrush)this.Background;
            Color currentColor = currentBrush.Color;

            // どのスライダーかに応じて戦略を選択
            var strategy = ChangeRDBHelper.Red; // 例：赤スライダーの場合
            Brush newBrush = strategy.Change(e, currentColor);

            // 背景更新
            this.Background = newBrush;
        }

        private void SheepSlider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Windowの背景ブラシから現在の色を取得
            var currentBrush = (SolidColorBrush)this.Background;
            Color currentColor = currentBrush.Color;

            // どのスライダーかに応じて戦略を選択
            var strategy = ChangeRDBHelper.Green; // 例：緑スライダーの場合
            Brush newBrush = strategy.Change(e, currentColor);

            // 背景更新
            this.Background = newBrush;
        }

        private void SheepSlider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Windowの背景ブラシから現在の色を取得
            var currentBrush = (SolidColorBrush)this.Background;
            Color currentColor = currentBrush.Color;

            // どのスライダーかに応じて戦略を選択
            var strategy = ChangeRDBHelper.Blue; // 例：青スライダーの場合
            Brush newBrush = strategy.Change(e, currentColor);

            // 背景更新
            this.Background = newBrush;
        }

        private void CompositeCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            _CompoSiteController.CompositeCapture();


        }

        private void CompoSiteUndoButton_Click(object sender, RoutedEventArgs e)
        {
            _CompoSiteController.Undo();
        }

        private void CompoSiteRedoButton_Click(object sender, RoutedEventArgs e)
        {
            _CompoSiteController.Redo();
        }
    }
}
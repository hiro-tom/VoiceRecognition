using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Recognition;

namespace VoiceRecognition
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        SpeechRecognitionEngine engine;
        public MainWindow()
        {
            InitializeComponent();
            vm = new ViewModel();
            this.DataContext = vm;
            engine = new SpeechRecognitionEngine();
            engine.SpeechRecognized += Engine_SpeechRecognized;//認識処理
            engine.SpeechHypothesized += Engine_SpeechHypothesized;//推定処理
            engine.LoadGrammar(new DictationGrammar());//ディクテーション用の辞書
            engine.SetInputToDefaultAudioDevice();//エンジンの入力
            engine.RecognizeAsync(RecognizeMode.Multiple);//開始
        }

        private void Engine_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            vm.Voice = e.Result.Text;
        }

        private void Engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            vm.Voice = e.Result.Text;
            vm.Text = e.Result.Text;
        }
    }
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        string _Text = "";
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value + "\r\n" + Text;
                OnPropertyChanged("Text");
            }
        }
        string _Voice = "";
        public string Voice
        {
            get
            {
                return _Voice;
            }
            set
            {
                _Voice = value;
                OnPropertyChanged("Voice");
            }
        }
    }
}

using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ReactivePropertyTestApp2
{
    public class MainWindowViewModel
    {
        public ReactiveCommand ChangeCanExecute { get; set; }
        public ReactiveCommand Command1 { get; set; }
        public AsyncReactiveCommand Command2 { get; set; }

        public ReactiveProperty<bool> CanExecute { get; set; } = new ReactiveProperty<bool>(false);

        public MainWindowViewModel()
        {
            ChangeCanExecute = new ReactiveCommand().WithSubscribe(() =>
            {
                // こっちだとCommand1とCommand2の、実行可否は正しく動作する。
                //Task.Run(() => CanExecute.Value = !CanExecute.Value);

                // こっちだとCommand1は正しく動作するが、Command2が正しく動作しない。
                CanExecute.Value = !CanExecute.Value;
            });

            Command1 = CanExecute.ToReactiveCommand().WithSubscribe(() =>
            {
                MessageBox.Show("OK");
            });

            Command2 = CanExecute.ToAsyncReactiveCommand().WithSubscribe(async () =>
            {
                await Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("OK");
                }));
            });

        }
    }
}

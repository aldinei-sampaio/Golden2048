using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golden2048.Wpf
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class GameCell
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Value { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TestModel
    {
        public ObservableCollection<GameCell> Items { get; set; }

        public DelegateCommand ChangePositionCommand { get; }
        public DelegateCommand AddRemoveCommand { get; }
        public DelegateCommand ChangeValueCommand { get; }

        public TestModel()
        {
            Items = new ObservableCollection<GameCell>()
            {
                new GameCell { Value = 2, Left = 0, Top = 0},
                new GameCell { Value = 4, Left = 100, Top = 200}
            };

            ChangePositionCommand = new DelegateCommand(
                i =>
                {
                    var item = Items.First();
                    item.Left = (item.Left == 0) ? 200 : 0;                    
                }
            );

            AddRemoveCommand = new DelegateCommand(
                i =>
                {
                    if (Items.Count < 3)
                    {
                        Items.Add(new GameCell { Value = 512, Left = 200, Top = 300 });
                    }
                    else
                    {
                        Items.RemoveAt(2);
                    }
                }
            );

            ChangeValueCommand = new DelegateCommand(
                i =>
                {
                    var item = Items.First();
                    item.Value = item.Value == 2 ? 4 : 2;
                }
            );
        }
    }
}

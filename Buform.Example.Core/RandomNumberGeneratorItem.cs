using System;
using System.Linq.Expressions;
using System.Windows.Input;
using MvvmCross.Commands;

namespace Buform.Example.Core
{
    public sealed class RandomNumberGeneratorItem : FormItem<int>
    {
        private const int MinValue = 0;
        private const int MaxValue = 1000;

        private readonly Random _random;

        public string? Label { get; set; }
        public ICommand GenerateCommand { get; }

        public RandomNumberGeneratorItem(Expression<Func<int>> property) : base(property)
        {
            _random = new Random();

            GenerateCommand = new MvxCommand(Regenerate);
        }

        private void Regenerate()
        {
            Value = _random.Next(MinValue, MaxValue);
        }
    }
}
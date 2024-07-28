namespace Illusion.Controls;

record ChangeFlags(
  bool Data = false,
  bool Filter = false,
  bool CollapseParenthesis = false,
  bool Grouping = false,
  bool ShowOtherDevs = false,
  bool ShowIncome = false);

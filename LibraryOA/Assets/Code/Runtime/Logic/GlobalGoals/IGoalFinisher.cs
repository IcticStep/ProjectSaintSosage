using Cysharp.Threading.Tasks;

namespace Code.Runtime.Logic.GlobalGoals
{
    public interface IGoalFinisher
    {
        UniTask FinishAsync();
    }
}
using System.Threading.Tasks;
using DG.Tweening;

namespace App.Scripts.Modules.TweenHelper
{
    public static class ExtensionTaskTween
    {
        public static Task Await(this Tween animation)
        {
            if (animation is null || animation.IsComplete())
            {
                return Task.CompletedTask;
            }

            TaskCompletionSource<bool> tsk = new();
            _ = animation.OnComplete(() => tsk.TrySetResult(true));

            return tsk.Task;
        }
    }
}

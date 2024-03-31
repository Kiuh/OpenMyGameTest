using System.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Modules.TaskExtensions
{
    public static class ExtensionsTask
    {
        public static void Forget(this Task task)
        {
            _ = task.ContinueWith(completedTask =>
            {
                if (completedTask.Exception is null)
                {
                    return;
                }

                foreach (System.Exception exception in completedTask.Exception.InnerExceptions)
                {
                    Debug.LogException(exception);
                }
            });
        }
    }
}
